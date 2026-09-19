# Changelog

## [Unreleased]
### Added
- **Post-rip fixer menu** `Tools > Unity Project Patcher > DragNWash > …` (`Editor/Fixers/DragNWashFixer.cs`):
  Fix All + individual, idempotent fixers for the 6 known post-rip issues (template leftovers,
  Addressables/Localization packages, DOTS generated files, SimpleJSON, YarnSpinner deps, duplicate libonigwrap).
- Bundled `Editor/Fixers/SimpleJSON.cs.txt` (used by the SimpleJSON fixer).
- `LICENSE.md` : project licensed under **GPL-3.0** (consistent with the reference wrappers).
### Changed
- `DragNWash_UnityProjectPatcherSettings.asset`: `_dllsToCopy` now includes the 13 `Yarn.*` dependency
  assemblies and no longer copies `libonigwrap.dll` (provided by com.unity.collab-proxy) : cleaner rips.
- `_customBuildUrl` pinned to Jettcodey AssetRipper **v2.0** (completes the pipeline on Unity 6000.3.14f1;
  v3.0 crashes in AnimatorController processing).

## [0.1.0] - 2026-09-19
### Added
- Initial scaffold of the Drag'n Wash Unity Project Patcher wrapper.
- `[UPPatcher]` entry point (`DragNWashWrapper`) targeting the Jettcodey Unity-6000+ UPP fork.
- Template settings: `DragNWash_UnityProjectPatcherSettings.asset` (URP, Unity 6000.3.14f1) and
  `DragNWash_AssetRipperSettings.asset` (folder mappings + game script assemblies to decompile).
- README with install/usage instructions and game tech profile.

### Notes
- Settings assets are best-effort templates; run UPP's in-editor scan to populate
  `_exactPackagesFound` / `_dllsToCopy` and to confirm the exact game version.
