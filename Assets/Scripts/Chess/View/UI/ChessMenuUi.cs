using System.Collections;
using Chess.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Chess.View.UI
{
    /// <summary>
    /// Demo-ready uGUI: mode select, in-game HUD, game over, optional AR place panel.
    /// Built at runtime with Fredoka + panel pop animations.
    /// </summary>
    [DefaultExecutionOrder(-20)]
    public class ChessMenuUi : MonoBehaviour
    {
        [SerializeField] ChessGameController controller;
        [SerializeField] Font headlineFont;
        [SerializeField] Font bodyFont;

        RectTransform _root;

        GameObject _placePanel;
        GameObject _modePanel;
        GameObject _hudPanel;
        GameObject _gameOverPanel;
        CanvasGroup _placeGroup;
        CanvasGroup _modeGroup;
        CanvasGroup _hudGroup;
        CanvasGroup _gameOverGroup;

        Text _placeStatus;
        Text _hudHeadline;
        Text _hudSubline;
        Text _hudTip;
        Text _hudModeBadge;
        Text _hudLastMove;
        Text _thinkingBadge;
        Image _hudBanner;
        Image _controlsBar;

        Text _overTitle;
        Text _overSubtitle;

        ParticleSystem _celebrate;
        Chess.AR.ARChessBoardPlacer _placer;
        GameUIManager _gameUIManager;
        bool _wasThinking;

        static readonly Color Cream = new Color(0.98f, 0.95f, 0.90f);
        static readonly Color Ink = new Color(0.18f, 0.14f, 0.12f);
        static readonly Color Accent = new Color(0.95f, 0.45f, 0.35f);
        static readonly Color Mint = new Color(0.35f, 0.72f, 0.58f);
        static readonly Color Night = new Color(0.16f, 0.17f, 0.22f);
        static readonly Color NightText = new Color(0.95f, 0.93f, 0.90f);
        static readonly Color Bar = new Color(0.12f, 0.11f, 0.14f, 0.92f);

        void Awake()
        {
            if (controller == null)
                controller = FindAnyObjectByType<ChessGameController>();

            _placer = FindAnyObjectByType<Chess.AR.ARChessBoardPlacer>();
            _gameUIManager = FindAnyObjectByType<GameUIManager>();
            ResolveFonts();
            BuildUi();
            DisableLegacyHud();
        }

        void Start()
        {
            if (controller == null)
                return;

            controller.OnTipChanged += OnTip;
            controller.OnModeChanged += RefreshPanels;
            var game = controller.Game;
            if (game != null)
            {
                game.OnStatusMessage += OnStatus;
                game.OnMoveApplied += OnMove;
                game.OnTurnChanged += OnTurn;
                game.OnNewGame += OnNewGame;
                game.OnGameOver += OnGameOver;
            }

            if (_placer != null)
            {
                _placer.OnBoardPlaced += OnArBoardPlaced;
                _placer.OnPlacementReset += OnArPlacementReset;
            }

            RefreshPanels();
        }

        void OnDestroy()
        {
            if (_placer != null)
            {
                _placer.OnBoardPlaced -= OnArBoardPlaced;
                _placer.OnPlacementReset -= OnArPlacementReset;
            }

            if (controller == null)
                return;
            controller.OnTipChanged -= OnTip;
            controller.OnModeChanged -= RefreshPanels;
            var game = controller.Game;
            if (game == null)
                return;
            game.OnStatusMessage -= OnStatus;
            game.OnMoveApplied -= OnMove;
            game.OnTurnChanged -= OnTurn;
            game.OnNewGame -= OnNewGame;
            game.OnGameOver -= OnGameOver;
        }

        void Update()
        {
            if (_placePanel != null && _placePanel.activeSelf && _placer != null && _placeStatus != null)
                _placeStatus.text = _placer.StatusText;

            if (controller == null || _thinkingBadge == null)
                return;

            var thinking = controller.ModeChosen && controller.IsComputerThinking;
            if (thinking == _wasThinking)
                return;

            _wasThinking = thinking;
            _thinkingBadge.gameObject.SetActive(thinking);
            if (thinking)
            {
                _hudHeadline.text = "Computer is thinking…";
                _hudSubline.text = "Black is choosing a move";
                if (_hudTip != null)
                    _hudTip.text = "Sit back — pieces move automatically";
            }
        }

        void OnArBoardPlaced()
        {
            if (_placeStatus != null && _placer != null)
                _placeStatus.text = _placer.StatusText;
            RefreshPanels();
        }

        void OnArPlacementReset()
        {
            if (_placeStatus != null)
                _placeStatus.text = "Scan a table, then tap to place the chess board";
            RefreshPanels();
        }

        void ChooseHotSeat()
        {
            if (controller == null)
                return;

            controller.ChooseMode(PlayMode.HotSeat);
            _gameUIManager?.OpenNamePanel();
        }

        void ResolveFonts()
        {
            if (headlineFont == null)
                headlineFont = Resources.Load<Font>("Fonts/Fredoka-Bold");
            if (bodyFont == null)
                bodyFont = Resources.Load<Font>("Fonts/Fredoka-SemiBold");

            if (headlineFont == null || bodyFont == null)
            {
                var builtins = Font.CreateDynamicFontFromOSFont(
                    new[] { "Arial Rounded MT Bold", "Avenir Next", "Helvetica Neue", "Arial" }, 64);
                if (headlineFont == null) headlineFont = builtins;
                if (bodyFont == null) bodyFont = builtins;
            }
        }

        void DisableLegacyHud()
        {
            foreach (var hud in FindObjectsByType<ChessHud>())
                hud.enabled = false;

            foreach (var arHud in FindObjectsByType<Chess.AR.ARChessHud>())
                arHud.enabled = false;
        }

        void BuildUi()
        {
            var canvasGo = new GameObject("ChessFancyCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvasGo.transform.SetParent(transform, false);
            var canvas = canvasGo.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 50;

            // Landscape-first for Editor / itch / presentation demos
            var scaler = canvasGo.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight = 0.5f;

            _root = canvasGo.GetComponent<RectTransform>();

            EnsureEventSystem();
            _placePanel = BuildPlacePanel();
            _modePanel = BuildModePanel();
            _hudPanel = BuildHudPanel();
            _gameOverPanel = BuildGameOverPanel();
            _celebrate = BuildCelebration();

            _placeGroup = _placePanel.GetComponent<CanvasGroup>();
            _modeGroup = _modePanel.GetComponent<CanvasGroup>();
            _hudGroup = _hudPanel.GetComponent<CanvasGroup>();
            _gameOverGroup = _gameOverPanel.GetComponent<CanvasGroup>();
        }

        static void EnsureEventSystem()
        {
            if (FindAnyObjectByType<UnityEngine.EventSystems.EventSystem>() != null)
                return;

            var es = new GameObject("EventSystem");
            es.AddComponent<UnityEngine.EventSystems.EventSystem>();
#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
            es.AddComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>();
#else
            es.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
#endif
        }

        GameObject BuildPlacePanel()
        {
            var panel = CreatePanel("ArPlacePanel", new Color(0.08f, 0.09f, 0.12f, 0.55f), true);
            var card = CreateCard(panel.transform, new Vector2(0.5f, 0.58f), new Vector2(780, 360), Cream);
            AddText(card.transform, "Place the board", 48, Ink, new Vector2(0, 100), new Vector2(700, 70), headlineFont, FontStyle.Bold);
            _placeStatus = AddText(card.transform, "Scan a table, then tap to place the chess board", 24,
                new Color(0.35f, 0.3f, 0.28f), new Vector2(0, 20), new Vector2(680, 80), bodyFont, FontStyle.Normal);
            AddText(card.transform, "Tip: move slowly until a surface highlights, then tap once.", 20,
                new Color(0.4f, 0.36f, 0.33f), new Vector2(0, -80), new Vector2(680, 50), bodyFont, FontStyle.Italic);
            panel.SetActive(false);
            return panel;
        }

        GameObject BuildModePanel()
        {
            var panel = CreatePanel("ModeSelectPanel", new Color(0.08f, 0.09f, 0.12f, 0.62f), true);
            var card = CreateCard(panel.transform, new Vector2(0.5f, 0.52f), new Vector2(860, 560), Cream);

            AddText(card.transform, "AR Tabletop Chess", 52, Ink, new Vector2(0, 200), new Vector2(780, 70), headlineFont, FontStyle.Bold);
            AddText(card.transform, "Real chess rules · pick how you want to play", 24,
                new Color(0.35f, 0.3f, 0.28f), new Vector2(0, 130), new Vector2(760, 40), bodyFont, FontStyle.Normal);

            CreateModeButton(card.transform, "Hot-seat", "2 players · one device\nBoard flips each turn", Mint,
                new Vector2(-180, -10), ChooseHotSeat);
            CreateModeButton(card.transform, "vs Computer", "You play White\nMinimax AI replies", Accent,
                new Vector2(180, -10), () => {
                    controller.ChooseMode(PlayMode.VersusComputer);
                    _gameUIManager?.HideTurnPanel();
                });

            AddText(card.transform, "Green dots = moves   ·   Red rings = captures   ·   Board flips on turn change", 18,
                new Color(0.4f, 0.36f, 0.33f), new Vector2(0, -210), new Vector2(780, 40), bodyFont, FontStyle.Italic);
            return panel;
        }

        GameObject BuildHudPanel()
        {
            var panel = CreatePanel("HudPanel", Color.clear, false);

            var banner = CreateCard(panel.transform, new Vector2(0.5f, 0.92f), new Vector2(920, 150), Cream);
            _hudBanner = banner.GetComponent<Image>();

            _hudModeBadge = AddText(banner.transform, "HOT-SEAT", 16, Accent, new Vector2(-300, 42), new Vector2(180, 28), bodyFont, FontStyle.Bold);
            _thinkingBadge = AddText(banner.transform, "THINKING…", 16, Accent, new Vector2(300, 42), new Vector2(180, 28), bodyFont, FontStyle.Bold);
            _thinkingBadge.gameObject.SetActive(false);

            _hudHeadline = AddText(banner.transform, "White's turn", 36, Ink, new Vector2(0, 18), new Vector2(860, 44), headlineFont, FontStyle.Bold);
            _hudSubline = AddText(banner.transform, "Make your move", 20, new Color(0.35f, 0.32f, 0.3f), new Vector2(0, -18), new Vector2(860, 30), bodyFont, FontStyle.Normal);
            _hudTip = AddText(banner.transform, "Tip: tap a piece, then a glowing square", 16,
                new Color(0.45f, 0.4f, 0.38f), new Vector2(0, -48), new Vector2(860, 26), bodyFont, FontStyle.Italic);

            // Bottom control bar — always visible during play
            var barGo = new GameObject("ControlsBar", typeof(RectTransform), typeof(Image));
            barGo.transform.SetParent(panel.transform, false);
            var barRt = barGo.GetComponent<RectTransform>();
            barRt.anchorMin = new Vector2(0f, 0f);
            barRt.anchorMax = new Vector2(1f, 0f);
            barRt.pivot = new Vector2(0.5f, 0f);
            barRt.sizeDelta = new Vector2(0f, 110f);
            barRt.anchoredPosition = Vector2.zero;
            _controlsBar = barGo.GetComponent<Image>();
            _controlsBar.color = Bar;

            _hudLastMove = AddText(barGo.transform, "Ready when you are", 18, new Color(0.85f, 0.82f, 0.78f),
                new Vector2(0, 28), new Vector2(900, 28), bodyFont, FontStyle.Normal);
            var lastRt = _hudLastMove.rectTransform;
            lastRt.anchorMin = lastRt.anchorMax = new Vector2(0.5f, 0.5f);
            lastRt.anchoredPosition = new Vector2(0f, 28f);

            CreatePillButton(barGo.transform, "New Game", new Vector2(0.28f, 0.32f), Mint, () =>
            {
                controller.ResetGame();
                HideGameOver();
            });
            CreatePillButton(barGo.transform, "Modes", new Vector2(0.50f, 0.32f), Accent, () =>
            {
                HideGameOver();
                controller.OpenModeSelect();
                _gameUIManager?.HideTurnPanel();
                RefreshPanels();
            });

            if (_placer != null)
            {
                CreatePillButton(barGo.transform, "Replace", new Vector2(0.72f, 0.32f), new Color(0.45f, 0.5f, 0.7f), () =>
                {
                    HideGameOver();
                    _placer.ResetPlacement();
                    if (_hudTip != null)
                        _hudTip.text = "Scan a table, then tap to place the board";
                    RefreshPanels();
                });
            }

            panel.SetActive(false);
            return panel;
        }

        GameObject BuildGameOverPanel()
        {
            var panel = CreatePanel("GameOverPanel", new Color(0.05f, 0.05f, 0.08f, 0.72f), true);
            var card = CreateCard(panel.transform, new Vector2(0.5f, 0.5f), new Vector2(720, 420), Cream);
            _overTitle = AddText(card.transform, "Checkmate!", 56, Accent, new Vector2(0, 110), new Vector2(640, 70), headlineFont, FontStyle.Bold);
            _overSubtitle = AddText(card.transform, "White wins the game", 26, Ink, new Vector2(0, 40), new Vector2(620, 50), bodyFont, FontStyle.Normal);
            AddText(card.transform, "Rematch or switch modes anytime.", 20,
                new Color(0.4f, 0.36f, 0.33f), new Vector2(0, -10), new Vector2(600, 36), bodyFont, FontStyle.Italic);

            CreatePillButton(card.transform, "Rematch", new Vector2(0.5f, 0.28f), Mint, () =>
            {
                controller.ResetGame();
                HideGameOver();
            }, true);
            CreatePillButton(card.transform, "Change Mode", new Vector2(0.5f, 0.12f), Accent, () =>
            {
                controller.OpenModeSelect();
                _gameUIManager?.HideTurnPanel();
                HideGameOver();
                RefreshPanels();
            }, true);

            panel.SetActive(false);
            return panel;
        }

        ParticleSystem BuildCelebration()
        {
            var go = new GameObject("Celebrate");
            go.transform.SetParent(transform, false);
            var ps = go.AddComponent<ParticleSystem>();
            var main = ps.main;
            main.startLifetime = 1.4f;
            main.startSpeed = 3.5f;
            main.startSize = 0.08f;
            main.maxParticles = 80;
            main.loop = false;
            main.playOnAwake = false;
            main.startColor = new ParticleSystem.MinMaxGradient(Accent, Mint);
            main.gravityModifier = 0.8f;
            var emission = ps.emission;
            emission.rateOverTime = 0;
            emission.SetBursts(new[] { new ParticleSystem.Burst(0f, 60) });
            var shape = ps.shape;
            shape.shapeType = ParticleSystemShapeType.Cone;
            shape.angle = 25;
            go.transform.position = new Vector3(0f, 0.4f, -0.2f);
            var renderer = go.GetComponent<ParticleSystemRenderer>();
            renderer.material = new Material(Shader.Find("Sprites/Default"));
            return ps;
        }

        void RefreshPanels()
        {
            if (controller == null)
                return;

            var needsPlace = _placer != null && !_placer.IsPlaced;

            if (needsPlace)
            {
                _gameOverPanel.SetActive(false);
                _hudPanel.SetActive(false);
                _modePanel.SetActive(false);
                _placePanel.SetActive(true);
                if (_placeStatus != null)
                    _placeStatus.text = _placer.StatusText;
                StartCoroutine(ChessUiAnimator.PopIn(_placeGroup, _placePanel.transform.GetChild(0) as RectTransform));
                return;
            }

            _placePanel.SetActive(false);

            if (!controller.ModeChosen)
            {
                _gameOverPanel.SetActive(false);
                _hudPanel.SetActive(false);
                _modePanel.SetActive(true);
                StartCoroutine(ChessUiAnimator.PopIn(_modeGroup, _modePanel.transform.GetChild(0) as RectTransform));
            }
            else
            {
                _modePanel.SetActive(false);
                _hudPanel.SetActive(true);
                StartCoroutine(ChessUiAnimator.PopIn(_hudGroup, _hudPanel.transform.GetChild(0) as RectTransform));
                _hudModeBadge.text = controller.Mode == PlayMode.VersusComputer ? "VS COMPUTER" : "HOT-SEAT";
                if (_hudLastMove != null && string.IsNullOrEmpty(_hudLastMove.text))
                    _hudLastMove.text = "Tap a piece, then a glowing square";
            }
        }

        void OnTip(string tip)
        {
            if (_hudTip != null)
                _hudTip.text = tip;
        }

        void OnStatus(string status)
        {
            if (_hudSubline != null)
                _hudSubline.text = status;
        }

        void OnTurn(PieceColor side)
        {
            var vsAi = controller != null && controller.Mode == PlayMode.VersusComputer;
            if (vsAi)
            {
                _hudHeadline.text = side == PieceColor.White ? "Your turn" : "Computer's turn";
                _hudSubline.text = side == PieceColor.White ? "Play as White" : "Black is thinking…";
            }
            else
            {
                _hudHeadline.text = side == PieceColor.White ? "White's turn" : "Black's turn";
                _hudSubline.text = "Pass the device — board faces you";
            }

            ApplyBannerTheme(side);
            if (_hudBanner != null)
                StartCoroutine(ChessUiAnimator.PunchScale(_hudBanner.rectTransform));
        }

        void OnMove(MoveEvent moveEvent)
        {
            var mover = moveEvent.SideThatMoved;
            var from = FormatSquare(moveEvent.Move.From);
            var to = FormatSquare(moveEvent.Move.To);
            var line = $"{mover}: {from} → {to}";

            if (moveEvent.WasCapture)
            {
                _hudHeadline.text = "Gotcha!";
                _hudSubline.text = $"{moveEvent.Captured.Type} joins the capture tray";
                line += $"  ·  captured {moveEvent.Captured.Type}";
            }
            else if (moveEvent.GaveCheck && moveEvent.ResultAfterMove == GameResult.Playing)
            {
                _hudHeadline.text = "Check!";
                _hudSubline.text = $"Protect the {moveEvent.SideToMoveAfter} king";
                line += "  ·  check!";
            }
            else if (controller != null && controller.Mode == PlayMode.VersusComputer && mover == PieceColor.Black)
            {
                _hudHeadline.text = "Computer moved";
                _hudSubline.text = "Your turn as White";
            }

            if (_hudLastMove != null)
                _hudLastMove.text = line;

            if (_hudBanner != null)
                StartCoroutine(ChessUiAnimator.PunchScale(_hudBanner.rectTransform, 1.06f, 0.22f));
        }

        void OnNewGame()
        {
            HideGameOver();
            _wasThinking = false;
            if (_thinkingBadge != null)
                _thinkingBadge.gameObject.SetActive(false);

            _hudHeadline.text = "New game!";
            _hudSubline.text = controller != null && controller.Mode == PlayMode.VersusComputer
                ? "You are White — good luck"
                : "White goes first";
            if (_hudLastMove != null)
                _hudLastMove.text = "Fresh board — tap a white piece to start";
            ApplyBannerTheme(PieceColor.White);
        }

        void OnGameOver(GameResult result, PieceColor? winner)
        {
            _gameOverPanel.SetActive(true);
            if (result == GameResult.Checkmate)
            {
                _overTitle.text = "Checkmate!";
                _overSubtitle.text = winner.HasValue ? $"{winner.Value} wins the game" : "Game over";
                if (_hudLastMove != null)
                    _hudLastMove.text = winner.HasValue ? $"Checkmate — {winner.Value} wins" : "Checkmate";
                _celebrate?.Play();
            }
            else
            {
                _overTitle.text = "Stalemate!";
                _overSubtitle.text = "It's a peaceful draw";
                if (_hudLastMove != null)
                    _hudLastMove.text = "Stalemate — draw";
            }

            StartCoroutine(ChessUiAnimator.PopIn(_gameOverGroup, _gameOverPanel.transform.GetChild(0) as RectTransform, 0.4f));
        }

        void HideGameOver()
        {
            if (_gameOverPanel != null)
                _gameOverPanel.SetActive(false);
        }

        void ApplyBannerTheme(PieceColor side)
        {
            if (_hudBanner == null)
                return;

            if (side == PieceColor.White)
            {
                _hudBanner.color = Cream;
                SetTextColor(_hudHeadline, Ink);
                SetTextColor(_hudSubline, new Color(0.35f, 0.32f, 0.3f));
                SetTextColor(_hudTip, new Color(0.45f, 0.4f, 0.38f));
                SetTextColor(_hudModeBadge, Accent);
            }
            else
            {
                _hudBanner.color = Night;
                SetTextColor(_hudHeadline, NightText);
                SetTextColor(_hudSubline, new Color(0.8f, 0.78f, 0.75f));
                SetTextColor(_hudTip, new Color(0.7f, 0.68f, 0.65f));
                SetTextColor(_hudModeBadge, Mint);
            }
        }

        static string FormatSquare(Square square) => square.ToString();

        static void SetTextColor(Text text, Color color)
        {
            if (text != null)
                text.color = color;
        }

        GameObject CreatePanel(string name, Color backdrop, bool stretchFull)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(CanvasGroup), typeof(Image));
            go.transform.SetParent(_root, false);
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
            var img = go.GetComponent<Image>();
            img.color = backdrop;
            img.raycastTarget = backdrop.a > 0.01f;
            return go;
        }

        GameObject CreateCard(Transform parent, Vector2 anchor, Vector2 size, Color color)
        {
            var go = new GameObject("Card", typeof(RectTransform), typeof(Image));
            go.transform.SetParent(parent, false);
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = anchor;
            rt.anchorMax = anchor;
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.sizeDelta = size;
            rt.anchoredPosition = Vector2.zero;
            var img = go.GetComponent<Image>();
            img.color = color;
            var outline = go.AddComponent<Outline>();
            outline.effectColor = new Color(0, 0, 0, 0.14f);
            outline.effectDistance = new Vector2(4, -4);
            return go;
        }

        void CreateModeButton(Transform parent, string title, string subtitle, Color color, Vector2 pos, UnityEngine.Events.UnityAction onClick)
        {
            var go = new GameObject(title + "Button", typeof(RectTransform), typeof(Image), typeof(Button));
            go.transform.SetParent(parent, false);
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.42f);
            rt.sizeDelta = new Vector2(300, 180);
            rt.anchoredPosition = pos;
            var img = go.GetComponent<Image>();
            img.color = color;
            var button = go.GetComponent<Button>();
            button.targetGraphic = img;
            button.onClick.AddListener(onClick);

            var outline = go.AddComponent<Outline>();
            outline.effectColor = new Color(1f, 1f, 1f, 0.25f);
            outline.effectDistance = new Vector2(2, -2);

            AddText(go.transform, title, 32, Color.white, new Vector2(0, 32), new Vector2(270, 44), headlineFont, FontStyle.Bold);
            AddText(go.transform, subtitle, 18, new Color(1f, 1f, 1f, 0.92f), new Vector2(0, -28), new Vector2(270, 80), bodyFont, FontStyle.Normal);
        }

        void CreatePillButton(Transform parent, string label, Vector2 anchor, Color color, UnityEngine.Events.UnityAction onClick, bool relativeToCard = false)
        {
            var go = new GameObject(label + "Btn", typeof(RectTransform), typeof(Image), typeof(Button));
            go.transform.SetParent(parent, false);
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = rt.anchorMax = anchor;
            rt.anchoredPosition = Vector2.zero;
            rt.sizeDelta = relativeToCard ? new Vector2(260, 64) : new Vector2(220, 56);

            var img = go.GetComponent<Image>();
            img.color = color;
            var button = go.GetComponent<Button>();
            button.targetGraphic = img;
            button.onClick.AddListener(onClick);

            var outline = go.AddComponent<Outline>();
            outline.effectColor = new Color(0f, 0f, 0f, 0.2f);
            outline.effectDistance = new Vector2(2, -2);

            AddText(go.transform, label, 22, Color.white, Vector2.zero, new Vector2(200, 44), headlineFont, FontStyle.Bold);
        }

        Text AddText(Transform parent, string content, int size, Color color, Vector2 pos, Vector2 sizeDelta, Font font, FontStyle style)
        {
            var go = new GameObject("Text", typeof(RectTransform), typeof(Text));
            go.transform.SetParent(parent, false);
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.anchoredPosition = pos;
            rt.sizeDelta = sizeDelta;
            var text = go.GetComponent<Text>();
            text.text = content;
            text.font = font != null ? font : Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = size;
            text.fontStyle = style;
            text.color = color;
            text.alignment = TextAnchor.MiddleCenter;
            text.horizontalOverflow = HorizontalWrapMode.Wrap;
            text.verticalOverflow = VerticalWrapMode.Overflow;
            text.raycastTarget = false;
            return text;
        }
    }
}
