using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class UIManager : MonoBehaviour
{
    [Header("Slot UI")]
    [SerializeField] internal Button SpinButton;
    [SerializeField] internal Button StopSpinButton;
    //[SerializeField] internal Button AutoSpinButton;

    [Header("Slot UI - Portrait")]
    [SerializeField] internal Button SpinButtonPortrait;
    [SerializeField] internal Button StopSpinButtonPortrait;

    [Header("Main UI Text")]
    [SerializeField] private TMP_Text Balance_Text;
    [SerializeField] private TMP_Text WinAmount_Text;
    [SerializeField] private TMP_Text Bet_Text;
    [SerializeField] private Button BetPlus_Button;
    [SerializeField] private Button BetMinus_Button;

    [Header("Main UI Text - Portrait")]
    [SerializeField] private TMP_Text Balance_Text_Portrait;
    [SerializeField] private TMP_Text WinAmount_Text_Portrait;
    [SerializeField] private TMP_Text Bet_Text_Portrait;
    [SerializeField] private Button BetPlus_Button_Portrait;
    [SerializeField] private Button BetMinus_Button_Portrait;

    [Header("Free Spin")]
    [SerializeField] private GameObject FreeSpinCountPanel;
    [SerializeField] private TMP_Text FreeSpinText;

    [Header("Free Spin - Portrait")]
    [SerializeField] private GameObject FreeSpinCountPanelPortrait;
    [SerializeField] private TMP_Text FreeSpinTextPortrait;

    [Header("Bg UI Reference")]
    [SerializeField] private Image Bg_Image;
    [SerializeField] private Image Bg_Image_Portrait;
    // [SerializeField] private Sprite Blue_Sprite;
    // [SerializeField] private Sprite Red_Sprite;

    // [SerializeField] private GameObject shinePrefab;
    // [SerializeField] private RectTransform parentRect;

    [Header("Main Popus UI Object")]
    [SerializeField] private GameObject MainPopup_Object;

    [Header("Paytable Popup UI References")]
    [SerializeField] private GameObject[] Pages;
    [SerializeField] private GameObject[] PageIndicator;
    [SerializeField] private Button Paytable_Button;
    [SerializeField] private GameObject PaytablePopup_Object;
    [SerializeField] private Button PaytableExit_Button;
    [SerializeField] private Button PaytableLeft_Button;
    [SerializeField] private Button PaytableRight_Button;

    [SerializeField] private TMP_Text WildText;
    [SerializeField] private TMP_Text TripleSevenText;
    [SerializeField] private TMP_Text DoubleSevenText;
    [SerializeField] private TMP_Text SevenText;
    [SerializeField] private TMP_Text TwoBarText;
    [SerializeField] private TMP_Text SevenMixedText;
    [SerializeField] private TMP_Text BarText;
    [SerializeField] private TMP_Text BarMixedText;

    [Header("Sound/Music UI References")]
    [SerializeField] private Button Sound_Button;
    [SerializeField] private Button Music_Button;
    [SerializeField] private Sprite SoundOff_Sprite;
    [SerializeField] private Sprite SoundOn_Sprite;
    [SerializeField] private Sprite MusicOff_Sprite;
    [SerializeField] private Sprite MusicOn_Sprite;

    [Header("Disconnection Popup UI References")]
    [SerializeField] private Button CloseDisconnect_Button;
    [SerializeField] private GameObject DisconnectPopup_Object;

    [Header("Reconnection Popup")]
    [SerializeField] private GameObject ReconnectPopup_Object;

    [Header("AnotherDevice Popup UI References")]
    [SerializeField] private Button CloseAD_Button;
    [SerializeField] private GameObject ADPopup_Object;

    [Header("LowBalance Popup UI References")]
    [SerializeField] private Button LBExit_Button;
    [SerializeField] private GameObject LBPopup_Object;

    [Header("Quit Popup UI References")]
    [SerializeField] private GameObject QuitPopup_Object;
    [SerializeField] private Button YesQuit_Button;
    [SerializeField] private Button NoQuit_Button;
    [SerializeField] private Button CrossQuit_Button;
    [SerializeField] private Button GameExit_Button;

    [Header("Managers")]
    [SerializeField] private AudioController audioController;
    [SerializeField] private SocketIOManager socketManager;
    [SerializeField] private SlotManager slotManager;
    [SerializeField] private BonusManager bonusManager;
    [SerializeField] private JSFunctCalls jsFunctCalls;

    internal double currentBalance = 0;
    internal double currentTotalBet = 0;
    internal int betCounter = 0;
    private bool isMusic = true;
    private bool isSound = true;
    private bool isExit = false;

    private int paytablePageCounter;
    private Coroutine bgAnimationRoutine;

    private void Awake()
    {
        if (jsFunctCalls != null)
            jsFunctCalls.RegisterVisibilityListener(gameObject.name);
    }

    public void OnFocusChanged(string value)
    {
        bool focused = value == "1";
        Debug.Log("UNITY FOCUS CHANGED: " + value + " (focused: " + focused + ")");
        if (audioController != null) audioController.SetMuteAll(!focused);
        if (socketManager != null) socketManager.HandleFocusChange(focused);
    }

    private void Start()
    {
        if (SpinButton) SpinButton.onClick.AddListener(OnSpinButtonPressed);
        //if (AutoSpinButton) AutoSpinButton.onClick.AddListener(OnAutoSpinButtonPressed);
        if (StopSpinButton) StopSpinButton.onClick.AddListener(OnStopSpinButtonPressed);

        if (BetPlus_Button) BetPlus_Button.onClick.AddListener(delegate { ChangeBet(true); });
        if (BetMinus_Button) BetMinus_Button.onClick.AddListener(delegate { ChangeBet(false); });

        isMusic = true;
        isSound = true;

        if (Paytable_Button) Paytable_Button.onClick.RemoveAllListeners();
        if (Paytable_Button) Paytable_Button.onClick.AddListener(delegate { OpenPaytable(); });

        if (PaytableExit_Button) PaytableExit_Button.onClick.RemoveAllListeners();
        if (PaytableExit_Button) PaytableExit_Button.onClick.AddListener(delegate { ClosePopup(PaytablePopup_Object); });

        if (PaytableLeft_Button) PaytableLeft_Button.onClick.RemoveAllListeners();
        if (PaytableLeft_Button) PaytableLeft_Button.onClick.AddListener(() => { SwitchPages(false); });

        if (PaytableRight_Button) PaytableRight_Button.onClick.RemoveAllListeners();
        if (PaytableRight_Button) PaytableRight_Button.onClick.AddListener(() => { SwitchPages(true); });

        if (GameExit_Button) GameExit_Button.onClick.RemoveAllListeners();
        if (GameExit_Button) GameExit_Button.onClick.AddListener(delegate { OpenPopup(QuitPopup_Object); });

        if (NoQuit_Button) NoQuit_Button.onClick.RemoveAllListeners();
        if (NoQuit_Button) NoQuit_Button.onClick.AddListener(delegate { if (!isExit) ClosePopup(QuitPopup_Object); });

        if (CrossQuit_Button) CrossQuit_Button.onClick.RemoveAllListeners();
        if (CrossQuit_Button) CrossQuit_Button.onClick.AddListener(delegate { if (!isExit) ClosePopup(QuitPopup_Object); });

        if (LBExit_Button) LBExit_Button.onClick.RemoveAllListeners();
        if (LBExit_Button) LBExit_Button.onClick.AddListener(delegate { ClosePopup(LBPopup_Object); });

        if (YesQuit_Button) YesQuit_Button.onClick.RemoveAllListeners();
        if (YesQuit_Button) YesQuit_Button.onClick.AddListener(delegate { CallOnExitFunction(); });

        if (CloseDisconnect_Button) CloseDisconnect_Button.onClick.RemoveAllListeners();
        if (CloseDisconnect_Button) CloseDisconnect_Button.onClick.AddListener(CallOnExitFunction);

        if (CloseAD_Button) CloseAD_Button.onClick.RemoveAllListeners();
        if (CloseAD_Button) CloseAD_Button.onClick.AddListener(CallOnExitFunction);

        if (Sound_Button) Sound_Button.onClick.RemoveAllListeners();
        if (Sound_Button) Sound_Button.onClick.AddListener(ToggleSound);

        if (Music_Button) Music_Button.onClick.RemoveAllListeners();
        if (Music_Button) Music_Button.onClick.AddListener(ToggleMusic);

        //StartBGAnimation();
    }

    #region Audio Buttons

    private void ToggleMusic()
    {
        audioController.PlayUIButton(false);
        if (isMusic)
        {
            Music_Button.image.sprite = MusicOff_Sprite;
            if (audioController) audioController.PlayUIButton(false);
            if (audioController) audioController.MuteBackground(true);
            isMusic = false;
        }
        else
        {
            Music_Button.image.sprite = MusicOn_Sprite;
            if (audioController) audioController.PlayUIButton(false);
            if (audioController) audioController.MuteBackground(false);
            isMusic = true;
        }
    }

    private void ToggleSound()
    {
        audioController.PlayUIButton(false);
        if (isSound)
        {
            Sound_Button.image.sprite = SoundOff_Sprite;
            if (audioController) audioController.PlayUIButton(false);
            if (audioController) audioController.MuteGame(true);
            isSound = false;
        }
        else
        {
            Sound_Button.image.sprite = SoundOn_Sprite;
            if (audioController) audioController.PlayUIButton(false);
            if (audioController) audioController.MuteGame(false);
            isSound = true;
        }
    }

    #endregion

    #region Button Functionality

    private void OnSpinButtonPressed()
    {
        if (!bonusManager.isBonusFinished)
        {
            bonusManager.wheelButtonPressed = true;
            SpinButton.interactable = false;
        }
        else
        {
            audioController.PlayUIButton(false);
            SpinButton.interactable = false;
            SetBetButtonsInteractable(false);

            slotManager.StartSlots();
            StopSpinButton.gameObject.SetActive(true);
            SpinButton.gameObject.SetActive(false);
        }
    }

    internal void OnStopSpinButtonPressed()
    {
        audioController.PlayUIButton(false);
        slotManager.RequestInstantStop();

        if (slotManager._isAutoSpin)
        {
            slotManager.StopAutoSpin();
            AutoSpinButtonAnimation(false);
            //AutoSpinButton.interactable = false;
        }

        StopSpinButton.interactable = false;
        StopSpinButton.gameObject.SetActive(true);
        SpinButton.gameObject.SetActive(false);
    }

    internal void OnStopSpinButtonTrigger()
    {
        OnStopSpinButtonPressed();
    }

    internal void SetSpinButtonReady()
    {
        StopSpinButton.interactable = true;

        if (slotManager._isAutoSpin)
        {
            StopSpinButton.gameObject.SetActive(true);
            SpinButton.gameObject.SetActive(false);
        }
        else
        {
            SpinButton.interactable = true;
            SpinButton.gameObject.SetActive(true);
            StopSpinButton.gameObject.SetActive(false);
            SetBetButtonsInteractable(true);
            //AutoSpinButton.interactable = true;
        }
    }

    private void OnAutoSpinButtonPressed()
    {
        audioController.PlayUIButton(false);
        if (!bonusManager.isBonusFinished)
        {
            if (!slotManager._isAutoSpin)
            {
                slotManager.AutoSpin();
                SetBetButtonsInteractable(false);
                AutoSpinButtonAnimation(true);
            }
            else
            {
                //AutoSpinButton.interactable = false;
                slotManager.StopAutoSpin();
                AutoSpinButtonAnimation(false);
            }
        }
        else
        {
            if (!slotManager._isAutoSpin)
            {
                slotManager.AutoSpin();
                SetBetButtonsInteractable(false);
                StopSpinButton.gameObject.SetActive(true);
                SpinButton.gameObject.SetActive(false);
                AutoSpinButtonAnimation(true);
            }
            else
            {
                //AutoSpinButton.interactable = false;
                slotManager.StopAutoSpin();
                StopSpinButton.interactable = false;
                StopSpinButton.gameObject.SetActive(true);
                SpinButton.gameObject.SetActive(false);
                AutoSpinButtonAnimation(false);
            }
        }
    }

    private void ChangeBet(bool IncDec)
    {
        if (audioController)
            audioController.PlayUIButton(false);
        if (IncDec)
        {
            betCounter++;
            if (betCounter >= socketManager.initialData.bets.Count) betCounter = 0;
        }
        else
        {
            betCounter--;
            if (betCounter < 0) betCounter = socketManager.initialData.bets.Count - 1;
        }
        if (Bet_Text) Bet_Text.text = (socketManager.initialData.bets[betCounter] * socketManager.initialData.lines.Count).ToString();
        currentTotalBet = socketManager.initialData.bets[betCounter] * socketManager.initialData.lines.Count;
        InitialisePayTable();
    }

    #endregion

    #region Bgs

    internal void ToggleBonusBackground(bool isBonus)
    {
        if (isBonus)
        {
            //Bg_Image.sprite = Red_Sprite;
        }
        else
        {
            //Bg_Image.sprite = Blue_Sprite;
        }
    }

    #endregion

    #region Free Spin
    internal void DecrementFreeSpinCount()
    {
        if (FreeSpinText != null &&
            int.TryParse(FreeSpinText.text, out int current) && current > 0)
        {
            FreeSpinText.text = (current - 1).ToString();
        }
    }

    internal void ToggleFreeSpinUI(bool isFreeSpin, int freeSpinLeft = 0)
    {
        if (isFreeSpin)
        {
            FreeSpinCountPanel.SetActive(true);
            if (FreeSpinText) FreeSpinText.text = freeSpinLeft.ToString();

            SpinButton.interactable = false;
            SpinButton.gameObject.SetActive(false);
            StopSpinButton.interactable = false;
            StopSpinButton.gameObject.SetActive(true);
        }
        else
        {
            FreeSpinCountPanel.SetActive(false);
        }
    }
    #endregion

    #region Text Update

    internal void UpdateBalance(double balance, bool doAnimate = false)
    {
        if (doAnimate)
        {
            Tween balanceTween;
            double displayAmount = currentBalance;
            balanceTween = DOTween.To(() => displayAmount, val =>
            {
                displayAmount = val;
                Balance_Text.text = val.ToString("F2");
            }, balance, 1.5f);
        }
        else
        {
            if (Balance_Text) Balance_Text.text = balance.ToString("F2");
        }
    }

    internal void UpdateBalanceDisplay(double newBalance)
    {
        currentBalance = newBalance;
        UpdateBalance(newBalance);
        if (currentBalance < currentTotalBet)
            LowBalPopup();
    }

    internal void UpdateWin(double winAmount, bool doAnimate = false)
    {
        if (doAnimate)
        {
            Tween winTween;
            double displayAmount = 0f;
            winTween = DOTween.To(() => displayAmount, val =>
            {
                displayAmount = val;
                WinAmount_Text.text = val.ToString("F2");
            }, winAmount, 1.5f);
        }
        else
        {
            if (WinAmount_Text) WinAmount_Text.text = winAmount.ToString("F2");
        }
    }

    #endregion

    #region Popups

    private void CallOnExitFunction()
    {
        isExit = true;
        audioController.PlayUIButton(false);
        socketManager.CloseGame();
    }

    private void OpenPopup(GameObject Popup)
    {
        audioController.PlayUIButton(false);
        if (Popup) Popup.SetActive(true);
        if (MainPopup_Object) MainPopup_Object.SetActive(true);
    }

    private void ClosePopup(GameObject Popup)
    {
        audioController.PlayUIButton(false);
        if (!DisconnectPopup_Object.activeSelf)
        {
            if ((Popup == PaytablePopup_Object && PaytablePopup_Object.activeSelf) || (Popup == QuitPopup_Object && QuitPopup_Object.activeSelf) || (Popup == LBPopup_Object && LBPopup_Object.activeSelf))
            {
                if (MainPopup_Object) MainPopup_Object.SetActive(false);
            }
        }
        if (Popup) Popup.SetActive(false);
    }

    internal void CheckAndClosePopups()
    {
        if (ReconnectPopup_Object.activeInHierarchy) ClosePopup(ReconnectPopup_Object);
        if (DisconnectPopup_Object.activeInHierarchy) ClosePopup(DisconnectPopup_Object);
    }

    internal void LowBalPopup()
    {
        OpenPopup(LBPopup_Object);
    }

    internal void DisconnectionPopup()
    {
        if (!isExit)
        {
            isExit = true;
            OpenPopup(DisconnectPopup_Object);
        }
    }

    internal void ReconnectionPopup()
    {
        OpenPopup(ReconnectPopup_Object);
    }

    internal void ADfunction()
    {
        OpenPopup(ADPopup_Object);
    }

    #endregion

    #region PayTable

    private void OpenPaytable()
    {
        audioController.PlayUIButton(false);
        foreach (GameObject gameObject in Pages)
            gameObject.SetActive(false);
        paytablePageCounter = 0;
        Pages[0].SetActive(true);
        MainPopup_Object.SetActive(true);
        PaytablePopup_Object.SetActive(true);
    }

    private void SwitchPages(bool IncDec)
    {
        audioController.PlayUIButton(false);
        if (IncDec)
        {
            paytablePageCounter++;
            if (paytablePageCounter == Pages.Length) paytablePageCounter = 0;
        }
        else
        {
            paytablePageCounter--;
            if (paytablePageCounter == -1) paytablePageCounter = Pages.Length - 1;
        }
        foreach (GameObject gameObject in Pages)
            gameObject.SetActive(false);

        Pages[paytablePageCounter].SetActive(true);

        foreach (GameObject gameObject in PageIndicator)
            gameObject.SetActive(false);

        PageIndicator[paytablePageCounter].SetActive(true);
    }

    private void InitialisePayTable()
    {
        WildText.text = (socketManager.initUIData.paylines.symbols[1].payout * socketManager.initialData.bets[betCounter]).ToString();
        TripleSevenText.text = (socketManager.initUIData.paylines.symbols[2].payout * socketManager.initialData.bets[betCounter]).ToString();
        DoubleSevenText.text = (socketManager.initUIData.paylines.symbols[3].payout * socketManager.initialData.bets[betCounter]).ToString();
        SevenText.text = (socketManager.initUIData.paylines.symbols[4].payout * socketManager.initialData.bets[betCounter]).ToString();
        TwoBarText.text = (socketManager.initUIData.paylines.symbols[5].payout * socketManager.initialData.bets[betCounter]).ToString();
        SevenMixedText.text = (socketManager.features.anyPayouts.sevens * socketManager.initialData.bets[betCounter]).ToString();
        BarText.text = (socketManager.initUIData.paylines.symbols[6].payout * socketManager.initialData.bets[betCounter]).ToString();
        BarMixedText.text = (socketManager.features.anyPayouts.bars * socketManager.initialData.bets[betCounter]).ToString();
    }

    #endregion

    #region Helper Function

    internal void InitialiseUIData(Root root)
    {
        InitialisePayTable();
        bonusManager.IntializeBonusWheelValue();
        UpdateBalance(root.player.balance);
        currentBalance = root.player.balance;
        UpdateWin(0.00);
        if (Bet_Text) Bet_Text.text = (root.gameData.bets[betCounter] * socketManager.initialData.lines.Count).ToString();
        currentTotalBet = root.gameData.bets[betCounter] * root.gameData.lines.Count;
        StartBGAnimation();
    }

    internal void SetBetButtonsInteractable(bool interactable)
    {
        if (BetPlus_Button) BetPlus_Button.interactable = interactable;
        if (BetMinus_Button) BetMinus_Button.interactable = interactable;
    }

    private void AutoSpinButtonAnimation(bool animate)
    {
        //var arrowObject = AutoSpinButton.transform.GetChild(0).gameObject;
        if (animate)
        {
            //arrowObject.transform.DORotate(new Vector3(0, 0, -360), 3f, RotateMode.LocalAxisAdd)
                       //.SetEase(Ease.Linear)
                       //.SetLoops(-1);
        }
        else
        {
            //arrowObject.transform.DOKill();
        }
    }

    internal void StartBGAnimation()
    {
        if (bgAnimationRoutine != null)
            StopCoroutine(bgAnimationRoutine);

        bgAnimationRoutine = StartCoroutine(BGAnimationRoutine());
    }

    internal void StopBGAnimation()
    {
        if (bgAnimationRoutine != null)
        {
            StopCoroutine(bgAnimationRoutine);
            bgAnimationRoutine = null;
        }
    }

    private IEnumerator BGAnimationRoutine()
    {
        WaitForSeconds spawnInterval = new WaitForSeconds(0.1f);

        while (socketManager != null && socketManager.isConnected)
        {
            SpawnShineParticle();
            yield return spawnInterval;
        }
    }

    private void SpawnShineParticle()
    {
        // //if (shinePrefab == null || parentRect == null) return;

        // float x = Random.Range(-960f, 960f);
        // float y = Random.Range(-540f, 540f);

        // //GameObject shine = Instantiate(shinePrefab, parentRect);
        // //shine.transform.localPosition = new Vector2(x, y);
        // //shine.transform.localScale = Vector3.zero;

        // //if (shine.TryGetComponent(out CanvasGroup cg))
        // {
        //     cg.alpha = 0f;

        //     Sequence shineSeq = DOTween.Sequence();

        //     // Fade In & Scale Up
        //     shineSeq.Append(shine.transform.DOScale(1f, 1f).SetEase(Ease.Linear));
        //     shineSeq.Join(cg.DOFade(1f, 1f).SetEase(Ease.Linear));

        //     // Fade Out & Scale Down
        //     shineSeq.Append(shine.transform.DOScale(0f, 1f).SetEase(Ease.Linear));
        //     shineSeq.Join(cg.DOFade(0f, 1f).SetEase(Ease.Linear));

        //     // Cleanup safely
        //     shineSeq.OnComplete(() =>
        //     {
        //         Destroy(shine);
        //     });
        // }
        // else
        // {
        //     Destroy(shine);
        // }
    }

    #endregion

    #region Sprite Fonts Functions

    internal static string ToSpriteString(string value)
    {
        var sb = new StringBuilder();
        foreach (char c in value)
        {
            if (c >= '0' && c <= '9')
                sb.Append($"<sprite index={(c - '0')}>");
            else if (c == '.')
                sb.Append("<sprite index=10>");
            else if (c == ',')
                sb.Append("<sprite index=11>");
            else if (c == '+')
                sb.Append("<sprite index=12>");
        }
        return sb.ToString();
    }

    internal static string ToSpriteString(double value, string format = "F2")
        => ToSpriteString(value.ToString(format));

    internal static string ToSpriteString(int value)
        => ToSpriteString(value.ToString());

    internal static double FromSpriteString(string spriteText)
    {
        if (string.IsNullOrEmpty(spriteText)) return 0;
        var sb = new StringBuilder();
        int i = 0;
        while (i < spriteText.Length)
        {
            if (spriteText[i] == '<')
            {
                int end = spriteText.IndexOf('>', i);
                if (end < 0) break;
                string tag = spriteText.Substring(i, end - i + 1);
                const string prefix = "<sprite index=";
                if (tag.StartsWith(prefix))
                {
                    string numStr = tag.Substring(prefix.Length, tag.Length - prefix.Length - 1);
                    if (int.TryParse(numStr, out int idx))
                    {
                        if (idx >= 0 && idx <= 9) sb.Append((char)('0' + idx));
                        else if (idx == 10) sb.Append('.');
                        else if (idx == 11) sb.Append(',');
                        else if (idx == 12) sb.Append('+');
                    }
                }
                i = end + 1;
            }
            else
            {
                sb.Append(spriteText[i]);
                i++;
            }
        }
        string plain = sb.ToString().Replace(",", "");
        return double.TryParse(plain, System.Globalization.NumberStyles.Any,
                               System.Globalization.CultureInfo.InvariantCulture, out double result)
               ? result : 0;
    }

    #endregion
}