@echo off
setlocal EnableExtensions

REM Ensure UTF-8 code page to avoid garbled Chinese output in some consoles.
chcp 65001 >nul

set "SCRIPT_DIR=%~dp0"
cd /d "%SCRIPT_DIR%"

powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%SCRIPT_DIR%PublishUpdateRelease.ps1" %*

echo.
pause
