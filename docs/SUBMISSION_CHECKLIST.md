# Pre-submission checklist — AR Tabletop Chess

**Do this pack first:** [`docs/submission/HOW_TO_SUBMIT.md`](submission/HOW_TO_SUBMIT.md)

Templates ready to paste:
- DevLog → `docs/submission/DEVLOG.md`
- Canvas hub → `docs/submission/CANVAS_HUB.md`
- Video script → `docs/submission/VIDEO_SCRIPT.md`
- Tracker → `docs/submission/TRACKER.md`

Deadline: submit Canvas **before** the posted cutoff. Use this as your final gate.

---

## A. Game ready (Editor)

- [ ] `ChessPlaytest` opens and compiles with **no red errors**
- [ ] Mode select shows: **Hot-seat** and **vs Computer**
- [ ] Hot-seat: move → board flip → other color plays
- [ ] vs Computer: you (White) move → “thinking” → Black moves alone
- [ ] Captures go to tray; tips and banner update
- [ ] Check highlights the king in red; checkmate/stalemate shows game-over panel
- [ ] **New Game** and **Change Mode** work
- [ ] Build Settings includes **ChessPlaytest** (+ **ARChess** for device builds)
- [ ] Take 4–6 screenshots / one short GIF for README + DevLog

---

## B. GitHub (5 pts)

- [ ] Repo public: https://github.com/cinna03/Chess-
- [ ] `.gitignore` present (Unity)
- [ ] README updated: features, contributors, setup, media, links section filled
- [ ] All latest work **committed + pushed** from GitHub Desktop (you push)
- [ ] Open repo in incognito — code + README visible

---

## C. DevLog (5 pts)

- [ ] Public doc (Notion / Google Doc / Wiki) — **anyone with link can view**
- [ ] Paste / adapt `docs/submission/DEVLOG.md` (7–8 dated entries)
- [ ] Images in most entries
- [ ] Reflections (what broke / what you learned)
- [ ] **Both names** appear as authors on different entries
- [ ] Test link in incognito

---

## D. Public build (3 pts)

- [ ] Unity: **Chess → Build → Mac Playtest (itch)** → `Builds/Mac/ARTabletopChess.app`
- [ ] Zip uploaded to **itch.io**, set **Public**
- [ ] No login / request-access wall
- [ ] Test play in **incognito** / second account
- [ ] Build page mentions: Hot-seat + vs Computer

---

## E. Video walkthrough (3 pts) — acts as presentation + Q&A

Record **5–7 minutes**, public YouTube (unlisted OK) or Drive “anyone with link”.  
Use `docs/submission/VIDEO_SCRIPT.md`.

1. **Intro (30s)** — title, team, group leader, one-line pitch  
2. **Modes (45s)** — Hot-seat vs vs Computer  
3. **Core UX (90s)** — select, green/red, move anim, capture tray, tips  
4. **Hot-seat flip (30s)** — board turns for the other player  
5. **vs Computer (60s)** — your move, AI thinks, AI replies  
6. **Technical Q&A voiceover (90s)** — answer out loud:
   - How are legal moves decided? *(MoveGenerator + check filter)*
   - How does vs Computer work? *(SimpleChessAi: minimax depth 3, alpha-beta, material + positional)*
   - Why no online multiplayer? *(scope / deadline)*
   - How does AR fit? *(plane tap placer; desktop build for public play)*
   - Who did what? *(honest attribution)*  
7. **Links end card (20s)** — GitHub, DevLog, itch, tracker  

- [ ] Video uploaded and link works logged out
- [ ] Audio clear; cursor/highlights visible

---

## F. Group attribution & tracker (5 pts)

- [ ] Group leader named (you)
- [ ] Contribution bullets match reality (you = implementation; teammate = DevLog/docs/video support)
- [ ] Task allocation tracker from `docs/submission/TRACKER.md`: **you = edit**, teammate = **comment only**
- [ ] Tracker tasks match DevLog / Canvas text
- [ ] Illness / limited availability noted honestly if needed

---

## G. Canvas hub document (required)

Paste `docs/submission/CANVAS_HUB.md` (fill URLs):

- [ ] GitHub URL  
- [ ] DevLog URL  
- [ ] Public build URL  
- [ ] Video URL  
- [ ] Group leader + contributions  
- [ ] Tracker link  

- [ ] Submitted on Canvas **before deadline**
- [ ] All four links tested after submit

---

## H. Live presentation (6 pts)

- [ ] Confirmed with facilitator: **no live presentation** / how those points are handled  
- [ ] If waived: note that in Canvas doc  
- [ ] If still required: both members rehearse speaking parts  

---

## I. Final 30-minute sweep

- [ ] Incognito: GitHub, DevLog, itch, video  
- [ ] Teammate can open all links  
- [ ] README names filled (no `[YOUR FULL NAME]` placeholders)  
- [ ] No “request access” on any link  
- [ ] Backup copy of Canvas text in Drive  

---

**Do not start online multiplayer.**
