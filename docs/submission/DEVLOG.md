# DevLog — AR Tabletop Chess

**Project:** AR Tabletop Chess  
**Repo:** https://github.com/cinna03/Chess-  
**Authors:** [YOUR FULL NAME] (Group Leader), [TEAMMATE FULL NAME]  
**Share setting:** Anyone with the link can view

> Paste this into a **Google Doc** or **Notion** page. Set sharing to public/view.  
> Add screenshots under each entry (mode select, moves, flip, AI, capture tray).

---

## Entry 1 — Ideation (Week of project start)
**Author:** [YOUR NAME]

We chose **AR Tabletop Chess** instead of inventing a brand-new mechanic late. The pitch is familiar (chess) with a clear XR twist (place a 3D board on a real table) plus a reliable desktop playtest path for graders who do not have an iPhone build.

**Scope we locked:**
- Hot-seat on one device
- vs Computer (local AI)
- Legal chess rules (including check / mate / stalemate)
- AR plane placement as a bonus path
- **No** online multiplayer (deadline risk)

**Reflection:** Keeping chess meant we could spend time on UX polish and AI instead of inventing rules from scratch.

---

## Entry 2 — Core rules engine
**Author:** [YOUR NAME]

Built `ChessBoard`, `MoveGenerator`, and `ChessGame` in C#. Legal moves are generated per piece, then filtered so a move cannot leave your king in check. Castling, en passant, and auto-queen promotion are supported.

**What broke:** Early versions allowed illegal escapes from check because we filtered after applying moves incorrectly. Fixing that with a clone-board check made the rules trustworthy.

**Learned:** Separation of Core (no Unity) vs View made testing moves much easier.

---

## Entry 3 — Board view & interaction
**Author:** [YOUR NAME]

Added 3D board squares, piece meshes (primitives), tap-to-select, green move dots, and red capture rings. Captured pieces animate into a side tray. The board can rotate so the side to move faces the camera — important for hot-seat.

**What broke:** Selection highlights fighting last-move highlights; we layered priority (selection > check > last move > legal).

**Screenshot:** legal moves + capture rings.

---

## Entry 4 — Modes: Hot-seat & vs Computer
**Author:** [YOUR NAME]

Mode select panel opens on Play. Hot-seat flips the board each turn. vs Computer locks the human as White; Black is driven by `SimpleChessAi` (minimax depth 3, alpha-beta, material + positional eval) with a short “thinking” pause so the turn feels readable.

**What broke:** Board rotation was briefly disabled in AI mode; we re-enabled it so demos look consistent.

**Screenshot:** mode select + THINKING badge.

---

## Entry 5 — UI polish for presentation
**Author:** [YOUR NAME]

Replaced bare OnGUI with a runtime **uGUI** (`ChessMenuUi`): Fredoka font, mode card, top turn banner, bottom control bar (New Game / Modes / last move), game-over panel, checkmate celebration. Landscape reference resolution for desktop/itch demos. UI clicks no longer accidentally move pieces.

**Learned:** Demo readability matters as much as rules — graders need to understand state in one glance.

---

## Entry 6 — AR placement path
**Author:** [YOUR NAME]

`ARChessBoardPlacer` raycasts AR planes; tap places the board. Input is gated until the board is placed. Fancy UI shows a place panel, then mode select. Desktop public build uses `ChessPlaytest` so anyone can play without AR hardware.

**Reflection:** AR is a strong demo beat on device; the public build must stay desktop-first for accessibility.

**Screenshot (optional):** board on a table / Editor XR simulation.

---

## Entry 7 — Ship / submission
**Author:** [TEAMMATE NAME] *(or co-author with leader)*

Prepared DevLog, tracker comments, and supported video/docs. Confirmed honest attribution: group leader implemented the game; teammate contributed process/docs as available.

**Links collected for Canvas:**
- GitHub
- This DevLog
- itch.io public build
- Walkthrough video
- Task allocation tracker

**Final reflection:** Shipping a polished local chess experience with clear modes beat chasing multiplayer.

---

## Entry 8 — What we would do next (optional)
**Author:** [YOUR NAME]

Promotion choice UI, stronger AI difficulty slider, nicer 3D chess set, and a clearer AR coaching overlay. Still no online multiplayer until core local play is rock solid.
