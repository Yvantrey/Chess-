#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Chess.EditorTools
{
    /// <summary>
    /// Builds ChessPlaytest for public itch.io / demo distribution.
    /// Menu: Chess → Build → Mac Playtest (itch)
    /// CLI: -executeMethod Chess.EditorTools.ChessBuildMenu.BuildMacPlaytest
    /// </summary>
    public static class ChessBuildMenu
    {
        const string PlaytestScene = "Assets/Scenes/ChessPlaytest.unity";
        const string MacOut = "Builds/Mac/ARTabletopChess.app";
        const string WinOut = "Builds/Windows/ARTabletopChess.exe";

        [MenuItem("Chess/Build/Mac Playtest (itch)", false, 100)]
        public static void BuildMacPlaytestMenu() => BuildMacPlaytest();

        [MenuItem("Chess/Build/Windows Playtest (itch)", false, 101)]
        public static void BuildWindowsPlaytestMenu() => BuildWindowsPlaytest();

        public static void BuildMacPlaytest()
        {
            var ok = BuildPlayer(PlaytestScene, MacOut, BuildTarget.StandaloneOSX);
            if (Application.isBatchMode && !ok)
                EditorApplication.Exit(1);
        }

        public static void BuildWindowsPlaytest()
        {
            var ok = BuildPlayer(PlaytestScene, WinOut, BuildTarget.StandaloneWindows64);
            if (Application.isBatchMode && !ok)
                EditorApplication.Exit(1);
        }

        static bool BuildPlayer(string scene, string locationPath, BuildTarget target)
        {
            if (!File.Exists(scene))
            {
                Debug.LogError($"[Chess Build] Missing scene: {scene}");
                return false;
            }

            var dir = Path.GetDirectoryName(locationPath);
            if (!string.IsNullOrEmpty(dir))
                Directory.CreateDirectory(dir);

            // Ensure only the playtest scene is in this player (desktop demo)
            EditorBuildSettings.scenes = new[]
            {
                new EditorBuildSettingsScene(PlaytestScene, true),
                new EditorBuildSettingsScene("Assets/Scenes/ARChess.unity", false),
                new EditorBuildSettingsScene("Assets/Scenes/SampleScene.unity", false)
            };

            var options = new BuildPlayerOptions
            {
                scenes = new[] { scene },
                locationPathName = locationPath,
                target = target,
                options = BuildOptions.None
            };

            Debug.Log($"[Chess Build] Building {target} → {locationPath}");
            var report = BuildPipeline.BuildPlayer(options);
            var summary = report.summary;

            if (summary.result == BuildResult.Succeeded)
            {
                Debug.Log($"[Chess Build] SUCCESS ({summary.totalSize / (1024 * 1024)} MB) → {locationPath}");
                return true;
            }

            Debug.LogError($"[Chess Build] FAILED: {summary.result}");
            return false;
        }
    }
}
#endif
