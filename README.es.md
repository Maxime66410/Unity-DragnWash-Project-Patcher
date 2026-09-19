# Drag'n Wash Project Patcher

[English](README.md) · [Français](README.fr.md) · **Español** · [日本語](README.ja.md)

> ⚠️ **En desarrollo** : el wrapper aún no es estable, queda trabajo por hacer (shaders URP, scripts faltantes, …).

Un **wrapper de [Unity Project Patcher](https://github.com/nomnomab/unity-project-patcher)** que genera un proyecto Unity editable a partir de una build de **Drag'n Wash**, para moddearlo fácilmente con **BepInEx**.

> Basado en el fork **[Jettcodey/unity-project-patcher](https://github.com/Jettcodey/unity-project-patcher)** ("Unity 6000+ Support"), porque el juego usa **Unity 6.3**, que el UPP oficial no soporta.

## 🎮 Perfil del juego objetivo

| Tipo | Descripción |
|---|---|
| Motor | **Unity 6000.3.14f1** (Unity 6.3) |
| Backend de scripting | **Mono** (x64) : DLLs decompilables |
| Render pipeline | **URP** |
| Input | nuevo Input System (+ legacy) |
| Otros | Addressables, TextMeshPro, Localization, VFX Graph, Steamworks.NET, **YarnSpinner** (diálogos) |
| Código del juego | `Assembly-CSharp.dll` : nombre en clave interno **WalkNWash**; namespaces `WalkNWash.*`, `Raliv.*`, `Naelstrof.*`, `Com.Gatordragongames.*` |
| BepInEx | **5.4.23.5** (Mono x64) : ver [TomXV/dragnwash-modframework](https://github.com/TomXV/dragnwash-modframework) |

## ✅ Requisitos

- **Unity 6000.3.14f1** instalado con Unity Hub (debe coincidir *exactamente* con la versión del juego).
- **Git** en el PATH.
- **.NET 10 Desktop Runtime** (requerido por AssetRipper v2.0).
- Una copia instalada de **Drag'n Wash**.

## 🚀 Instalación y uso

1. Crea un **proyecto Unity vacío nuevo en 6000.3.14f1** (plantilla 3D URP recomendada).
2. En **Window ▸ Package Manager ▸ + ▸ Add package from git URL…**, instala en este orden:
   1. Core (fork 6000+): `https://github.com/Jettcodey/unity-project-patcher.git`
   2. Companion de BepInEx: `https://github.com/Jettcodey/unity-project-patcher-bepinex.git`
   3. Este wrapper: `https://github.com/Maxime66410/Unity-DragnWash-Project-Patcher.git`
3. Abre **Tools ▸ Unity Project Patcher ▸ Open Window**.
4. Indica la **carpeta del juego** (`.../steamapps/common/Drag'n Wash`). Deja que la herramienta **escanee**: detecta la versión exacta y rellena `_exactPackagesFound` / `_dllsToCopy`.
5. Revisa ambos ajustes, luego ejecuta el **patch**. Cuenta ~10 min. **Haz una copia de seguridad** antes de cualquier re-patch (re-patchear puede corromper los assets).

> **Nota AssetRipper:** `DragNWash_AssetRipperSettings.asset` ya fija `_customBuildUrl` en **Jettcodey AssetRipper v2.0**. No lo borres ni lo cambies a v3.0, el patch fallaría en los AnimationClips / AnimatorControllers de Unity 6.3.

## 📦 Contenido del paquete

```
package.json                              # Manifiesto UPM (Unity 6000.3)
Editor/
  Constants.cs                            # PackageName
  DragNWashWrapper.cs                     # Clase [UPPatcher] + pipeline (GetSteps)
  com.maxime66410.dragnwash-project-patcher.Editor.asmdef
  Fixers/
    DragNWashFixer.cs                     # Menú Tools > … > DragNWash (correcciones post-rip)
    SimpleJSON.cs.txt                     # Fuente SimpleJSON incluida (fix ④)
DragNWash_UnityProjectPatcherSettings.asset   # Versión, pipeline (URP), DLLs a copiar
DragNWash_AssetRipperSettings.asset           # Mapeos de carpetas, assemblies a decompilar, config AssetRipper
Shaders/                                  # Shaders URP de reemplazo (añadir si hace falta)
```

## 🛠️ Correcciones post-rip (menú Fixer)

Un rip recién generado compila tras unas correcciones estándar. El paquete incluye un menú que las automatiza:

**`Tools ▸ Unity Project Patcher ▸ DragNWash ▸`**
- **Fix All**: aplica todo de una vez
- **Remove Template Leftovers**: elimina `TutorialInfo/` + `Readme.asset` de la plantilla (`Readme` duplicado)
- **Install Required Packages**: instala **Addressables** + **Localization** (vía UPM)
- **Remove DOTS Generated Files**: elimina los `__JobReflectionRegistrationOutput__*.cs` (Burst/DOTS)
- **Add SimpleJSON**: coloca `SimpleJSON.cs` en `Assembly-CSharp` (AssetRipper no decompila SimpleJSON)
- **Fix YarnSpinner Dependencies**: copia los `Yarn.*.dll` que faltan desde la carpeta del juego
- **Remove Duplicate Native Plugins**: elimina el `libonigwrap.dll` duplicado

Cada acción es **idempotente** (sin efecto si ya está corregido). La ruta del juego (para Yarn) se lee de *UP Patcher User Settings*, si no, se abre un selector de carpeta.

## 📝 Licencia

Bajo licencia **GPL-3.0**, ver [LICENSE.md](LICENSE.md).

Copyright © 2026 Maxime66410. Este programa es software libre: puedes redistribuirlo y/o modificarlo bajo los términos de la GNU General Public License v3.
