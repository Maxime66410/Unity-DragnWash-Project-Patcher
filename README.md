# Drag'n Wash Project Patcher

**English** · [Français](README.fr.md) · [Español](README.es.md) · [日本語](README.ja.md)

> ⚠️ **Work in progress** : this wrapper isn't stable yet, some work remains (URP shaders, missing scripts, …).

A **[Unity Project Patcher](https://github.com/nomnomab/unity-project-patcher) wrapper** that generates an editable Unity project from a **Drag'n Wash** build, so you can mod it easily with **BepInEx**.

> Built on the **[Jettcodey/unity-project-patcher](https://github.com/Jettcodey/unity-project-patcher)** fork ("Unity 6000+ Support"), because the game runs on **Unity 6.3**, which the official UPP doesn't support.

## 🎮 Target game profile

| | |
|---|---|
| Engine | **Unity 6000.3.14f1** (Unity 6.3) |
| Scripting backend | **Mono** (x64) : decompilable DLLs |
| Render pipeline | **URP** |
| Input | new Input System (+ legacy) |
| Other | Addressables, TextMeshPro, Localization, VFX Graph, Steamworks.NET, **YarnSpinner** (dialogue) |
| Game code | `Assembly-CSharp.dll` : internal codename **WalkNWash**; namespaces `WalkNWash.*`, `Raliv.*`, `Naelstrof.*`, `Com.Gatordragongames.*` |
| BepInEx | **5.4.23.5** (Mono x64) : see [TomXV/dragnwash-modframework](https://github.com/TomXV/dragnwash-modframework) |

## ✅ Requirements

- **Unity 6000.3.14f1** installed via Unity Hub (must match the game's version *exactly*).
- **Git** on your PATH.
- **.NET 10 Desktop Runtime** (required by AssetRipper v2.0).
- An installed copy of **Drag'n Wash**.

## 🚀 Installation & usage

1. Create a **new empty Unity project on 6000.3.14f1** (3D URP template recommended).
2. In **Window ▸ Package Manager ▸ + ▸ Add package from git URL…**, install in this order:
   1. Core (6000+ fork): `https://github.com/Jettcodey/unity-project-patcher.git`
   2. BepInEx companion: `https://github.com/Jettcodey/unity-project-patcher-bepinex.git`
   3. This wrapper: `https://github.com/Maxime66410/Unity-DragnWash-Project-Patcher.git`
3. Open **Tools ▸ Unity Project Patcher ▸ Open Window**.
4. Set the **game folder** (`.../steamapps/common/Drag'n Wash`). Let the tool **scan**: it detects the exact version and fills `_exactPackagesFound` / `_dllsToCopy`.
5. Check both settings, then run the **patch**. Expect ~10 min. **Make a backup** before any re-patch (re-patching can corrupt assets).

> **AssetRipper note:** `DragNWash_AssetRipperSettings.asset` already pins `_customBuildUrl` to **Jettcodey AssetRipper v2.0**. Don't clear it or switch it to v3.0, the patch would fail on Unity 6.3's AnimationClips / AnimatorControllers.

## 📦 Package contents

```
package.json                              # UPM manifest (Unity 6000.3)
Editor/
  Constants.cs                            # PackageName
  DragNWashWrapper.cs                     # [UPPatcher] class + pipeline (GetSteps)
  com.maxime66410.dragnwash-project-patcher.Editor.asmdef
  Fixers/
    DragNWashFixer.cs                     # Menu Tools > … > DragNWash (post-rip fixes)
    SimpleJSON.cs.txt                     # Bundled SimpleJSON source (fix ④)
DragNWash_UnityProjectPatcherSettings.asset   # Version, pipeline (URP), DLLs to copy
DragNWash_AssetRipperSettings.asset           # Folder mappings, assemblies to decompile, AssetRipper config
Shaders/                                  # URP replacement shaders (add as needed)
```

## 🛠️ Post-rip fixes (Fixer menu)

A freshly generated rip compiles after a few standard fixes. The package ships a menu that automates them:

**`Tools ▸ Unity Project Patcher ▸ DragNWash ▸`**
- **Fix All** : apply everything at once
- **Remove Template Leftovers** : deletes the template's `TutorialInfo/` + `Readme.asset` (duplicate `Readme`)
- **Install Required Packages** : installs **Addressables** + **Localization** (via UPM)
- **Remove DOTS Generated Files** : deletes the `__JobReflectionRegistrationOutput__*.cs` files (Burst/DOTS)
- **Add SimpleJSON** : drops `SimpleJSON.cs` into `Assembly-CSharp` (AssetRipper doesn't decompile SimpleJSON)
- **Fix YarnSpinner Dependencies** : copies the missing `Yarn.*.dll` files from the game folder
- **Remove Duplicate Native Plugins** : deletes the duplicate `libonigwrap.dll`

Every action is **idempotent** (no effect if already fixed). The game path (for Yarn) is read from *UP Patcher User Settings*, otherwise a folder picker opens.

## 📝 License

Licensed under **GPL-3.0**, see [LICENSE.md](LICENSE.md).

Copyright © 2026 Maxime66410. This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License v3.
