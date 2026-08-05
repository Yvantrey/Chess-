# Submission package — do these in order

Everything under `docs/submission/` is ready to paste. The Mac build runs via Unity menu or CLI.

## 1. Push code (5 min) — GitHub Desktop
Commit any uncommitted UI/build-script changes, push `main`.  
Incognito-check: https://github.com/cinna03/Chess-

## 2. Mac / Windows build for itch (15–25 min)

### In Unity (easiest)
1. Open this project in Unity **6000.5.0f1**
2. Open `Assets/Scenes/ChessPlaytest.unity` once (optional smoke test)
3. Menu **Chess → Build → Mac Playtest (itch)**
4. Output: `Builds/Mac/ARTabletopChess.app`
5. Optional: **Chess → Build → Windows Playtest (itch)** if you have Windows target modules

### Zip for itch
```bash
cd /Users/cikirezi/Chess/Builds/Mac
ditto -c -k --sequesterRsrc --keepParent ARTabletopChess.app ARTabletopChess-mac.zip
```

### itch.io
1. https://itch.io/game/new  
2. Title: **AR Tabletop Chess**  
3. Upload zip · Kind: downloadable · Platform: macOS (+ Windows if built)  
4. Visibility: **Public**  
5. Description: Hot-seat + vs Computer · Unity desktop playtest (AR is iPhone bonus)  
6. Copy public page URL into README + Canvas hub

## 3. DevLog (15 min)
1. New Google Doc or Notion page  
2. Paste `docs/submission/DEVLOG.md`  
3. Replace `[YOUR NAME]` / `[TEAMMATE NAME]`  
4. Drop in 4+ screenshots from Play mode  
5. Share → **Anyone with the link can view**  
6. Incognito-test the link

## 4. Tracker (5 min)
1. New Google Sheet  
2. Paste rows from `docs/submission/TRACKER.md`  
3. Leader = edit, teammate = comment  
4. Share public/view or “anyone with link can comment”

## 5. Video (20 min)
1. Follow `docs/submission/VIDEO_SCRIPT.md`  
2. QuickTime screen record while playing `ChessPlaytest`  
3. Upload YouTube Unlisted or Drive anyone-with-link  
4. Incognito-test

## 6. README + Canvas (10 min)
1. Fill names + four URLs in `README.md` Links section  
2. Paste `docs/submission/CANVAS_HUB.md` into Canvas (with URLs filled)  
3. Submit Canvas **before deadline**  
4. Re-open every link in incognito

## Score map (don’t skip)
| Item | Pts | Artifact |
|------|-----|----------|
| GitHub | 5 | public repo + README |
| DevLog | 5 | public dated doc |
| Public build | 3 | itch Public |
| Video | 3 | 5–7 min + spoken Q&A |
| Attribution / tracker | 5 | hub + tracker |
| Presentation | 6 | live or waived + video Q&A |
