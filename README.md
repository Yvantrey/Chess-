# AR Tabletop Chess

Hot-seat and vs-computer chess with an AR placement path for iPhone. Built with Unity 6, AR Foundation, and ARKit.

Play in the Editor (`ChessPlaytest`) or place a 3D board on a real table (AR). Legal moves, captures, check detection, turn UX, and a computer opponent (minimax).

**GitHub:** https://github.com/cinna03/Chess-

## Features

- **Hot-seat** — two players, one device; board flips to face whose turn it is
- **vs Computer** — you play White; **minimax AI** (depth 3, material + positional eval)
- **Game over** — checkmate / stalemate panel with rematch
- Polished **uGUI** with **Fredoka** + panel pop animations
- Legal-move markers (green) and capture rings (red)
- Piece move hop + capture tray + last-move line in the HUD
- Check detection + celebration burst on checkmate
- AR: tap a detected plane to place the board (`ARChess` / setup menu)

## Out of scope

- Online multiplayer
- Difficulty selector / grandmaster-level engine
- Android public build (iPhone AR path; itch build from ChessPlaytest)

## Contributors

| Name | Role | Contributions |
|------|------|----------------|
| **[YOUR FULL NAME] — Group Leader** | Lead developer | Game logic, UX, AI mode, AR placement, repo, build, video |
| **[TEAMMATE FULL NAME]** | Collaborator | DevLog entries, documentation support, video / process contributions |

*(Replace names before Canvas submit. Tracker: leader = edit, teammate = comment.)*

## Requirements

- Unity 6 (`6000.5.0f1` or compatible) with URP + AR Foundation / ARKit
- macOS + Xcode for iOS builds
- Editor playtest works without a headset/phone

## Project structure

```
Assets/
├── Scripts/Chess/
│   ├── Core/     # Board, legal moves, AI
│   ├── View/     # Board visuals, HUD, input, modes
│   ├── AR/       # Plane placement
│   └── Editor/   # Playtest / AR setup / itch build menus
├── Scenes/
│   ├── ChessPlaytest.unity   # Desktop / Editor demo (use for itch build)
│   ├── ARChess.unity         # AR-enabled scene
│   └── SampleScene.unity     # Mobile AR template
docs/
├── SUBMISSION_CHECKLIST.md
└── submission/               # DevLog, Canvas hub, video script, tracker templates
```

## Setup

1. Clone this repository.
2. Open the folder in Unity Hub (Unity 6).
3. Let Unity import packages.

### Editor playtest (recommended for graders / itch)

4. Open `Assets/Scenes/ChessPlaytest.unity`.
5. Press **Play**.
6. Choose **Hot-seat** or **vs Computer**.
7. Click pieces → glowing squares to move.

### Build for itch.io

- Menu **Chess → Build → Mac Playtest (itch)** → `Builds/Mac/ARTabletopChess.app`
- Optional Windows: **Chess → Build → Windows Playtest (itch)**
- Zip and upload to itch; set project **Public**
- See `docs/submission/HOW_TO_SUBMIT.md`

### AR on iPhone

4. Open `Assets/Scenes/ARChess.unity` (or SampleScene + **Chess → Setup AR Chess In Open Scene**).
5. Build Settings → iOS → Build & Run.
6. Scan a table → tap to place → pick a mode → play.

## Demo media

Add screenshots / GIFs before final submit:

- Mode select screen
- Legal move markers + capture
- Board flip (hot-seat)
- vs Computer “thinking” + reply
- Capture tray / game over
- (Optional) AR board on table

## Links (fill before Canvas)

- **DevLog:** _[public Notion / Google Doc]_
- **Public build (itch.io):** _[url]_
- **Video walkthrough:** _[YouTube / Drive public link]_
- **Task allocation tracker:** _[url]_

Submission templates: `docs/submission/`
