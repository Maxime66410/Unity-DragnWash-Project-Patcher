# Drag'n Wash Project Patcher

[English](README.md) · **Français** · [Español](README.es.md) · [日本語](README.ja.md)

> ⚠️ **En cours de développement** : le wrapper n'est pas encore stable, il reste du travail (shaders URP, scripts manquants, …).

Un **wrapper [Unity Project Patcher](https://github.com/nomnomab/unity-project-patcher)** qui génère un projet Unity éditable à partir d'un build de **Drag'n Wash**, pour le modder facilement avec **BepInEx**.

> Construit sur le fork **[Jettcodey/unity-project-patcher](https://github.com/Jettcodey/unity-project-patcher)** (« Unity 6000+ Support »), car le jeu tourne sous **Unity 6.3**, non supporté par l'UPP officiel.

## 🎮 Profil du jeu ciblé

| | |
|---|---|
| Moteur | **Unity 6000.3.14f1** (Unity 6.3) |
| Backend de scripting | **Mono** (x64) : DLL décompilables |
| Render pipeline | **URP** |
| Input | nouveau Input System (+ legacy) |
| Autres | Addressables, TextMeshPro, Localization, VFX Graph, Steamworks.NET, **YarnSpinner** (dialogues) |
| Code du jeu | `Assembly-CSharp.dll` : nom de code interne **WalkNWash** ; namespaces `WalkNWash.*`, `Raliv.*`, `Naelstrof.*`, `Com.Gatordragongames.*` |
| BepInEx | **5.4.23.5** (Mono x64) : voir [TomXV/dragnwash-modframework](https://github.com/TomXV/dragnwash-modframework) |

## ✅ Prérequis

- **Unity 6000.3.14f1** installé via Unity Hub (doit correspondre *exactement* à la version du jeu).
- **Git** dans le PATH.
- **.NET 10 Desktop Runtime** (requis par AssetRipper v2.0).
- Une copie installée de **Drag'n Wash**.

## 🚀 Installation & utilisation

1. Crée un **nouveau projet Unity vide en 6000.3.14f1** (template 3D URP recommandé).
2. Dans **Window ▸ Package Manager ▸ + ▸ Add package from git URL…**, installe dans cet ordre :
   1. Core (fork 6000+) : `https://github.com/Jettcodey/unity-project-patcher.git`
   2. Companion BepInEx : `https://github.com/Jettcodey/unity-project-patcher-bepinex.git`
   3. Ce wrapper : `https://github.com/Maxime66410/Unity-DragnWash-Project-Patcher.git`
3. Ouvre **Tools ▸ Unity Project Patcher ▸ Open Window**.
4. Renseigne le **dossier du jeu** (`.../steamapps/common/Drag'n Wash`). Laisse l'outil **scanner** : il détecte la version exacte et remplit `_exactPackagesFound` / `_dllsToCopy`.
5. Vérifie les deux settings, puis lance le **patch**. Compte ~10 min. **Fais une sauvegarde** avant tout re-patch (re-patcher peut corrompre les assets).

> **Note AssetRipper :** `DragNWash_AssetRipperSettings.asset` fixe déjà `_customBuildUrl` sur **Jettcodey AssetRipper v2.0**. Ne le vide pas et ne le passe pas en v3.0, le patch échouerait sur les AnimationClips / AnimatorControllers d'Unity 6.3.

## 📦 Contenu du package

```
package.json                              # Manifeste UPM (Unity 6000.3)
Editor/
  Constants.cs                            # PackageName
  DragNWashWrapper.cs                     # Classe [UPPatcher] + pipeline (GetSteps)
  com.maxime66410.dragnwash-project-patcher.Editor.asmdef
  Fixers/
    DragNWashFixer.cs                     # Menu Tools > … > DragNWash (correctifs post-rip)
    SimpleJSON.cs.txt                     # Source SimpleJSON bundlée (fix ④)
DragNWash_UnityProjectPatcherSettings.asset   # Version, pipeline (URP), DLL à copier
DragNWash_AssetRipperSettings.asset           # Mappings dossiers, assemblies à décompiler, config AssetRipper
Shaders/                                  # Shaders URP de remplacement (à ajouter au besoin)
```

## 🛠️ Correctifs post-rip (menu Fixer)

Un rip fraîchement généré compile après quelques correctifs standards. Le package fournit un menu qui les automatise :

**`Tools ▸ Unity Project Patcher ▸ DragNWash ▸`**
- **Fix All** : applique tout d'un coup
- **Remove Template Leftovers** : supprime `TutorialInfo/` + `Readme.asset` du template (doublon `Readme`)
- **Install Required Packages** : installe **Addressables** + **Localization** (via UPM)
- **Remove DOTS Generated Files** : supprime les `__JobReflectionRegistrationOutput__*.cs` (Burst/DOTS)
- **Add SimpleJSON** : dépose `SimpleJSON.cs` dans `Assembly-CSharp` (AssetRipper ne décompile pas SimpleJSON)
- **Fix YarnSpinner Dependencies** : copie les `Yarn.*.dll` manquantes depuis le dossier du jeu
- **Remove Duplicate Native Plugins** : supprime le doublon `libonigwrap.dll`

Chaque action est **idempotente** (sans effet si déjà corrigé). Le chemin du jeu (pour Yarn) est lu depuis *UP Patcher User Settings*, sinon un sélecteur de dossier s'ouvre.

## 📝 Licence

Sous licence **GPL-3.0**, voir [LICENSE.md](LICENSE.md).

Copyright © 2026 Maxime66410. Ce programme est un logiciel libre : vous pouvez le redistribuer et/ou le modifier selon les termes de la GNU General Public License v3.
