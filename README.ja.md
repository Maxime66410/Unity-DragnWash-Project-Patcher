# Drag'n Wash Project Patcher

[English](README.md) · [Français](README.fr.md) · [Español](README.es.md) · **日本語**

> ⚠️ **開発中** : このラッパーはまだ安定版ではありません。残作業があります（URP シェーダー、欠落スクリプトなど）。

**Drag'n Wash** のビルドから編集可能な Unity プロジェクトを生成する **[Unity Project Patcher](https://github.com/nomnomab/unity-project-patcher) ラッパー**です。**BepInEx** で手軽に MOD を作れます。

> 公式 UPP が非対応の **Unity 6.3** で動作するため、**[Jettcodey/unity-project-patcher](https://github.com/Jettcodey/unity-project-patcher)** フォーク（「Unity 6000+ Support」）をベースにしています。

## 🎮 対象ゲームのプロフィール

| | |
|---|---|
| エンジン | **Unity 6000.3.14f1** (Unity 6.3) |
| スクリプティングバックエンド | **Mono** (x64)：逆コンパイル可能な DLL |
| レンダーパイプライン | **URP** |
| 入力 | 新 Input System（+ レガシー） |
| その他 | Addressables, TextMeshPro, Localization, VFX Graph, Steamworks.NET, **YarnSpinner**（ダイアログ） |
| ゲームコード | `Assembly-CSharp.dll`：内部コードネーム **WalkNWash**；名前空間 `WalkNWash.*`, `Raliv.*`, `Naelstrof.*`, `Com.Gatordragongames.*` |
| BepInEx | **5.4.23.5** (Mono x64)：[TomXV/dragnwash-modframework](https://github.com/TomXV/dragnwash-modframework) を参照 |

## ✅ 必要環境

- **Unity 6000.3.14f1**（Unity Hub 経由。ゲームのバージョンと*完全に*一致すること）。
- PATH に **Git**。
- **.NET 10 Desktop Runtime**（AssetRipper v2.0 が必要）。
- **Drag'n Wash** のインストール済みコピー。

## 🚀 インストールと使い方

1. **6000.3.14f1 で空の Unity プロジェクトを新規作成**（3D URP テンプレート推奨）。
2. **Window ▸ Package Manager ▸ + ▸ Add package from git URL…** から、この順に追加：
   1. Core（6000+ フォーク）：`https://github.com/Jettcodey/unity-project-patcher.git`
   2. BepInEx コンパニオン：`https://github.com/Jettcodey/unity-project-patcher-bepinex.git`
   3. 本ラッパー：`https://github.com/Maxime66410/Unity-DragnWash-Project-Patcher.git`
3. **Tools ▸ Unity Project Patcher ▸ Open Window** を開く。
4. **ゲームフォルダー**（`.../steamapps/common/Drag'n Wash`）を指定。**スキャン**させると正確なバージョンを検出し、`_exactPackagesFound` / `_dllsToCopy` を埋めます。
5. 両方の設定を確認してから **patch** を実行。約 10 分。再 patch の前に**バックアップ**を取ってください（再 patch はアセットを破損させる場合があります）。

> **AssetRipper に関する注意：** `DragNWash_AssetRipperSettings.asset` は `_customBuildUrl` を **Jettcodey AssetRipper v2.0** に固定済みです。空にしたり v3.0 に変更しないでください。Unity 6.3 の AnimationClips / AnimatorControllers で patch が失敗します。

## 📦 パッケージ構成

```
package.json                              # UPM マニフェスト (Unity 6000.3)
Editor/
  Constants.cs                            # PackageName
  DragNWashWrapper.cs                     # [UPPatcher] クラス + パイプライン (GetSteps)
  com.maxime66410.dragnwash-project-patcher.Editor.asmdef
  Fixers/
    DragNWashFixer.cs                     # メニュー Tools > … > DragNWash（post-rip 修正）
    SimpleJSON.cs.txt                     # 同梱の SimpleJSON ソース (fix ④)
DragNWash_UnityProjectPatcherSettings.asset   # バージョン、パイプライン (URP)、コピーする DLL
DragNWash_AssetRipperSettings.asset           # フォルダーマッピング、逆コンパイル対象、AssetRipper 設定
Shaders/                                  # URP 置き換えシェーダー（必要に応じて追加）
```

## 🛠️ post-rip 修正（Fixer メニュー）

生成直後の rip は、いくつかの標準的な修正でコンパイルできます。パッケージはそれを自動化するメニューを提供します：

**`Tools ▸ Unity Project Patcher ▸ DragNWash ▸`**
- **Fix All**：すべてを一括適用
- **Remove Template Leftovers**：テンプレートの `TutorialInfo/` + `Readme.asset` を削除（`Readme` の重複）
- **Install Required Packages**：**Addressables** + **Localization** を導入（UPM 経由）
- **Remove DOTS Generated Files**：`__JobReflectionRegistrationOutput__*.cs` を削除（Burst/DOTS）
- **Add SimpleJSON**：`SimpleJSON.cs` を `Assembly-CSharp` に配置（AssetRipper は SimpleJSON を逆コンパイルしない）
- **Fix YarnSpinner Dependencies**：不足している `Yarn.*.dll` をゲームフォルダーからコピー
- **Remove Duplicate Native Plugins**：重複した `libonigwrap.dll` を削除

各アクションは**冪等**です（すでに修正済みなら何もしません）。ゲームパス（Yarn 用）は *UP Patcher User Settings* から読み取り、なければフォルダー選択ダイアログが開きます。

## 📝 ライセンス

**GPL-3.0** ライセンス。[LICENSE.md](LICENSE.md) を参照してください。

Copyright © 2026 Maxime66410. 本プログラムはフリーソフトウェアです。GNU General Public License v3 の条件の下で再配布・改変できます。
