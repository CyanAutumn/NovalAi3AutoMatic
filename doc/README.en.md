# NovalAi3 Auto Batch Generation Tool

Language: [简体中文](../README.md) | English | [日本語](README.ja-JP.md)

![platform](https://img.shields.io/badge/platform-Windows-0078D6?logo=windows&logoColor=white)
![framework](https://img.shields.io/badge/.NET%20Framework-4.8-512BD4?logo=dotnet&logoColor=white)
![release](https://img.shields.io/github/v/release/CyanAutumn/NovalAi3AutoMatic)

A Windows client for NovelAI batch image generation and director tools. Supports multiple models, prompt templates, Vibe/Img2Img reference images, and automated runs.

![UI screenshot](source/image.png)

## Table of Contents
- [Features](#features)
- [System Requirements](#system-requirements)
- [Download & Install](#download--install)
- [First-time Setup](#first-time-setup)
- [Usage Guide](#usage-guide)
- [Parameter Reference](#parameter-reference)
- [Output & Naming](#output--naming)
- [Configuration & File Locations](#configuration--file-locations)
- [Auto Update](#auto-update)
- [FAQ](#faq)
- [Development & Build](#development--build)
- [Links](#links)
- [License](#license)

## Features
- Batch generation with pacing: run count and short/long sleep intervals.
- Multi-model support: NAI2 / NAI3 / NAI3 Furry / NAI4 Preview / NAI4 Full / NAI4.5 Curated / NAI4.5 Full.
- Prompt templates: fixed/random artist, random prompt, Wildcard placeholders.
- Reference images: Vibe multi-reference (including `.naiv4vibe`), Img2Img strength/noise.
- Director tools: background removal, line art, sketch, colorize, emotion, declutter; single or batch.
- Output & logging: PNG/WEBP, prompt saved to TXT, filename format options, logs on disk.
- Preset management: save/switch multiple presets, auto-restore last session.
- Built-in auto-update (GitHub Releases).

## System Requirements
- Windows 10/11
- .NET Framework 4.8
- NovelAI account Token (optional proxy)

## Download & Install
1. Download and extract from Releases: <https://github.com/CyanAutumn/NovalAi3AutoMatic/releases>
2. Run `AutoNai3Tools.exe`.
3. First run creates default output and config folders.

## First-time Setup
1. Set `Token` (from NovelAI account).
2. If needed, set `Proxy`, e.g. `http://127.0.0.1:7890`.
3. Set directories:
   - Output directory `OutputPath`
   - Random prompt directory `RandomPromptFolderPath`
   - Wildcard directory `WildcardFolderPath`
4. Adjust model, resolution, Sampler, Steps, etc.

## Usage Guide

### Basic Generation Flow
1. Enter Prompt / Negative Prompt.
2. Select model and parameters (Steps, Sampler, Scale, CFG, etc.).
3. Set “Run Count” and “Keep Params Count”.
4. Click “Generate”; you can stop anytime.

### Prompt Template Syntax
| Syntax | Description | Example |
| --- | --- | --- |
| `<固定画师>` | Use content from “Fixed Artist” | `1girl, <固定画师>` |
| `<随机画师>` | Randomly combine from artist list | `1girl, <随机画师>` |
| `<随机提示词>` | Randomly pick from random prompt folder | `<随机提示词>` |
| `<随机提示词:顺序>` | Cycle in file order | `<随机提示词:顺序>` |
| `<xxx>` | Wildcard: read one line from `wildcard/xxx.txt` | `<衣服>` |
| `<xxx:顺序>` | Wildcard in sequence | `<衣服:顺序>` |

Notes:
- Each `.txt` file in the random prompt folder is treated as a prompt fragment (comma-separated). Newlines are treated as spaces.
- Each line in a Wildcard file is a candidate. Clicking a snippet inserts it into the prompt.
- Prompt blacklist and regex blacklist mainly filter random prompt results.

### Random Prompt Folder Format
Put multiple `.txt` files in the folder. Each file is a prompt snippet:
```
1girl, solo, masterpiece, best quality
```
`<随机提示词>` picks a random file. `<随机提示词:顺序>` cycles by file order.

### Wildcard Folder & Management
Each `.txt` file under `wildcard/` maps to a `<xxx>` placeholder:
```
wildcard/
  衣服.txt
  发型.txt
```
Example `衣服.txt` (one option per line):
```
hoodie
long coat
school uniform
```

### Artist List Format & Weights
“Random Artist” is grouped by line; within a line, use `|` to separate artists. Each artist supports weight parameters:
```
artistA|artistB
artistC,0,2,0,3|artistD
```
Weight format:
- `name,downMin,downMax,upMin,upMax`
- The system randomly wraps with `[]` or `{}` for down/up weight.
- If an artist ends with `::` (e.g. `artistE::`), it becomes `x::artistE::`.
- Optionally enable “Artist Modify” to add the `artist:` prefix.

### Vibe Reference Images
- Supports multiple references and `.naiv4vibe` files.
- Each image has `informationExtracted` and `referenceStrength`.
- `.naiv4vibe` generates a local cache on first use for faster reuse.

### Img2Img
- Select input image, set `Strength` and `Noise`.
- If no image is selected, Img2Img params are not sent.

### Director Tools
- Background removal, line art, sketch, colorize, emotion, declutter.
- Single image or folder batch.
- Colorize/emotion modes accept extra prompt and defry parameters.
- Outputs are saved to `OutputPath`.

### Preset Management
- Save/load/delete presets.
- Auto-load “auto-save from last close” on startup.

### Resolution & Parameter Hold
- “Keep Params Count” controls refresh frequency:
  - Example: set to 3 → refresh on run 1, keep for runs 2–3, refresh on run 4.
- You can keep random artist, Wildcard, random prompt, and resolution separately.
- Resolution modes:
  - Fixed: always use current resolution
  - Sequential: cycle through list order
  - Random: random pick from list

## Parameter Reference
| Parameter | Description | Notes |
| --- | --- | --- |
| Model | Model selection | NAI2 / NAI3 / NAI3 Furry / NAI4 Preview / NAI4 Full / NAI4.5 Curated / NAI4.5 Full |
| Steps | Steps count | 1-28 (clamped) |
| Sampler | Sampler | `k_euler` / `k_euler_ancestral` / `k_dpmpp_2s_ancestral` / `k_dpmpp_2m_sde` / `k_dpmpp_2m` / `k_dpmpp_sde` / `ddim_v3` |
| Noise Schedule | Noise strategy | `native` / `karras` / `exponential` / `polyexponential` |
| Scale | Prompt Guidance | 0-10, 1 decimal |
| CFG Rescale | CFG Rescale | Same as NovelAI |
| SMEA / DYN | Sampling optimization | DYN turns off when SMEA is off |
| Decrisp | Decrisp | Same as NovelAI |
| Variety | Diversity | Off / On / Custom risk params |
| Resolution | Width / Height | Controlled by resolution list & mode |
| Resolution List | Multi-line input | e.g. `832x1216` |
| Run Count | RunNum | Total runs per click |
| Keep Params Count | RunKeepParams | Random refresh frequency |
| Fixed Seeds | FixedSeeds | If off, Seed randomizes each run |
| Seed | Seeds | Used when Fixed Seeds is on |
| Output Format | ImageFormat | PNG / WEBP |
| Output Filename Format | OutputFileNameFormat | NovalAI / All Artists / Date |
| Save Prompt | SavePromptToTxt | Save full prompt or without artists |
| Proxy | Proxy | Only if needed |
| Blacklist | PromptBlackList / Regex | Filters random prompts |

## Output & Naming
Filename format depends on “Output Filename Format”:
- `NovalAI`: `{prompt} s-{seed}`
- `All Artists`: `{artist_summary}_{seed}`
- `Date`: `yyyyMMdd_HHmmss`

If “Save prompt to TXT” is enabled, a `.txt` file will be written next to the image.

## Configuration & File Locations
| Item | Default | Notes |
| --- | --- | --- |
| Output folder | `.\output` | Configurable in settings |
| Wildcard folder | `.\wildcard` | Stores `*.txt` snippets |
| Random prompt folder | `.\prompt\prompt_by_风吟` | Stores `*.txt` prompts |
| Presets | `C:\Users\Public\Documents\auto_nai3_2\*.toml` | Preset files |
| System config | `C:\Users\Public\Documents\auto_nai3_system\config.toml` | Token, sleep settings, etc. |
| Logs | `logs/mylog.txt` | log4net output |

## Auto Update
In non-debug mode, the app checks updates on startup via GitHub Releases.

## FAQ
1. Token invalid or request failed  
   Check token expiration and network access to NovelAI.

2. Wildcard/random prompt not working  
   Ensure folder exists with `.txt` files and the path is not empty.

3. No preview but files saved  
   WebP preview decode may fail; files are still saved.

4. Cannot save config  
   Check write permission for `C:\Users\Public\Documents\`.

## Development & Build
- Dependencies: Visual Studio 2022 + .NET Framework 4.8
- Open `AutoNai3Tools.sln`, restore NuGet packages, build `Release`.
- Project structure:
  - `controllers/` generation pipeline and director tools
  - `services/` config and wildcard services
  - `utils/` requests, logs, prompt parsing, Vibe handling
  - `body/` request bodies per model

## Links
- User guide: <https://cyanautumn.github.io/NovalAi3AutoMaticDoc/>
- Prompt parsing: <https://spell.novelai.dev/>
- WD-Tagger: <https://huggingface.co/spaces/SmilingWolf/wd-tagger>

## License
License not specified.
