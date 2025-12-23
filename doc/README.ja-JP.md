# NovalAi3 自動バッチ生成ツール

言語: [简体中文](../README.md) | [English](README.en.md) | 日本語

![platform](https://img.shields.io/badge/platform-Windows-0078D6?logo=windows&logoColor=white)
![framework](https://img.shields.io/badge/.NET%20Framework-4.8-512BD4?logo=dotnet&logoColor=white)
![release](https://img.shields.io/github/v/release/CyanAutumn/NovalAi3AutoMatic)

NovelAI 向けの Windows 一括生成・ディレクターツールクライアント。複数モデル、Prompt テンプレート、Vibe/Img2Img 参照画像、自動連続実行に対応。

![UI スクリーンショット](source/image.png)

## 目次
- [機能](#機能)
- [動作環境](#動作環境)
- [ダウンロードとインストール](#ダウンロードとインストール)
- [初期設定](#初期設定)
- [使い方](#使い方)
- [パラメータ説明](#パラメータ説明)
- [出力とファイル名](#出力とファイル名)
- [設定とファイル配置](#設定とファイル配置)
- [自動更新](#自動更新)
- [よくある質問](#よくある質問)
- [開発とビルド](#開発とビルド)
- [関連リンク](#関連リンク)
- [ライセンス](#ライセンス)

## 機能
- 一括生成とリズム制御：実行回数と短休/長休の設定。
- 複数モデル対応：NAI2 / NAI3 / NAI3 Furry / NAI4 Preview / NAI4 Full / NAI4.5 Curated / NAI4.5 Full。
- Prompt テンプレート：固定/ランダム画師、ランダム提示詞、Wildcard 占位符。
- 参照画像：Vibe 複数参照（`.naiv4vibe` 対応）、Img2Img 強度/ノイズ。
- ディレクターツール：背景除去、線画、スケッチ、着色、表情、ゴミ除去。単体/一括に対応。
- 出力と記録：PNG/WEBP、同名 TXT への Prompt 保存、ファイル名形式切替、ログ出力。
- プリセット管理：複数プリセットの保存/切替、前回終了時の自動復元。
- 自動更新（GitHub Releases）。

## 動作環境
- Windows 10/11
- .NET Framework 4.8
- NovelAI アカウント Token（必要に応じて Proxy）

## ダウンロードとインストール
1. Releases からダウンロードして展開：<https://github.com/CyanAutumn/NovalAi3AutoMatic/releases>
2. `AutoNai3Tools.exe` を実行。
3. 初回起動で既定の出力/設定フォルダーが作成されます。

## 初期設定
1. `Token` を設定（NovelAI アカウントから取得）。
2. 必要なら `Proxy` を設定。例：`http://127.0.0.1:7890`。
3. ディレクトリ設定：
   - 出力フォルダー `OutputPath`
   - ランダム提示詞フォルダー `RandomPromptFolderPath`
   - Wildcard フォルダー `WildcardFolderPath`
4. モデル、解像度、Sampler、Steps などを調整。

## 使い方

### 基本的な生成フロー
1. Prompt / Negative Prompt を入力。
2. モデルと生成パラメータ（Steps、Sampler、Scale、CFG など）を選択。
3. 「実行回数」「パラメータ固定回数」を設定。
4. 「生成」をクリック。実行中はいつでも停止可能。

### Prompt テンプレート記法
| 記法 | 説明 | 例 |
| --- | --- | --- |
| `<固定画师>` | 「固定画師」入力欄の内容を使用 | `1girl, <固定画师>` |
| `<随机画师>` | 画師リストからランダム合成 | `1girl, <随机画师>` |
| `<随机提示词>` | ランダム提示詞フォルダーから抽選 | `<随机提示词>` |
| `<随机提示词:顺序>` | ファイル順で巡回 | `<随机提示词:顺序>` |
| `<xxx>` | Wildcard：`wildcard/xxx.txt` から1行 | `<衣服>` |
| `<xxx:顺序>` | Wildcard を順番に | `<衣服:顺序>` |

補足：
- ランダム提示詞フォルダー内の各 `.txt` は 1 つの Prompt 断片（カンマ区切り）。改行はスペース扱い。
- Wildcard は 1 行 1 候補。画面の片段（スニペット）クリックで挿入できます。
- 提示詞ブラックリスト/正規表現ブラックリストは主にランダム提示詞の結果をフィルタします。

### ランダム提示詞フォルダー形式
フォルダー内に複数の `.txt` を配置。各ファイルが 1 つの Prompt 断片です：
```
1girl, solo, masterpiece, best quality
```
`<随机提示词>` はランダム選択、`<随机提示词:顺序>` は順番に巡回します。

### Wildcard フォルダーと管理
`wildcard/` 配下の各 `.txt` が `<xxx>` の占位符になります：
```
wildcard/
  衣服.txt
  发型.txt
```
`衣服.txt` 例（1 行 1 候補）：
```
hoodie
long coat
school uniform
```

### 画師リスト形式と重み
「ランダム画師」は行ごとにグループ化し、行内は `|` で区切ります。各画師に重み指定が可能です：
```
artistA|artistB
artistC,0,2,0,3|artistD
```
重み形式：
- `名前,減重最小,減重最大,加重最小,加重最大`
- `[]` または `{}` で減重/加重をランダム付与。
- `::` で終わる画師（例：`artistE::`）は `x::artistE::` 形式に自動変換。
- 「Artist Modify」を有効にすると `artist:` プレフィックスを自動付与。

### Vibe 参照画像
- 複数参照および `.naiv4vibe` に対応。
- 各画像に `informationExtracted` と `referenceStrength` を設定。
- `.naiv4vibe` は初回にローカルキャッシュを生成し、以後高速化します。

### Img2Img
- 入力画像を選択し、`Strength` と `Noise` を設定。
- 画像未選択時は Img2Img パラメータは送信しません。

### ディレクターツール
- 背景除去、線画、スケッチ、着色、表情、ゴミ除去。
- 単体/フォルダー一括に対応。
- 着色/表情モードは追加 Prompt と Defry を入力可能。
- 出力は `OutputPath` に保存。

### プリセット管理
- プリセットの保存/読み込み/削除。
- 起動時に「前回終了時の自動保存」を読み込み。

### 解像度とパラメータ固定
- 「パラメータ固定回数」はランダム項目の更新頻度を制御：
  - 例：3 の場合、1 回目で更新、2～3 回目は保持、4 回目で再更新。
- ランダム画師/Wildcard/ランダム提示詞/解像度は個別に固定可能。
- 解像度モード：
  - 固定：常に現在の解像度
  - 順序：リスト順に巡回
  - ランダム：リストからランダム選択

## パラメータ説明
| パラメータ | 説明 | 備考 |
| --- | --- | --- |
| Model | モデル選択 | NAI2 / NAI3 / NAI3 Furry / NAI4 Preview / NAI4 Full / NAI4.5 Curated / NAI4.5 Full |
| Steps | 生成ステップ数 | 1-28（超過は自動制限） |
| Sampler | サンプラー | `k_euler` / `k_euler_ancestral` / `k_dpmpp_2s_ancestral` / `k_dpmpp_2m_sde` / `k_dpmpp_2m` / `k_dpmpp_sde` / `ddim_v3` |
| Noise Schedule | ノイズ方式 | `native` / `karras` / `exponential` / `polyexponential` |
| Scale | Prompt Guidance | 0-10、小数1桁 |
| CFG Rescale | CFG Rescale | NovelAI と同様 |
| SMEA / DYN | サンプリング最適化 | SMEA オフで DYN もオフ |
| Decrisp | Decrisp | NovelAI と同様 |
| Variety | 多様性 | オフ / オン / カスタム_リスクパラメータ |
| 解像度 | Width / Height | 解像度リストとモードで制御 |
| 解像度リスト | 複数行入力 | 例：`832x1216` |
| 実行回数 | RunNum | クリックあたりの総生成回数 |
| パラメータ固定回数 | RunKeepParams | ランダム更新頻度 |
| 固定シード | FixedSeeds | オフ時は毎回ランダム Seed |
| Seed | Seeds | 固定シード時に有効 |
| 出力形式 | ImageFormat | PNG / WEBP |
| 出力ファイル名形式 | OutputFileNameFormat | NovalAI / 全画師語 / 日付 |
| Prompt 保存 | SavePromptToTxt | 画師あり/なしで保存可能 |
| Proxy | Proxy | 必要時のみ |
| ブラックリスト | PromptBlackList / Regex | ランダム提示詞のフィルタ用 |

## 出力とファイル名
出力ファイル名は「出力ファイル名形式」で決まります：
- `NovalAI`：`{prompt} s-{seed}`
- `全画師語`：`{artist_summary}_{seed}`
- `日付`：`yyyyMMdd_HHmmss`

「同名 TXT に Prompt を保存」を有効にすると、画像の横に `.txt` が作成されます。

## 設定とファイル配置
| 項目 | 既定 | 備考 |
| --- | --- | --- |
| 出力フォルダー | `.\output` | 設定で変更可能 |
| Wildcard フォルダー | `.\wildcard` | `*.txt` 断片を保存 |
| ランダム提示詞フォルダー | `.\prompt\prompt_by_风吟` | `*.txt` を保存 |
| プリセット | `C:\Users\Public\Documents\auto_nai3_2\*.toml` | プリセット保存 |
| システム設定 | `C:\Users\Public\Documents\auto_nai3_system\config.toml` | Token、休眠設定など |
| ログ | `logs/mylog.txt` | log4net 出力 |

## 自動更新
非デバッグモード起動時に GitHub Releases から更新確認を行います。

## よくある質問
1. Token が無効/リクエスト失敗  
   Token の期限と NovelAI への接続を確認。

2. Wildcard/ランダム提示詞が動作しない  
   フォルダーが存在し `.txt` があること、パスが空でないことを確認。

3. プレビューが出ないが保存はされる  
   WebP のプレビュー復号に失敗している可能性。保存は行われます。

4. 設定が保存できない  
   `C:\Users\Public\Documents\` への書き込み権限を確認。

## 開発とビルド
- 依存：Visual Studio 2022 + .NET Framework 4.8
- `AutoNai3Tools.sln` を開き、NuGet を復元して `Release` ビルド。
- プロジェクト構成：
  - `controllers/` 生成フローとディレクターツール制御
  - `services/` 設定と Wildcard サービス
  - `utils/` リクエスト、ログ、Prompt 解析、Vibe 処理
  - `body/` モデル別のリクエストボディ

## 関連リンク
- 利用ガイド：<https://cyanautumn.github.io/NovalAi3AutoMaticDoc/>
- Prompt 解析：<https://spell.novelai.dev/>
- WD-Tagger：<https://huggingface.co/spaces/SmilingWolf/wd-tagger>

## ライセンス
ライセンスは未指定です。
