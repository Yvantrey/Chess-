using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Chess.Core;
using Chess.View;

public class GameUIManager : MonoBehaviour
{
    public GameObject playerNamePanel;
    public GameObject turnPanel;

    public TMP_InputField whiteInput;
    public TMP_InputField blackInput;

    public TMP_Text whiteNameText;
    public TMP_Text blackNameText;
    public TMP_Text turnText;

    private string whitePlayer = "White";
    private string blackPlayer = "Black";
    private ChessGameController _controller;

    public void OpenNamePanel()
    {
        if (playerNamePanel != null)
            playerNamePanel.SetActive(true);
    }

    public void StartGame()
    {
        if (whiteInput != null && !string.IsNullOrEmpty(whiteInput.text))
            whitePlayer = whiteInput.text;

        if (blackInput != null && !string.IsNullOrEmpty(blackInput.text))
            blackPlayer = blackInput.text;

        if (whiteNameText != null)
            whiteNameText.text = "White : " + whitePlayer;
        if (blackNameText != null)
            blackNameText.text = "Black : " + blackPlayer;

        if (turnText != null)
            turnText.text = "Turn : " + whitePlayer;

        if (playerNamePanel != null)
            playerNamePanel.SetActive(false);

        ShowTurnPanel();

        if (_controller == null)
            _controller = FindAnyObjectByType<ChessGameController>();
        if (_controller != null)
        {
            _controller.Game.OnTurnChanged -= HandleTurnChanged;
            _controller.Game.OnTurnChanged += HandleTurnChanged;
        }
    }

    public void UpdateTurn(bool isWhiteTurn)
    {
        if (turnText == null)
            return;

        turnText.text = isWhiteTurn ? "Turn : " + whitePlayer : "Turn : " + blackPlayer;
    }

    public void HideTurnPanel()
    {
        if (turnPanel != null)
            turnPanel.SetActive(false);
    }

    public void ShowTurnPanel()
    {
        if (turnPanel == null)
            return;

        turnPanel.SetActive(true);
        EnsureTurnPanelGroup();
    }

    void EnsureTurnPanelGroup()
    {
        if (turnPanel == null)
            return;

        var canvasGroup = turnPanel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = turnPanel.AddComponent<CanvasGroup>();

        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }

    void HandleTurnChanged(PieceColor side)
    {
        if (turnText == null)
            return;

        turnText.text = side == PieceColor.White ? "Turn : " + whitePlayer : "Turn : " + blackPlayer;
    }
}