#!/usr/bin/env bash
# Build Mac playtest for itch. Close the Unity Editor on this project first.
set -euo pipefail
ROOT="$(cd "$(dirname "$0")/.." && pwd)"
UNITY="${UNITY_PATH:-/Applications/Unity/Hub/Editor/6000.5.0f1/Unity.app/Contents/MacOS/Unity}"
mkdir -p "$ROOT/Builds"
"$UNITY" -quit -batchmode -nographics \
  -projectPath "$ROOT" \
  -buildTarget StandaloneOSX \
  -executeMethod Chess.EditorTools.ChessBuildMenu.BuildMacPlaytest \
  -logFile "$ROOT/Builds/mac-build.log"
echo "Build finished. Check Builds/Mac/ARTabletopChess.app and Builds/mac-build.log"
if [[ -d "$ROOT/Builds/Mac/ARTabletopChess.app" ]]; then
  ditto -c -k --sequesterRsrc --keepParent \
    "$ROOT/Builds/Mac/ARTabletopChess.app" \
    "$ROOT/Builds/Mac/ARTabletopChess-mac.zip"
  echo "Zipped → Builds/Mac/ARTabletopChess-mac.zip"
fi
