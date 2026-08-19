using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SymbolInfoCard : MonoBehaviour
{
    [Header("UI Component References")]
    [SerializeField] private Image cardBgImage;
    [SerializeField] private TMP_Text infoText;

    [Header("Pointer Sprites")]
    [Tooltip("Sprite used when card is on the RIGHT side of symbol (1st & 2nd reel - pointer points left)")]
    [SerializeField] private Sprite rightSideCardSprite;
    [Tooltip("Sprite used when card is on the LEFT side of symbol (3rd, 4th, 5th reel - pointer points right)")]
    [SerializeField] private Sprite leftSideCardSprite;

    [Header("Layout & Auto-Close Settings")]
    [Tooltip("Horizontal spacing from symbol center")]
    [SerializeField] private float xSpacing = 160f;
    [Tooltip("Vertical offset adjustment")]
    [SerializeField] private float yOffset = 0f;
    [Tooltip("Auto close duration in seconds")]
    [SerializeField] private float autoCloseDuration = 1.5f;
    [Tooltip("Reel columns to the left of this index place the card on the right; columns at/after it place the card on the left")]
    [SerializeField] private int leftSideStartColumn = 2;

    private RectTransform rectTransform;
    private int activeCol = -1;
    private int activeRow = -1;
    private int activeSymbolId = -1;
    private SocketIOManager cachedSocketManager;
    private int cachedBetCounter;
    private Coroutine autoCloseCoroutine;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void ShowCard(int symbolId, int colIndex, int rowIndex, RectTransform symbolRect, SocketIOManager socketManager, int betCounter)
    {
        // Toggle hide if clicking the exact same symbol position while visible
        if (gameObject.activeSelf && activeCol == colIndex && activeRow == rowIndex)
        {
            HideCard();
            return;
        }

        // Cancel any active auto-close timer
        if (autoCloseCoroutine != null)
        {
            StopCoroutine(autoCloseCoroutine);
            autoCloseCoroutine = null;
        }

        activeCol = colIndex;
        activeRow = rowIndex;
        activeSymbolId = symbolId;
        cachedSocketManager = socketManager;
        cachedBetCounter = betCounter;

        if (rectTransform == null)
            rectTransform = GetComponent<RectTransform>();

        // 1. Position Card based on reel/column index
        Vector3 symbolWorldPos = symbolRect != null ? symbolRect.position : transform.position;
        Vector3 localPos = transform.parent != null ? transform.parent.InverseTransformPoint(symbolWorldPos) : symbolWorldPos;

        float offsetDir = (colIndex < leftSideStartColumn) ? Mathf.Abs(xSpacing) : -Mathf.Abs(xSpacing);
        rectTransform.localPosition = new Vector3(localPos.x + offsetDir, localPos.y + yOffset, localPos.z);

        // 2. Change sprite based on side
        if (cardBgImage != null)
        {
            Sprite targetSprite = (colIndex < leftSideStartColumn) ? rightSideCardSprite : leftSideCardSprite;
            if (targetSprite != null)
            {
                cardBgImage.sprite = targetSprite;
            }
        }

        // 3. Setup info text content
        SetupCardContent(symbolId, socketManager, betCounter);

        gameObject.SetActive(true);

        // 4. Start auto-close timer
        autoCloseCoroutine = StartCoroutine(AutoCloseTimer(autoCloseDuration));
    }

    private IEnumerator AutoCloseTimer(float duration)
    {
        yield return new WaitForSeconds(duration);
        HideCard();
    }

    public void RefreshCard(SocketIOManager socketManager, int betCounter)
    {
        if (!gameObject.activeSelf || activeSymbolId < 0) return;
        cachedSocketManager = socketManager;
        cachedBetCounter = betCounter;
        SetupCardContent(activeSymbolId, cachedSocketManager, cachedBetCounter);

        // Reset auto close timer on refresh
        if (autoCloseCoroutine != null)
        {
            StopCoroutine(autoCloseCoroutine);
        }
        autoCloseCoroutine = StartCoroutine(AutoCloseTimer(autoCloseDuration));
    }

    private void SetupCardContent(int symbolId, SocketIOManager socketManager, int betCounter)
    {
        if (infoText == null) return;

        Symbol symbolInfo = null;
        if (socketManager != null && socketManager.initUIData != null && socketManager.initUIData.paylines != null && socketManager.initUIData.paylines.symbols != null)
        {
            symbolInfo = socketManager.initUIData.paylines.symbols.Find(s => s.id == symbolId);
        }

        if (symbolInfo != null && socketManager.initialData != null && socketManager.initialData.bets != null && betCounter >= 0 && betCounter < socketManager.initialData.bets.Count)
        {
            double payout = symbolInfo.multiplier[0] * socketManager.initialData.bets[betCounter];
            infoText.text = $"{symbolInfo.name}\n{payout:0.##}";
        }
        else
        {
            infoText.text = symbolInfo != null ? symbolInfo.name : "";
        }
    }

    public void HideCard()
    {
        if (autoCloseCoroutine != null)
        {
            StopCoroutine(autoCloseCoroutine);
            autoCloseCoroutine = null;
        }

        activeCol = -1;
        activeRow = -1;
        activeSymbolId = -1;

        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        if (autoCloseCoroutine != null)
        {
            StopCoroutine(autoCloseCoroutine);
            autoCloseCoroutine = null;
        }
    }
}
