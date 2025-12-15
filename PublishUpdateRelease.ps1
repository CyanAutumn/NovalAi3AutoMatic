# 自动打包并发布更新到 GitHub Releases（NovalAi3AutoMatic）
# 依赖：
# - GitHub CLI（gh），并已执行 `gh auth login`
# - MSBuild（Visual Studio / Build Tools），或 dotnet（可选）
# - 7-Zip（可选，用于更快/更高压缩率的 zip；未安装则使用 PowerShell 自带的 Compress-Archive）
#
# 用法（在本仓库根目录执行）：
#   powershell -ExecutionPolicy Bypass -File .\PublishUpdateRelease.ps1
# 或指定版本：
#   powershell -ExecutionPolicy Bypass -File .\PublishUpdateRelease.ps1 -Version 2.2.9
#
# 默认行为：
# - 从本仓库最新 release tag 解析版本并自动 +1（例如 2.2.8 -> 2.2.9）
# - 同步更新 Properties\AssemblyInfo.cs（AssemblyVersion/AssemblyFileVersion），并执行 Release 构建
# - 将 bin\Release 目录内容打包为 dist\AutoNai3Tools.zip
# - 在仓库根目录生成/更新 version.xml（供 Autoupdater.NET.Official 使用），并上传到 release 资产
#
# 客户端更新 XML：
#   https://github.com/CyanAutumn/NovalAi3AutoMatic/releases/latest/download/version.xml

[CmdletBinding()]
param(
    [string]$Repo = "CyanAutumn/NovalAi3AutoMatic",
    [string]$ProjectFile = "AutoNai3Tools.csproj",
    [string]$AssemblyInfoFile = "Properties\\AssemblyInfo.cs",
    [string]$BuildDir = "bin\\Release",
    [string]$OutDir = "dist",
    [string]$TagPrefix = "",
    [string]$Version = "",
    [string]$ZipAssetName = "AutoNai3Tools.zip",
    [string]$SevenZipPath = "",
    [string]$VersionXmlPath = "version.xml",
    [string]$ChangelogUrl = "",
    [string]$Mandatory = "",
    [switch]$SkipProjectVersionUpdate,
    [switch]$SkipBuild
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

try {
    [Console]::OutputEncoding = [System.Text.Encoding]::UTF8
    $OutputEncoding = [System.Text.Encoding]::UTF8
}
catch {
    # ignore
}

function Get-ScriptBaseDir {
    $dir = $PSScriptRoot
    if (-not [string]::IsNullOrWhiteSpace($dir)) {
        return $dir
    }

    if (-not [string]::IsNullOrWhiteSpace($PSCommandPath)) {
        return (Split-Path -Parent $PSCommandPath)
    }

    if ($MyInvocation -and $MyInvocation.MyCommand -and -not [string]::IsNullOrWhiteSpace($MyInvocation.MyCommand.Path)) {
        return (Split-Path -Parent $MyInvocation.MyCommand.Path)
    }

    return (Get-Location).Path
}

function Resolve-SevenZipPath {
    param([string]$PreferredPath)

    if (-not [string]::IsNullOrWhiteSpace($PreferredPath)) {
        if (Test-Path -LiteralPath $PreferredPath -PathType Leaf) {
            return $PreferredPath
        }
        throw "找不到 7z.exe：$PreferredPath"
    }

    $cmd = Get-Command 7z -ErrorAction SilentlyContinue
    if ($cmd) {
        return $cmd.Source
    }

    $candidates = @()
    if ($env:ProgramFiles) {
        $candidates += (Join-Path $env:ProgramFiles "7-Zip\\7z.exe")
    }
    $programFilesX86 = ${env:ProgramFiles(x86)}
    if ($programFilesX86) {
        $candidates += (Join-Path $programFilesX86 "7-Zip\\7z.exe")
    }

    foreach ($p in $candidates) {
        if ($p -and (Test-Path -LiteralPath $p -PathType Leaf)) {
            return $p
        }
    }

    throw "未检测到 7z.exe。请安装 7-Zip 并确保 7z.exe 在 PATH 中，或用 -SevenZipPath 指定路径。"
}

function Get-LatestReleaseTag {
    param([string]$Repo)

    $json = & gh release list --repo $Repo --limit 1 --json tagName 2>$null
    if ([string]::IsNullOrWhiteSpace($json)) {
        return $null
    }

    $data = $json | ConvertFrom-Json
    if ($null -eq $data -or $data.Count -eq 0) {
        return $null
    }

    return $data[0].tagName
}

function Increment-Version {
    param([string]$VersionString)

    $v = if ($null -eq $VersionString) { "" } else { $VersionString }
    $v = $v.Trim()
    if ([string]::IsNullOrWhiteSpace($v)) {
        throw "版本号为空，无法递增。"
    }

    $parts = $v.Split(".")
    if ($parts.Count -lt 3) {
        throw "版本号格式不正确：$v（至少应为 x.y.z 或 x.y.z.n）"
    }

    $lastIndex = $parts.Count - 1
    $parts[$lastIndex] = (([int]$parts[$lastIndex]) + 1).ToString()
    return ($parts -join ".")
}

function Normalize-Version3 {
    param([string]$VersionString)

    $v = if ($null -eq $VersionString) { "" } else { $VersionString.Trim() }
    if ([string]::IsNullOrWhiteSpace($v)) {
        throw "版本号为空。"
    }

    $parts = $v.Split(".")
    if ($parts.Count -ne 3) {
        throw "版本号格式不正确：$v（应为 x.y.z）"
    }

    foreach ($p in $parts) {
        if (-not [regex]::IsMatch($p, '^\d+$')) {
            throw "版本号格式不正确：$v（必须为纯数字分段，例如 2.2.8）"
        }
    }

    return ($parts -join ".")
}

function Derive-NextVersionFromLatestRelease {
    param([string]$Repo)

    $latestTag = Get-LatestReleaseTag -Repo $Repo
    if ([string]::IsNullOrWhiteSpace($latestTag)) {
        return $null
    }

    $m = [regex]::Match($latestTag, "^(?<prefix>[^0-9]*)(?<version>[0-9].*)$")
    if (-not $m.Success) {
        throw "无法从最新 tag 解析版本号：$latestTag"
    }

    $prefix = $m.Groups["prefix"].Value
    $latestVersion = $m.Groups["version"].Value
    $nextVersion = Increment-Version -VersionString $latestVersion

    return [pscustomobject]@{
        Prefix = $prefix
        LatestTag = $latestTag
        Version = $nextVersion
    }
}

function Update-AssemblyInfoVersion {
    param(
        [string]$AssemblyInfoPath,
        [string]$NewVersion
    )

    if ([string]::IsNullOrWhiteSpace($AssemblyInfoPath)) {
        throw "AssemblyInfoPath 不能为空。"
    }
    if ([string]::IsNullOrWhiteSpace($NewVersion)) {
        throw "NewVersion 不能为空。"
    }
    if (-not (Test-Path -LiteralPath $AssemblyInfoPath -PathType Leaf)) {
        throw "找不到 AssemblyInfo 文件：$AssemblyInfoPath"
    }

	    $text = [System.IO.File]::ReadAllText($AssemblyInfoPath)
	    $updated = $text

	    $assemblyVersionPattern = '(?m)^\s*\[assembly:\s*AssemblyVersion\("[^"]*"\)\]\s*$'
	    $assemblyFileVersionPattern = '(?m)^\s*\[assembly:\s*AssemblyFileVersion\("[^"]*"\)\]\s*$'
	    $assemblyVersionReplacement = ('[assembly: AssemblyVersion("{0}")]' -f $NewVersion)
	    $assemblyFileVersionReplacement = ('[assembly: AssemblyFileVersion("{0}")]' -f $NewVersion)

	    $updated = [regex]::Replace($updated, $assemblyVersionPattern, $assemblyVersionReplacement)
	    $updated = [regex]::Replace($updated, $assemblyFileVersionPattern, $assemblyFileVersionReplacement)

    if ($updated -ne $text) {
        [System.IO.File]::WriteAllText($AssemblyInfoPath, $updated, [System.Text.Encoding]::UTF8)
        Write-Host "已更新版本号：$AssemblyInfoPath -> $NewVersion"
    }
    else {
        throw "未在 $AssemblyInfoPath 中找到 AssemblyVersion/AssemblyFileVersion，无法更新版本号。"
    }
}

function Invoke-ProjectBuild {
    param(
        [string]$ProjectPath,
        [string]$Configuration = "Release",
        [string]$Platform = "AnyCPU"
    )

    if (-not (Test-Path -LiteralPath $ProjectPath -PathType Leaf)) {
        throw "找不到项目文件：$ProjectPath"
    }

    $msbuild = Get-Command msbuild -ErrorAction SilentlyContinue
    if ($msbuild) {
        Write-Host "开始构建：msbuild /restore /p:Configuration=$Configuration /p:Platform=$Platform $ProjectPath"
        & $msbuild.Source $ProjectPath /restore /t:Build /p:Configuration=$Configuration /p:Platform=$Platform | Out-Host
        if ($LASTEXITCODE -ne 0) {
            throw "msbuild 失败（exit code: $LASTEXITCODE）"
        }
        return
    }

    $dotnet = Get-Command dotnet -ErrorAction SilentlyContinue
    if ($dotnet) {
        Write-Host "开始构建：dotnet msbuild -restore /p:Configuration=$Configuration /p:Platform=$Platform $ProjectPath"
        & dotnet msbuild $ProjectPath -restore /t:Build /p:Configuration=$Configuration /p:Platform=$Platform | Out-Host
        if ($LASTEXITCODE -ne 0) {
            throw "dotnet msbuild 失败（exit code: $LASTEXITCODE）"
        }
        return
    }

    throw "未检测到 msbuild 或 dotnet。请安装 Visual Studio / Build Tools 或 .NET SDK。"
}

function Build-VersionXml {
    param(
        [string]$Version,
        [string]$Url,
        [string]$Changelog,
        [string]$Mandatory
    )

    $vEsc = [System.Security.SecurityElement]::Escape($Version)
    $uEsc = [System.Security.SecurityElement]::Escape($Url)
    $cEsc = [System.Security.SecurityElement]::Escape($Changelog)
    $mEsc = [System.Security.SecurityElement]::Escape($Mandatory)

    return @"
<?xml version="1.0" encoding="UTF-8"?>
<item>
  <version>$vEsc</version>
  <url>$uEsc</url>
  <changelog>$cEsc</changelog>
  <mandatory>$mEsc</mandatory>
</item>
"@
}

function Get-Existing-VersionXmlSettings {
    param(
        [string]$XmlPath,
        [string]$DefaultChangelog,
        [string]$DefaultMandatory
    )

    if (-not (Test-Path -LiteralPath $XmlPath -PathType Leaf)) {
        return [pscustomobject]@{
            Changelog = $DefaultChangelog
            Mandatory = $DefaultMandatory
        }
    }

    try {
        [xml]$xml = [System.IO.File]::ReadAllText($XmlPath)
        $existingChangelog = if ($xml.item.changelog) { [string]$xml.item.changelog } else { "" }
        $existingMandatory = if ($xml.item.mandatory) { [string]$xml.item.mandatory } else { "" }

        return [pscustomobject]@{
            Changelog = if ([string]::IsNullOrWhiteSpace($existingChangelog)) { $DefaultChangelog } else { $existingChangelog }
            Mandatory = if ([string]::IsNullOrWhiteSpace($existingMandatory)) { $DefaultMandatory } else { $existingMandatory }
        }
    }
    catch {
        return [pscustomobject]@{
            Changelog = $DefaultChangelog
            Mandatory = $DefaultMandatory
        }
    }
}

$baseDir = Get-ScriptBaseDir
Push-Location $baseDir
try {
    $gh = Get-Command gh -ErrorAction SilentlyContinue
    if (-not $gh) {
        throw "未检测到 GitHub CLI（gh）。请先安装：https://cli.github.com/ 并执行 `gh auth login`。"
    }

    $resolvedProjectFile = $ProjectFile
    if (-not [System.IO.Path]::IsPathRooted($resolvedProjectFile)) {
        $resolvedProjectFile = Join-Path $baseDir $resolvedProjectFile
    }

    $resolvedAssemblyInfoFile = $AssemblyInfoFile
    if (-not [System.IO.Path]::IsPathRooted($resolvedAssemblyInfoFile)) {
        $resolvedAssemblyInfoFile = Join-Path $baseDir $resolvedAssemblyInfoFile
    }

    $resolvedBuildDir = $BuildDir
    if (-not [System.IO.Path]::IsPathRooted($resolvedBuildDir)) {
        $resolvedBuildDir = Join-Path $baseDir $resolvedBuildDir
    }

    $resolvedOutDir = $OutDir
    if (-not [System.IO.Path]::IsPathRooted($resolvedOutDir)) {
        $resolvedOutDir = Join-Path $baseDir $resolvedOutDir
    }

    $resolvedVersionXmlPath = $VersionXmlPath
    if (-not [System.IO.Path]::IsPathRooted($resolvedVersionXmlPath)) {
        $resolvedVersionXmlPath = Join-Path $baseDir $resolvedVersionXmlPath
    }

    if ([string]::IsNullOrWhiteSpace($Version)) {
        $auto = Derive-NextVersionFromLatestRelease -Repo $Repo
        if ($null -eq $auto) {
            throw "无法自动推断下一个版本号（可能是仓库无 releases 或未登录 gh）。请用 -Version 指定，例如：-Version 2.2.9"
        }

        if ([string]::IsNullOrWhiteSpace($TagPrefix)) {
            $TagPrefix = $auto.Prefix
        }

        $Version = $auto.Version
    }

    $tag = "$TagPrefix$Version"
    $version3 = Normalize-Version3 -VersionString $Version

    if (-not $SkipProjectVersionUpdate) {
        Update-AssemblyInfoVersion -AssemblyInfoPath $resolvedAssemblyInfoFile -NewVersion $version3
    }

    if (-not $SkipBuild) {
        Invoke-ProjectBuild -ProjectPath $resolvedProjectFile -Configuration "Release" -Platform "AnyCPU"
    }

    if (-not (Test-Path -LiteralPath $resolvedBuildDir -PathType Container)) {
        throw "找不到构建输出目录：$resolvedBuildDir"
    }

    $sevenZip = $null
    if (-not [string]::IsNullOrWhiteSpace($SevenZipPath)) {
        $sevenZip = Resolve-SevenZipPath -PreferredPath $SevenZipPath
    }
    else {
        try {
            $sevenZip = Resolve-SevenZipPath -PreferredPath ""
        }
        catch {
            $sevenZip = $null
        }
    }

    New-Item -ItemType Directory -Force -Path $resolvedOutDir | Out-Null
    $zipArchivePath = Join-Path $resolvedOutDir $ZipAssetName
    if (Test-Path -LiteralPath $zipArchivePath) {
        Remove-Item -LiteralPath $zipArchivePath -Force
    }

    Write-Host "打包目录: $resolvedBuildDir"
    Write-Host "输出文件: $zipArchivePath"

    Push-Location $resolvedBuildDir
    try {
        $items = Get-ChildItem -Force
        if ($null -eq $items -or $items.Count -eq 0) {
            throw "目标目录为空，无法打包：$resolvedBuildDir"
        }

        if ($null -ne $sevenZip) {
            & $sevenZip a -tzip -mx=9 $zipArchivePath * | Out-Host
            if ($LASTEXITCODE -ne 0) {
                throw "打包 zip 失败（7z exit code: $LASTEXITCODE）"
            }
        }
        else {
            $compress = Get-Command Compress-Archive -ErrorAction SilentlyContinue
            if (-not $compress) {
                throw "未检测到 7z.exe 且当前 PowerShell 不支持 Compress-Archive。请安装 7-Zip，或升级 PowerShell。"
            }

            Compress-Archive -Path * -DestinationPath $zipArchivePath -Force
        }
    }
    finally {
        Pop-Location
    }

    $defaultChangelog = "https://github.com/$Repo/releases/latest"
    $defaultMandatory = "false"
    $existing = Get-Existing-VersionXmlSettings -XmlPath $resolvedVersionXmlPath -DefaultChangelog $defaultChangelog -DefaultMandatory $defaultMandatory

    $finalChangelog = if ([string]::IsNullOrWhiteSpace($ChangelogUrl)) { $existing.Changelog } else { $ChangelogUrl }
    $finalMandatory = if ([string]::IsNullOrWhiteSpace($Mandatory)) { $existing.Mandatory } else { $Mandatory }
    $downloadUrl = "https://github.com/$Repo/releases/latest/download/$ZipAssetName"

    $xmlContent = Build-VersionXml -Version $version3 -Url $downloadUrl -Changelog $finalChangelog -Mandatory $finalMandatory
    [System.IO.File]::WriteAllText($resolvedVersionXmlPath, $xmlContent, [System.Text.Encoding]::UTF8)

    $releaseExists = $false
    $prevErrorActionPreference = $ErrorActionPreference
    try {
        $ErrorActionPreference = "Continue"
        & gh release view $tag --repo $Repo --json tagName 1>$null 2>$null
        $releaseExists = ($LASTEXITCODE -eq 0)
    }
    finally {
        $ErrorActionPreference = $prevErrorActionPreference
    }

    if (-not $releaseExists) {
        Write-Host "创建 Release: $Repo $tag"
        & gh release create $tag $zipArchivePath $resolvedVersionXmlPath --repo $Repo --title $tag --notes "Auto upload $tag" | Out-Host
        if ($LASTEXITCODE -ne 0) {
            throw "创建 Release 失败（gh exit code: $LASTEXITCODE）"
        }
    }
    else {
        Write-Host "更新 Release 资产: $Repo $tag"
        & gh release upload $tag $zipArchivePath $resolvedVersionXmlPath --repo $Repo --clobber | Out-Host
        if ($LASTEXITCODE -ne 0) {
            throw "上传 Release 资产失败（gh exit code: $LASTEXITCODE）"
        }
    }

    $viewJson = & gh release view $tag --repo $Repo --json url 2>$null
    if (-not [string]::IsNullOrWhiteSpace($viewJson)) {
        $url = ($viewJson | ConvertFrom-Json).url
        Write-Host "Release URL: $url"
    }

    Write-Host "完成。"
}
finally {
    Pop-Location
}
