using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace DragNWash.ProjectPatcher.Editor {
    public static class DragNWashFixer {
        const string MenuRoot = "Tools/Unity Project Patcher/DragNWash/";

        const string GameFolder      = "Assets/DragNWash";
        const string ScriptsFolder   = GameFolder + "/Game/Scripts";
        const string AsmCSharpFolder = ScriptsFolder + "/Assembly-CSharp";
        const string PluginsFolder   = GameFolder + "/Plugins";
        const string GameDataFolder = "DragNWash_Data";
        const string PackagePath     = "Packages/" + Constants.PackageName;
        const string SimpleJsonAsset = PackagePath + "/Editor/Fixers/SimpleJSON.cs.txt";

        static readonly string[] RequiredPackages = {
            "com.unity.addressables",
            "com.unity.localization",
        };

        [MenuItem(MenuRoot + "Fix All", priority = 0)]
        public static void FixAll() {
            int changed = 0;
            changed += RemoveTemplateLeftovers(false);
            changed += RemoveDotsGeneratedFiles(false);
            changed += AddSimpleJson(false);
            changed += RemoveDuplicateNativePlugins(false);
            changed += FixYarnDependencies(false);
            AssetDatabase.Refresh();
            InstallRequiredPackages(false);
            Debug.Log($"[DragNWash Fixer] Fix All: {changed} file change(s) applied. " +
                      "Package resolution (Addressables/Localization) runs asynchronously, watch the console.");
        }

        [MenuItem(MenuRoot + "Remove Template Leftovers", priority = 20)]
        static void RemoveTemplateLeftoversMenu() { RemoveTemplateLeftovers(true); AssetDatabase.Refresh(); }

        static int RemoveTemplateLeftovers(bool log) {
            int n = 0;
            n += DeleteIfExists("Assets/TutorialInfo", log);
            n += DeleteIfExists("Assets/Readme.asset", log);
            if (log && n == 0) Debug.Log("[DragNWash Fixer] (1) No template leftovers found, already clean.");
            return n;
        }

        [MenuItem(MenuRoot + "Install Required Packages", priority = 21)]
        static void InstallRequiredPackagesMenu() { InstallRequiredPackages(true); }

        static AddAndRemoveRequest _addRequest;

        static void InstallRequiredPackages(bool log) {
            if (_addRequest != null && !_addRequest.IsCompleted) {
                if (log) Debug.Log("[DragNWash Fixer] (2) A package request is already running.");
                return;
            }
            _addRequest = Client.AddAndRemove(packagesToAdd: RequiredPackages);
            EditorApplication.update += PollAddRequest;
            if (log) Debug.Log("[DragNWash Fixer] (2) Requesting Addressables + Localization via UPM (async)…");
        }

        static void PollAddRequest() {
            if (_addRequest == null || !_addRequest.IsCompleted) return;
            EditorApplication.update -= PollAddRequest;
            if (_addRequest.Status == StatusCode.Success)
                Debug.Log("[DragNWash Fixer] (2) Packages resolved.");
            else
                Debug.LogError("[DragNWash Fixer] (2) Package install failed: " + _addRequest.Error?.message);
            _addRequest = null;
        }

        [MenuItem(MenuRoot + "Remove DOTS Generated Files", priority = 22)]
        static void RemoveDotsMenu() { RemoveDotsGeneratedFiles(true); AssetDatabase.Refresh(); }

        static int RemoveDotsGeneratedFiles(bool log) {
            if (!Directory.Exists(ScriptsFolder)) {
                if (log) Debug.LogWarning("[DragNWash Fixer] (3) Scripts folder not found: " + ScriptsFolder);
                return 0;
            }
            var files = Directory.GetFiles(ScriptsFolder, "__JobReflectionRegistrationOutput__*.cs",
                                           SearchOption.AllDirectories);
            int n = files.Sum(f => DeleteIfExists(f.Replace('\\', '/'), log));
            if (log && n == 0) Debug.Log("[DragNWash Fixer] (3) No DOTS generated files found, already clean.");
            return n;
        }

        [MenuItem(MenuRoot + "Add SimpleJSON", priority = 23)]
        static void AddSimpleJsonMenu() { AddSimpleJson(true); AssetDatabase.Refresh(); }

        static int AddSimpleJson(bool log) {
            var target = AsmCSharpFolder + "/SimpleJSON.cs";
            if (File.Exists(target)) {
                if (log) Debug.Log("[DragNWash Fixer] (4) SimpleJSON.cs already present.");
                return 0;
            }
            if (!Directory.Exists(AsmCSharpFolder)) {
                if (log) Debug.LogWarning("[DragNWash Fixer] (4) Target folder not found: " + AsmCSharpFolder);
                return 0;
            }
            var txt = AssetDatabase.LoadAssetAtPath<TextAsset>(SimpleJsonAsset);
            if (txt == null) {
                Debug.LogError("[DragNWash Fixer] (4) Bundled SimpleJSON.cs.txt not found at " + SimpleJsonAsset);
                return 0;
            }
            File.WriteAllText(target, txt.text);
            if (log) Debug.Log("[DragNWash Fixer] (4) Added SimpleJSON.cs into " + AsmCSharpFolder);
            return 1;
        }

        [MenuItem(MenuRoot + "Fix YarnSpinner Dependencies", priority = 24)]
        static void FixYarnMenu() { FixYarnDependencies(true); AssetDatabase.Refresh(); }

        static int FixYarnDependencies(bool log) {
            if (!Directory.Exists(PluginsFolder)) {
                if (log) Debug.LogWarning("[DragNWash Fixer] (5) Plugins folder not found: " + PluginsFolder);
                return 0;
            }
            var managed = GetGameManagedPath();
            if (managed == null) {
                Debug.LogError("[DragNWash Fixer] (5) Could not locate the game's Managed folder. " +
                               "Set the game path in UP Patcher User Settings, or pick it when prompted.");
                return 0;
            }
            var deps = Directory.GetFiles(managed, "Yarn.*.dll")
                                .Where(f => Path.GetFileName(f).StartsWith("Yarn.")
                                            && Path.GetFileName(f).EndsWith(".dll"));
            int n = 0;
            foreach (var src in deps) {
                var dest = PluginsFolder + "/" + Path.GetFileName(src);
                if (File.Exists(dest)) continue;
                File.Copy(src, dest);
                n++;
            }
            if (log) Debug.Log(n > 0
                ? $"[DragNWash Fixer] (5) Copied {n} Yarn.* dependency DLL(s)."
                : "[DragNWash Fixer] (5) Yarn dependencies already present.");
            return n;
        }

        [MenuItem(MenuRoot + "Remove Duplicate Native Plugins", priority = 25)]
        static void RemoveDupNativeMenu() { RemoveDuplicateNativePlugins(true); AssetDatabase.Refresh(); }

        static int RemoveDuplicateNativePlugins(bool log) {
            int n = DeleteIfExists(PluginsFolder + "/libonigwrap.dll", log);
            if (log && n == 0) Debug.Log("[DragNWash Fixer] (6) No duplicate libonigwrap.dll found.");
            return n;
        }

        static int DeleteIfExists(string assetPath, bool log) {
            if (File.Exists(assetPath) || Directory.Exists(assetPath)) {
                if (AssetDatabase.DeleteAsset(assetPath)) {
                    if (log) Debug.Log("[DragNWash Fixer] Deleted " + assetPath);
                    return 1;
                }
                Debug.LogWarning("[DragNWash Fixer] Failed to delete " + assetPath);
            }
            return 0;
        }

        static string GetGameManagedPath() {
            foreach (var guid in AssetDatabase.FindAssets("UPPatcherUserSettings")) {
                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                if (!assetPath.EndsWith(".asset")) continue;
                foreach (var raw in File.ReadAllLines(assetPath)) {
                    var line = raw.Trim();
                    const string key = "_gameFolderPath:";
                    if (!line.StartsWith(key)) continue;
                    var value = line.Substring(key.Length).Trim().Trim('"', '\'');
                    if (string.IsNullOrEmpty(value)) continue;
                    var managed = Path.Combine(value, GameDataFolder, "Managed");
                    if (Directory.Exists(managed)) return managed;
                }
            }
            var picked = EditorUtility.OpenFolderPanel("Select the Drag'n Wash game folder", "", "");
            if (!string.IsNullOrEmpty(picked)) {
                var managed = Path.Combine(picked, GameDataFolder, "Managed");
                if (Directory.Exists(managed)) return managed;
            }
            return null;
        }
    }
}
