# Video script (5–7 minutes) — speak this while screen recording

**Record:** QuickTime (File → New Screen Recording) on `ChessPlaytest` in Play mode.  
**Upload:** YouTube **Unlisted** or Google Drive **Anyone with the link**.

---

### 0:00–0:30 — Intro
“Hi, I’m [YOUR NAME], group leader for AR Tabletop Chess. This is a hot-seat and vs-computer chess game with real rules, a polished UI, and an AR path to place the board on a table. Today I’ll show the desktop playtest graders can use, then answer technical questions.”

### 0:30–1:15 — Modes
Open mode select.  
“Two modes: Hot-seat — two players, one device, board flips each turn. vs Computer — I play White, a minimax AI plays Black.”  
Click **Hot-seat**.

### 1:15–2:45 — Core UX
Select a white piece → show green dots / red rings → move.  
Capture something if quick.  
“Tips and the turn banner update. Captures go to the tray. The bottom bar shows the last move.”

### 2:45–3:15 — Board flip
Make a move; wait for flip.  
“In hot-seat the board turns so the next player faces the pieces.”

### 3:15–4:15 — vs Computer
Modes → vs Computer → move as White → wait for THINKING… → Black moves.  
“You can see the thinking badge, then the AI replies automatically.”

### 4:15–5:45 — Technical Q&A (say out loud)
1. **Legal moves?** “`MoveGenerator` builds candidate moves, then we reject any that leave our king in check.”  
2. **vs Computer?** “`SimpleChessAi` — minimax depth 3 with alpha-beta, material and positional scores.”  
3. **Why no online multiplayer?** “Scope and deadline — local polish was the priority.”  
4. **AR?** “`ARChessBoardPlacer` taps a detected plane. Public itch build is desktop `ChessPlaytest` so anyone can play.”  
5. **Who did what?** “I led implementation. [TEAMMATE] supported DevLog/docs/video as available.”

### 5:45–6:15 — End card
Show GitHub README or paste links on screen:  
- GitHub: https://github.com/cinna03/Chess-  
- DevLog / itch / tracker  

“Thanks — links are also on our Canvas hub.”
