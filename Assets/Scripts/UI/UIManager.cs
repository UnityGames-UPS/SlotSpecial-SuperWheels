using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Collections;
using System.Text;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SlotManager slotManager;
    [SerializeField] private BonusManager bonusManager;
    [SerializeField] private SocketIOManager socketManager;
    [SerializeField] private AudioController audioController;
    [SerializeField] private JSFunctCalls jsFunctCalls;
    [SerializeField] private SymbolInfoCard symbolInfoCard;

    [Header("Loading & Intro")]
    [SerializeField] private GameObject gameScreen;
    [SerializeField] private GameObject gameLogoObject;

    [Header("Bet Controls")]
    [SerializeField] private TMP_Text betAmountText;
    [SerializeField] private Button betPlusButton;
    [SerializeField] private Button betMinusButton;
    [Header("Bet Controls - Portrait")]
    [SerializeField] private TMP_Text betAmountTextPortrait;
    [SerializeField] private Button betPlusButtonPortrait;
    [SerializeField] private Button betMinusButtonPortrait;

    [Header("Balance & Win")]
    [SerializeField] private TMP_Text balanceText;
    [SerializeField] private TMP_Text winAmountText;
    [SerializeField] private GameObject winTextObject;
    [SerializeField] private GameObject goodLuckObject;
    [Header("Balance & Win - Portrait")]
    [SerializeField] private TMP_Text balanceTextPortrait;
    [SerializeField] private TMP_Text winAmountTextPortrait;
    [SerializeField] private GameObject winTextObjectPortrait;
    [SerializeField] private GameObject goodLuckObjectPortrait;

    [Header("Bonus Background")]
    [SerializeField] private Image bgImage;
    [SerializeField] private Sprite blueSprite;
    [SerializeField] private Sprite redSprite;

    [Header("Background Shine Particles")]
    [SerializeField] private GameObject shinePrefab;
    [SerializeField] private RectTransform shineParentRect;

    [Header("Main Popup Backdrop")]
    [SerializeField] private GameObject mainPopupObject;
    [Header("Main Popup Backdrop - Portrait")]
    [SerializeField] private GameObject mainPopupObjectPortrait;

    [Header("Universal Win Popup")]
    [SerializeField] private GameObject universalWinPopup;
    [SerializeField] private RectTransform universalWinPopupRect;
    [SerializeField] private GameObject uwpCongratulationsTitle;
    [SerializeField] private GameObject uwpYouWonSubtitle;
    [SerializeField] private GameObject uwpBigWinTitle;
    [SerializeField] private TMP_Text uwpWinAmountText;

    [SerializeField] private ImageAnimation uwpSparkleAnimation;
    [SerializeField] private GameObject MainFreeSpinObject;
    [SerializeField] private GameObject uwpFreeSpinTitle;
    [SerializeField] private TMP_Text uwpFreeSpinCountText;
    [SerializeField] private GameObject uwpFreeSpinTextObject;

    [SerializeField] private Button uwpTakeButton;
    [SerializeField] private float uwpAutoCloseDelay = 5f;
    [Header("Universal Win Popup - Portrait")]
    [SerializeField] private GameObject universalWinPopupPortrait;
    [SerializeField] private RectTransform universalWinPopupRectPortrait;
    [SerializeField] private GameObject uwpCongratulationsTitlePortrait;
    [SerializeField] private GameObject uwpYouWonSubtitlePortrait;
    [SerializeField] private GameObject uwpBigWinTitlePortrait;
    [SerializeField] private TMP_Text uwpWinAmountTextPortrait;
    [SerializeField] private TMP_Text uwpFreeSpinCountTextPortrait;
    [SerializeField] private GameObject uwpFreeSpinObjectPortrait;
    [SerializeField] private Button uwpTakeButtonPortrait;

    [Header("Spin Button")]
    [SerializeField] internal Button spinButton;
    [SerializeField] internal Button stopButton;
    [Header("Spin Button - Portrait")]
    [SerializeField] internal Button spinButtonPortrait;
    [SerializeField] internal Button stopButtonPortrait;

    [Header("Auto Spin Toggle")]
    [SerializeField] internal Button autoSpinButton;
    [Header("Auto Spin Toggle - Portrait")]
    [SerializeField] internal Button autoSpinButtonPortrait;

    [Header("Auto Play Stop Control")]
    [SerializeField] private Button autoSpinStopButton;
    [SerializeField] private TMP_Text autoSpinRemainingText;
    [Header("Auto Play Stop Control - Portrait")]
    [SerializeField] private Button autoSpinStopButtonPortrait;
    [SerializeField] private TMP_Text autoSpinRemainingTextPortrait;

    [Header("Auto Play Panel")]
    [SerializeField] private GameObject autoPlayPanel;
    [SerializeField] private RectTransform autoPlayPanelRect;
    [SerializeField] private Button autoPlayCloseButton;
    [Header("Auto Play Selection Buttons")]
    [SerializeField] private Button autoPlay10Button;
    [SerializeField] private Button autoPlay50Button;
    [SerializeField] private Button autoPlay100Button;
    [SerializeField] private Button autoPlay200Button;
    [SerializeField] private Button autoPlay500Button;
    [SerializeField] private Button autoPlayInfiniteButton;
    [Header("Auto Play Panel - Portrait")]
    [SerializeField] private GameObject autoPlayPanelPortrait;
    [SerializeField] private RectTransform autoPlayPanelRectPortrait;
    [SerializeField] private Button autoPlayCloseButtonPortrait;
    [SerializeField] private Button autoPlay10ButtonPortrait;
    [SerializeField] private Button autoPlay50ButtonPortrait;
    [SerializeField] private Button autoPlay100ButtonPortrait;
    [SerializeField] private Button autoPlay200ButtonPortrait;
    [SerializeField] private Button autoPlay500ButtonPortrait;
    [SerializeField] private Button autoPlayInfiniteButtonPortrait;

    [Header("Settings Panel")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private RectTransform settingsPanelRect;
    [SerializeField] private Button settingsOpenButton;
    [SerializeField] private Button settingsCloseButton;
    [SerializeField] private Button settingsBgCloseButton;
    [SerializeField] private Button gameQuitButton;
    [Header("Settings Panel - Portrait")]
    [SerializeField] private GameObject settingsPanelPortrait;
    [SerializeField] private RectTransform settingsPanelRectPortrait;
    [SerializeField] private Button settingsOpenButtonPortrait;
    [SerializeField] private Button settingsCloseButtonPortrait;
    [SerializeField] private Button settingsBgCloseButtonPortrait;
    [SerializeField] private Button gameQuitButtonPortrait;

    [Header("Speed Buttons (Three-Layer Toggle)")]
    [SerializeField] private Button normalSpeedButton;
    [SerializeField] private Button turboSpeedButton;
    [SerializeField] private Button quickSpeedButton;
    [Header("Speed Buttons - Portrait")]
    [SerializeField] private Button normalSpeedButtonPortrait;
    [SerializeField] private Button turboSpeedButtonPortrait;
    [SerializeField] private Button quickSpeedButtonPortrait;

    [Header("Sound Panel")]
    [SerializeField] private GameObject soundPanel;
    [SerializeField] private RectTransform soundPanelRect;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Button soundPanelCloseButton;
    [SerializeField] private Button soundPanelOpenButton;
    [SerializeField] private Button soundPanelOpenButtonPortrait;

    [Header("Game Rules Panel")]
    [SerializeField] private GameObject gameRulesPanel;
    [SerializeField] private RectTransform gameRulesPanelRect;
    [SerializeField] private Button gameRulesOpenButton;
    [SerializeField] private Button gameRulesBackButton;
    [Header("Game Rules Panel - Portrait")]
    [SerializeField] private Button gameRulesOpenButtonPortrait;

    [Header("Guide Panel")]
    [SerializeField] private GameObject guidePanel;
    [SerializeField] private RectTransform guidePanelRect;
    [SerializeField] private Button guideOpenButton;
    [SerializeField] private Button guideBackButton;
    [Header("Guide Panel - Portrait")]
    [SerializeField] private Button guideOpenButtonPortrait;

    [Header("Game Rules Dynamic Texts")]
    [SerializeField] private TMP_Text totalLineCountText;
    [SerializeField] private TMP_Text ruleSymbol0Text;
    [SerializeField] private TMP_Text ruleSymbol1Text;
    [SerializeField] private TMP_Text ruleSymbol2Text;
    [SerializeField] private TMP_Text ruleSymbol3Text;
    [SerializeField] private TMP_Text ruleSymbol4Text;
    [SerializeField] private TMP_Text ruleSymbol5Text;
    [SerializeField] private TMP_Text ruleSymbol6Text;
    [SerializeField] private TMP_Text ruleSymbol7Text;
    [SerializeField] private TMP_Text ruleSymbol8Text;
    [SerializeField] private TMP_Text ruleSymbol9Text;

    [Header("Free Spin Count Display - Game Screen")]
    [SerializeField] private GameObject freeSpinCountContainer;
    [SerializeField] private TMP_Text remainingFreeSpinsText;
    [SerializeField] private TMP_Text FreeSpinTotalWinText;
    [Header("Free Spin Transition")]
    [SerializeField] private CanvasGroup transitionBackFilm;

    [Header("Ping Display")]
    [SerializeField] private TMP_Text pingText;
    [SerializeField] private TMP_Text pingTextPortrait;

    [Header("Platform Jackpot")]
    [SerializeField] private TMP_Text grandJackpotText;
    [SerializeField] private TMP_Text majorJackpotText;
    [SerializeField] private TMP_Text minorJackpotText;
    [SerializeField] private TMP_Text miniJackpotText;
    [Header("Platform Jackpot - Portrait")]
    [SerializeField] private TMP_Text grandJackpotTextPortrait;
    [SerializeField] private TMP_Text majorJackpotTextPortrait;
    [SerializeField] private TMP_Text minorJackpotTextPortrait;
    [SerializeField] private TMP_Text miniJackpotTextPortrait;

    [Header("Expand-Shrink Controls")]
    [SerializeField] private Button expandButton;
    [SerializeField] private Button shrinkButton;
    [Header("Expand-Shrink Controls - Portrait")]
    [SerializeField] private Button expandButtonPortrait;
    [SerializeField] private Button shrinkButtonPortrait;

    [Header("Disconnection Popup")]
    [SerializeField] private Button closeDisconnectButton;
    [SerializeField] private GameObject disconnectPopup;
    [Header("Disconnection Popup - Portrait")]
    [SerializeField] private Button closeDisconnectButtonPortrait;
    [SerializeField] private GameObject disconnectPopupPortrait;

    [Header("Reconnection Popup")]
    [SerializeField] private GameObject reconnectPopup;
    [Header("Reconnection Popup - Portrait")]
    [SerializeField] private GameObject reconnectPopupPortrait;

    [Header("Another Device Popup")]
    [SerializeField] private Button closeADButton;
    [SerializeField] private GameObject adPopup;
    [Header("Another Device Popup - Portrait")]
    [SerializeField] private Button closeADButtonPortrait;
    [SerializeField] private GameObject adPopupPortrait;

    [Header("Low Balance Popup")]
    [SerializeField] private Button lowBalanceExitButton;
    [SerializeField] private GameObject lowBalancePopup;
    [Header("Low Balance Popup - Portrait")]
    [SerializeField] private Button lowBalanceExitButtonPortrait;
    [SerializeField] private GameObject lowBalancePopupPortrait;

    [Header("Quit Popup")]
    [SerializeField] private GameObject quitPopup;
    [SerializeField] private Button yesQuitButton;
    [SerializeField] private Button noQuitButton;
    [SerializeField] private Button crossQuitButton;
    [Header("Quit Popup - Portrait")]
    [SerializeField] private GameObject quitPopupPortrait;
    [SerializeField] private Button yesQuitButtonPortrait;
    [SerializeField] private Button noQuitButtonPortrait;
    [SerializeField] private Button crossQuitButtonPortrait;

    [Header("Rapid Stop Cooldown")]
    [Tooltip("Seconds the player must wait before pressing Stop again after an immediate stop.")]
    [SerializeField] private float rapidStopCooldown = 1f;
    private float lastRapidStopTime = -99f;

    // Cross-file API surface: SlotManager/BonusManager/SocketManager/SymbolClickHandler
    // call directly into these exact field/method names — kept stable even though the
    // reference project routes the equivalent state through GameManager/PopupManager.
    internal double currentBalance = 0;
    internal double currentTotalBet = 0;
    internal int betCounter = 0;

    private bool isExit = false;
    private bool isExpanded = false;

    private Tween balanceTween;
    private Tween winTween;
    private int totalFreeSpinsAwarded = 0;

    private Coroutine bgAnimationRoutine;

    private System.Action universalWinPopupCallback;
    private Coroutine uwpAutoCloseCoroutine;
    private Tween uwpWinTween;

    private void Awake()
    {
        if (jsFunctCalls != null)
        {
            jsFunctCalls.RegisterVisibilityListener(gameObject.name);
            jsFunctCalls.RegisterFullscreenListener(gameObject.name);
        }
    }

    public void OnFocusChanged(string value)
    {
        bool focused = value == "1";
        Debug.Log("UNITY FOCUS CHANGED: " + value + " (focused: " + focused + ")");
        if (audioController != null) audioController.SetMuteAll(!focused);
        if (socketManager != null) socketManager.HandleFocusChange(focused);
    }

    internal void OnFullscreenChanged(string isFullscreen)
    {
        bool newExpandedState = isFullscreen == "1";
        if (isExpanded != newExpandedState)
        {
            isExpanded = newExpandedState;
            SetExpandShrinkButtons(isExpanded);
        }
    }

    private void Start()
    {
        SetupButtons();
        SetupAutoPlayPanel();
        SetupSettingsPanel();
        SetupGameRulesPanel();
        SetupGuidePanel();

        InitializeExpandShrink();
        UpdateSpeedButtonsVisibility(SlotManager.SpinSpeed.Normal);

        if (gameScreen) gameScreen.SetActive(true);
        InitializeUI();
    }

    private void InitializeUI()
    {
        SetGameObjectActive(autoPlayPanel, autoPlayPanelPortrait, false);
        if (autoPlayPanelRect) autoPlayPanelRect.anchoredPosition = new Vector2(autoPlayPanelRect.anchoredPosition.x, -600f);
        if (autoPlayPanelRectPortrait) autoPlayPanelRectPortrait.anchoredPosition = new Vector2(autoPlayPanelRectPortrait.anchoredPosition.x, -600f);
        SetButtonActive(autoSpinStopButton, autoSpinStopButtonPortrait, false);

        SetGameObjectActive(settingsPanel, settingsPanelPortrait, false);
        SetGameObjectActive(soundPanel, null, false);
        SetGameObjectActive(gameRulesPanel, null, false);
        SetGameObjectActive(guidePanel, null, false);

        if (uwpWinTween != null) { uwpWinTween.Kill(); uwpWinTween = null; }
        SetGameObjectActive(universalWinPopup, universalWinPopupPortrait, false);

        if (freeSpinCountContainer) freeSpinCountContainer.SetActive(false);
        if (transitionBackFilm) transitionBackFilm.gameObject.SetActive(false);

        UpdatePingDisplay("-- ms");
    }

    #region UI Synchronization Helpers

    private void SetGameObjectActive(GameObject obj1, GameObject obj2, bool active)
    {
        if (obj1) obj1.SetActive(active);
        if (obj2) obj2.SetActive(active);
    }

    private void SetButtonActive(Button btn1, Button btn2, bool active)
    {
        if (btn1) btn1.gameObject.SetActive(active);
        if (btn2) btn2.gameObject.SetActive(active);
    }

    private void SetButtonInteractable(Button btn1, Button btn2, bool interactable)
    {
        if (btn1) btn1.interactable = interactable;
        if (btn2) btn2.interactable = interactable;
    }

    private void SetTMPText(TMP_Text text1, TMP_Text text2, string content)
    {
        if (text1) text1.text = content;
        if (text2) text2.text = content;
    }

    private bool IsActivePair(GameObject a, GameObject b)
    {
        return (a != null && a.activeSelf) || (b != null && b.activeSelf);
    }

    #endregion

    #region Button Setup

    private void SetupButtons()
    {
        WireSpinButton(spinButton);
        WireSpinButton(spinButtonPortrait);

        if (stopButton) stopButton.onClick.AddListener(OnStopButtonPressed);
        if (stopButtonPortrait) stopButtonPortrait.onClick.AddListener(OnStopButtonPressed);

        if (autoSpinButton) autoSpinButton.onClick.AddListener(OnAutoSpinButtonPressed);
        if (autoSpinButtonPortrait) autoSpinButtonPortrait.onClick.AddListener(OnAutoSpinButtonPressed);

        if (autoSpinStopButton) autoSpinStopButton.onClick.AddListener(OnAutoSpinButtonPressed);
        if (autoSpinStopButtonPortrait) autoSpinStopButtonPortrait.onClick.AddListener(OnAutoSpinButtonPressed);

        if (betPlusButton) betPlusButton.onClick.AddListener(() => ChangeBet(true));
        if (betPlusButtonPortrait) betPlusButtonPortrait.onClick.AddListener(() => ChangeBet(true));
        if (betMinusButton) betMinusButton.onClick.AddListener(() => ChangeBet(false));
        if (betMinusButtonPortrait) betMinusButtonPortrait.onClick.AddListener(() => ChangeBet(false));

        if (gameQuitButton) gameQuitButton.onClick.AddListener(() => { audioController.PlayUIButton(false); OpenPopup(quitPopup, quitPopupPortrait); });
        if (gameQuitButtonPortrait) gameQuitButtonPortrait.onClick.AddListener(() => { audioController.PlayUIButton(false); OpenPopup(quitPopup, quitPopupPortrait); });

        if (yesQuitButton) yesQuitButton.onClick.AddListener(CallOnExitFunction);
        if (yesQuitButtonPortrait) yesQuitButtonPortrait.onClick.AddListener(CallOnExitFunction);
        if (noQuitButton) noQuitButton.onClick.AddListener(() => { if (!isExit) ClosePopup(quitPopup, quitPopupPortrait); });
        if (noQuitButtonPortrait) noQuitButtonPortrait.onClick.AddListener(() => { if (!isExit) ClosePopup(quitPopup, quitPopupPortrait); });
        if (crossQuitButton) crossQuitButton.onClick.AddListener(() => { if (!isExit) ClosePopup(quitPopup, quitPopupPortrait); });
        if (crossQuitButtonPortrait) crossQuitButtonPortrait.onClick.AddListener(() => { if (!isExit) ClosePopup(quitPopup, quitPopupPortrait); });

        if (closeDisconnectButton) closeDisconnectButton.onClick.AddListener(CallOnExitFunction);
        if (closeDisconnectButtonPortrait) closeDisconnectButtonPortrait.onClick.AddListener(CallOnExitFunction);
        if (closeADButton) closeADButton.onClick.AddListener(CallOnExitFunction);
        if (closeADButtonPortrait) closeADButtonPortrait.onClick.AddListener(CallOnExitFunction);
        if (lowBalanceExitButton) lowBalanceExitButton.onClick.AddListener(() => ClosePopup(lowBalancePopup, lowBalancePopupPortrait));
        if (lowBalanceExitButtonPortrait) lowBalanceExitButtonPortrait.onClick.AddListener(() => ClosePopup(lowBalancePopup, lowBalancePopupPortrait));

        if (expandButton) expandButton.onClick.AddListener(() => { audioController.PlayUIButton(false); OnExpand(); });
        if (shrinkButton) shrinkButton.onClick.AddListener(() => { audioController.PlayUIButton(false); OnShrink(); });
        if (expandButtonPortrait) expandButtonPortrait.onClick.AddListener(() => { audioController.PlayUIButton(false); OnExpand(); });
        if (shrinkButtonPortrait) shrinkButtonPortrait.onClick.AddListener(() => { audioController.PlayUIButton(false); OnShrink(); });

        if (uwpTakeButton) uwpTakeButton.onClick.AddListener(OnUniversalWinTakeButtonClicked);
        if (uwpTakeButtonPortrait) uwpTakeButtonPortrait.onClick.AddListener(OnUniversalWinTakeButtonClicked);

        if (normalSpeedButton) normalSpeedButton.onClick.AddListener(() => { audioController.PlayUIButton(false); SetSpeedMode(SlotManager.SpinSpeed.Turbo); });
        if (turboSpeedButton) turboSpeedButton.onClick.AddListener(() => { audioController.PlayUIButton(false); SetSpeedMode(SlotManager.SpinSpeed.QuickSpin); });
        if (quickSpeedButton) quickSpeedButton.onClick.AddListener(() => { audioController.PlayUIButton(false); SetSpeedMode(SlotManager.SpinSpeed.Normal); });
        if (normalSpeedButtonPortrait) normalSpeedButtonPortrait.onClick.AddListener(() => { audioController.PlayUIButton(false); SetSpeedMode(SlotManager.SpinSpeed.Turbo); });
        if (turboSpeedButtonPortrait) turboSpeedButtonPortrait.onClick.AddListener(() => { audioController.PlayUIButton(false); SetSpeedMode(SlotManager.SpinSpeed.QuickSpin); });
        if (quickSpeedButtonPortrait) quickSpeedButtonPortrait.onClick.AddListener(() => { audioController.PlayUIButton(false); SetSpeedMode(SlotManager.SpinSpeed.Normal); });
    }

    private void WireSpinButton(Button button)
    {
        if (!button) return;

        var holdHandler = button.GetComponent<SpinButtonHoldHandler>();
        if (holdHandler != null)
        {
            holdHandler.OnClick.AddListener(OnSpinButtonPressed);
            holdHandler.OnHoldThreeSeconds.AddListener(OnSpinButtonHeld);
        }
        else
        {
            button.onClick.AddListener(OnSpinButtonPressed);
        }
    }

    private void SetupAutoPlayPanel()
    {
        if (autoPlay10Button) autoPlay10Button.onClick.AddListener(() => { audioController.PlayUIButton(false); StartAutoplayWithRounds(10); });
        if (autoPlay50Button) autoPlay50Button.onClick.AddListener(() => { audioController.PlayUIButton(false); StartAutoplayWithRounds(50); });
        if (autoPlay100Button) autoPlay100Button.onClick.AddListener(() => { audioController.PlayUIButton(false); StartAutoplayWithRounds(100); });
        if (autoPlay200Button) autoPlay200Button.onClick.AddListener(() => { audioController.PlayUIButton(false); StartAutoplayWithRounds(200); });
        if (autoPlay500Button) autoPlay500Button.onClick.AddListener(() => { audioController.PlayUIButton(false); StartAutoplayWithRounds(500); });
        if (autoPlayInfiniteButton) autoPlayInfiniteButton.onClick.AddListener(() => { audioController.PlayUIButton(false); StartAutoplayWithRounds(-1); });

        if (autoPlay10ButtonPortrait) autoPlay10ButtonPortrait.onClick.AddListener(() => { audioController.PlayUIButton(false); StartAutoplayWithRounds(10); });
        if (autoPlay50ButtonPortrait) autoPlay50ButtonPortrait.onClick.AddListener(() => { audioController.PlayUIButton(false); StartAutoplayWithRounds(50); });
        if (autoPlay100ButtonPortrait) autoPlay100ButtonPortrait.onClick.AddListener(() => { audioController.PlayUIButton(false); StartAutoplayWithRounds(100); });
        if (autoPlay200ButtonPortrait) autoPlay200ButtonPortrait.onClick.AddListener(() => { audioController.PlayUIButton(false); StartAutoplayWithRounds(200); });
        if (autoPlay500ButtonPortrait) autoPlay500ButtonPortrait.onClick.AddListener(() => { audioController.PlayUIButton(false); StartAutoplayWithRounds(500); });
        if (autoPlayInfiniteButtonPortrait) autoPlayInfiniteButtonPortrait.onClick.AddListener(() => { audioController.PlayUIButton(false); StartAutoplayWithRounds(-1); });

        if (autoPlayCloseButton) autoPlayCloseButton.onClick.AddListener(CloseAutoPlayPanel);
        if (autoPlayCloseButtonPortrait) autoPlayCloseButtonPortrait.onClick.AddListener(CloseAutoPlayPanel);
    }

    private void SetupSettingsPanel()
    {
        if (settingsOpenButton) settingsOpenButton.onClick.AddListener(() => { audioController.PlayUIButton(false); OpenSettingsPanel(); });
        if (settingsOpenButtonPortrait) settingsOpenButtonPortrait.onClick.AddListener(() => { audioController.PlayUIButton(false); OpenSettingsPanel(); });

        if (settingsCloseButton) settingsCloseButton.onClick.AddListener(() => { audioController.PlayUIButton(false); CloseSettingsPanel(); });
        if (settingsCloseButtonPortrait) settingsCloseButtonPortrait.onClick.AddListener(() => { audioController.PlayUIButton(false); CloseSettingsPanel(); });
        if (settingsBgCloseButton) settingsBgCloseButton.onClick.AddListener(() => { audioController.PlayUIButton(false); CloseSettingsPanel(); });
        if (settingsBgCloseButtonPortrait) settingsBgCloseButtonPortrait.onClick.AddListener(() => { audioController.PlayUIButton(false); CloseSettingsPanel(); });

        SetButtonActive(settingsOpenButton, settingsOpenButtonPortrait, true);
        SetButtonActive(settingsCloseButton, settingsCloseButtonPortrait, false);
        SetButtonActive(settingsBgCloseButton, settingsBgCloseButtonPortrait, false);

        if (soundPanelOpenButton) soundPanelOpenButton.onClick.AddListener(OpenSoundPanel);
        if (soundPanelOpenButtonPortrait) soundPanelOpenButtonPortrait.onClick.AddListener(OpenSoundPanel);
        if (soundPanelCloseButton) soundPanelCloseButton.onClick.AddListener(CloseSoundPanel);

        if (musicSlider)
        {
            musicSlider.value = audioController.MusicVolume;
            musicSlider.onValueChanged.AddListener(audioController.SetMusicVolume);
        }
        if (sfxSlider)
        {
            sfxSlider.value = audioController.SfxVolume;
            sfxSlider.onValueChanged.AddListener(audioController.SetSfxVolume);
        }
    }

    private void SetupGameRulesPanel()
    {
        if (gameRulesOpenButton) gameRulesOpenButton.onClick.AddListener(OpenGameRulesPanel);
        if (gameRulesOpenButtonPortrait) gameRulesOpenButtonPortrait.onClick.AddListener(OpenGameRulesPanel);
        if (gameRulesBackButton) gameRulesBackButton.onClick.AddListener(() => { audioController.PlayUIButton(false); CloseGameRulesPanel(); });
    }

    private void SetupGuidePanel()
    {
        if (guideOpenButton) guideOpenButton.onClick.AddListener(OpenGuidePanel);
        if (guideOpenButtonPortrait) guideOpenButtonPortrait.onClick.AddListener(OpenGuidePanel);
        if (guideBackButton) guideBackButton.onClick.AddListener(() => { audioController.PlayUIButton(false); CloseGuidePanel(); });
    }

    #endregion

    #region Spin Button

    public void OnSpinButtonPressed()
    {
        if (slotManager._isAutoSpin)
        {
            audioController.PlayUIButton(false);
            SetButtonInteractable(autoSpinButton, autoSpinButtonPortrait, false);
            slotManager.StopAutoSpin();
            AutoSpinButtonAnimation(false);
            return;
        }

        // if (!bonusManager.isBonusFinished)
        // {
        //     bonusManager.wheelButtonPressed = true;
        //     SetButtonInteractable(spinButton, spinButtonPortrait, false);
        //     return;
        // }

        audioController.PlayUIButton(false);
        SetButtonInteractable(spinButton, spinButtonPortrait, false);
        SetBetControlsEnabled(false);

        slotManager.StartSlots();
        SetButtonActive(stopButton, stopButtonPortrait, true);
        SetButtonActive(spinButton, spinButtonPortrait, false);
    }

    public void OnSpinButtonHeld()
    {
        if (!slotManager._isAutoSpin)
        {
            audioController.PlayUIButton(false);
            OpenAutoPlayPanel();
        }
    }

    private void OnStopButtonPressed()
    {
        if (Time.unscaledTime - lastRapidStopTime < rapidStopCooldown) return;
        lastRapidStopTime = Time.unscaledTime;

        audioController.PlayUIButton(false);
        slotManager.RequestInstantStop();

        if (slotManager._isAutoSpin)
        {
            slotManager.StopAutoSpin();
            AutoSpinButtonAnimation(false);
            SetButtonInteractable(autoSpinButton, autoSpinButtonPortrait, false);
        }

        SetButtonInteractable(stopButton, stopButtonPortrait, false);
        SetButtonActive(stopButton, stopButtonPortrait, true);
        SetButtonActive(spinButton, spinButtonPortrait, false);
    }

    internal void OnStopSpinButtonTrigger()
    {
        OnStopButtonPressed();
    }

    internal void SetSpinButtonReady()
    {
        SetButtonInteractable(stopButton, stopButtonPortrait, true);

        if (slotManager._isAutoSpin)
        {
            SetButtonActive(stopButton, stopButtonPortrait, true);
            SetButtonActive(spinButton, spinButtonPortrait, false);
        }
        else
        {
            SetButtonInteractable(spinButton, spinButtonPortrait, true);
            SetButtonActive(spinButton, spinButtonPortrait, true);
            SetButtonActive(stopButton, stopButtonPortrait, false);
            SetBetControlsEnabled(true);
            SetButtonInteractable(autoSpinButton, autoSpinButtonPortrait, true);
        }

        SetButtonActive(autoSpinStopButton, autoSpinStopButtonPortrait, slotManager._isAutoSpin);
    }

    private void OnAutoSpinButtonPressed()
    {
        audioController.PlayUIButton(false);
        if (!bonusManager.isBonusFinished)
        {
            ToggleAutoSpin();
        }
        else
        {
            ToggleAutoSpin();
            if (slotManager._isAutoSpin)
            {
                SetButtonActive(stopButton, stopButtonPortrait, true);
                SetButtonActive(spinButton, spinButtonPortrait, false);
            }
        }

        SetButtonActive(autoSpinStopButton, autoSpinStopButtonPortrait, slotManager._isAutoSpin);
        UpdateAutoPlayCount();
    }

    private void ToggleAutoSpin()
    {
        if (!slotManager._isAutoSpin)
        {
            slotManager.AutoSpin();
            SetBetControlsEnabled(false);
            AutoSpinButtonAnimation(true);
        }
        else
        {
            SetButtonInteractable(autoSpinButton, autoSpinButtonPortrait, false);
            slotManager.StopAutoSpin();
            SetButtonInteractable(stopButton, stopButtonPortrait, false);
            SetButtonActive(stopButton, stopButtonPortrait, true);
            SetButtonActive(spinButton, spinButtonPortrait, false);
            AutoSpinButtonAnimation(false);
        }
    }

    internal void UpdateAutoPlayCount()
    {
        string displayStr = slotManager.autoSpinRoundsRemaining < 0 ? "∞" : slotManager.autoSpinRoundsRemaining.ToString();
        SetTMPText(autoSpinRemainingText, autoSpinRemainingTextPortrait, displayStr);
    }

    private void AutoSpinButtonAnimation(bool animate)
    {
        AnimateAutoSpinArrow(autoSpinButton, animate);
        AnimateAutoSpinArrow(autoSpinButtonPortrait, animate);
    }

    private void AnimateAutoSpinArrow(Button button, bool animate)
    {
        if (!button || button.transform.childCount == 0) return;
        var arrowObject = button.transform.GetChild(0).gameObject;
        if (animate)
        {
            arrowObject.transform.DORotate(new Vector3(0, 0, -360), 3f, RotateMode.LocalAxisAdd)
                       .SetEase(Ease.Linear)
                       .SetLoops(-1);
        }
        else
        {
            arrowObject.transform.DOKill();
        }
    }

    #endregion

    #region Bet Controls

    internal void UpdateBetDisplay()
    {
        if (socketManager.initialData == null) return;

        double totalPay = socketManager.initialData.bets[betCounter] * socketManager.initialData.lines.Count;
        currentTotalBet = totalPay;

        SetTMPText(betAmountText, betAmountTextPortrait, FormatAmount(totalPay));
        UpdateGameRulesDynamicTexts();
    }

    private void ChangeBet(bool increase)
    {
        audioController.PlayUIButton(false);
        if (increase)
        {
            betCounter++;
            if (betCounter >= socketManager.initialData.bets.Count) betCounter = 0;
        }
        else
        {
            betCounter--;
            if (betCounter < 0) betCounter = socketManager.initialData.bets.Count - 1;
        }
        UpdateBetDisplay();
    }

    private void SetBetControlsEnabled(bool enabled)
    {
        SetButtonInteractable(betPlusButton, betPlusButtonPortrait, enabled);
        SetButtonInteractable(betMinusButton, betMinusButtonPortrait, enabled);
    }

    #endregion

    #region Auto Play Panel

    private void OpenAutoPlayPanel()
    {
        audioController.PlayUIButton(false);
        if (IsActivePair(settingsPanel, settingsPanelPortrait)) ClosePopup(settingsPanel, settingsPanelPortrait);

        SetGameObjectActive(autoPlayPanel, autoPlayPanelPortrait, true);
        if (autoPlayPanelRect)
        {
            autoPlayPanelRect.anchoredPosition = new Vector2(autoPlayPanelRect.anchoredPosition.x, -600f);
            autoPlayPanelRect.DOAnchorPosY(0f, 0.35f).SetEase(Ease.OutCubic);
        }
        if (autoPlayPanelRectPortrait)
        {
            autoPlayPanelRectPortrait.anchoredPosition = new Vector2(autoPlayPanelRectPortrait.anchoredPosition.x, -600f);
            autoPlayPanelRectPortrait.DOAnchorPosY(0f, 0.35f).SetEase(Ease.OutCubic);
        }
    }

    private void CloseAutoPlayPanel()
    {
        audioController.PlayUIButton(false);

        if (autoPlayPanelRect)
        {
            autoPlayPanelRect.DOAnchorPosY(-600f, 0.35f).SetEase(Ease.InCubic).OnComplete(() =>
            {
                if (autoPlayPanel) autoPlayPanel.SetActive(false);
            });
        }
        else if (autoPlayPanel) autoPlayPanel.SetActive(false);

        if (autoPlayPanelRectPortrait)
        {
            autoPlayPanelRectPortrait.DOAnchorPosY(-600f, 0.35f).SetEase(Ease.InCubic).OnComplete(() =>
            {
                if (autoPlayPanelPortrait) autoPlayPanelPortrait.SetActive(false);
            });
        }
        else if (autoPlayPanelPortrait) autoPlayPanelPortrait.SetActive(false);
    }

    private void StartAutoplayWithRounds(int rounds)
    {
        CloseAutoPlayPanel();
        SetBetControlsEnabled(false);
        AutoSpinButtonAnimation(true);
        slotManager.AutoSpin(rounds);
        SetButtonActive(stopButton, stopButtonPortrait, true);
        SetButtonActive(spinButton, spinButtonPortrait, false);
        SetButtonActive(autoSpinStopButton, autoSpinStopButtonPortrait, true);
        UpdateAutoPlayCount();
    }

    #endregion

    #region Spin Speed

    internal void SetSpeedMode(SlotManager.SpinSpeed speed)
    {
        slotManager.SetSpinSpeed(speed);
        UpdateSpeedButtonsVisibility(speed);
    }

    private void UpdateSpeedButtonsVisibility(SlotManager.SpinSpeed speed)
    {
        SetButtonActive(normalSpeedButton, normalSpeedButtonPortrait, speed == SlotManager.SpinSpeed.Normal);
        SetButtonActive(turboSpeedButton, turboSpeedButtonPortrait, speed == SlotManager.SpinSpeed.Turbo);
        SetButtonActive(quickSpeedButton, quickSpeedButtonPortrait, speed == SlotManager.SpinSpeed.QuickSpin);
    }

    #endregion

    #region Sound Panel

    private void OpenSoundPanel()
    {
        audioController.PlayUIButton(false);
        if (soundPanel == null) return;
        soundPanel.SetActive(true);
        if (soundPanelRect != null) AnimatePopupOpen(soundPanelRect);

        if (musicSlider) musicSlider.value = audioController.MusicVolume;
        if (sfxSlider) sfxSlider.value = audioController.SfxVolume;
    }

    private void CloseSoundPanel()
    {
        if (soundPanel == null || !soundPanel.activeSelf) return;
        if (soundPanelRect != null)
        {
            AnimatePopupClose(soundPanelRect, () => soundPanel.SetActive(false));
        }
        else
        {
            soundPanel.SetActive(false);
        }
    }

    #endregion

    #region Settings Panel

    private void OpenSettingsPanel()
    {
        if (IsActivePair(autoPlayPanel, autoPlayPanelPortrait)) CloseAutoPlayPanelImmediate();

        SetButtonActive(settingsOpenButton, settingsOpenButtonPortrait, false);
        SetButtonActive(settingsCloseButton, settingsCloseButtonPortrait, true);
        SetButtonActive(settingsBgCloseButton, settingsBgCloseButtonPortrait, true);

        FadeInSettingsPanel(settingsPanel);
        FadeInSettingsPanel(settingsPanelPortrait);
    }

    private void FadeInSettingsPanel(GameObject panel)
    {
        if (!panel) return;
        panel.SetActive(true);
        CanvasGroup cg = panel.GetComponent<CanvasGroup>();
        if (cg == null) cg = panel.AddComponent<CanvasGroup>();
        cg.alpha = 0f;
        cg.DOFade(1f, 0.35f);
    }

    private void CloseSettingsPanel()
    {
        SetButtonActive(settingsOpenButton, settingsOpenButtonPortrait, true);
        SetButtonActive(settingsCloseButton, settingsCloseButtonPortrait, false);
        SetButtonActive(settingsBgCloseButton, settingsBgCloseButtonPortrait, false);

        FadeOutSettingsPanel(settingsPanel);
        FadeOutSettingsPanel(settingsPanelPortrait);
    }

    private void FadeOutSettingsPanel(GameObject panel)
    {
        if (!panel) return;
        CanvasGroup cg = panel.GetComponent<CanvasGroup>();
        if (cg == null) cg = panel.AddComponent<CanvasGroup>();
        cg.DOFade(0f, 0.35f).OnComplete(() => panel.SetActive(false));
    }

    private void CloseSettingsPanelImmediate()
    {
        SetButtonActive(settingsOpenButton, settingsOpenButtonPortrait, true);
        SetButtonActive(settingsCloseButton, settingsCloseButtonPortrait, false);
        SetButtonActive(settingsBgCloseButton, settingsBgCloseButtonPortrait, false);

        SnapSettingsPanelClosed(settingsPanel);
        SnapSettingsPanelClosed(settingsPanelPortrait);
    }

    private void SnapSettingsPanelClosed(GameObject panel)
    {
        if (!panel) return;
        CanvasGroup cg = panel.GetComponent<CanvasGroup>();
        if (cg != null) cg.alpha = 0f;
        panel.SetActive(false);
    }

    private void CloseAutoPlayPanelImmediate()
    {
        if (autoPlayPanelRect) autoPlayPanelRect.localScale = Vector3.one;
        if (autoPlayPanelRectPortrait) autoPlayPanelRectPortrait.localScale = Vector3.one;
        SetGameObjectActive(autoPlayPanel, autoPlayPanelPortrait, false);
    }

    #endregion

    #region Game Rules Panel

    private void OpenGameRulesPanel()
    {
        if (IsActivePair(settingsPanel, settingsPanelPortrait)) CloseSettingsPanelImmediate();
        ShowGameRulesPanel();
    }

    private void ShowGameRulesPanel()
    {
        if (gameRulesPanel == null) return;
        audioController.PlayUIButton(false);
        gameRulesPanel.SetActive(true);
        if (gameRulesPanelRect != null) AnimatePopupOpen(gameRulesPanelRect);
    }

    private void CloseGameRulesPanel()
    {
        if (gameRulesPanel == null) return;
        if (gameRulesPanelRect != null)
        {
            AnimatePopupClose(gameRulesPanelRect, () => gameRulesPanel.SetActive(false));
        }
        else
        {
            gameRulesPanel.SetActive(false);
        }
    }

    private void UpdateGameRulesDynamicTexts()
    {
        if (socketManager.initUIData == null || socketManager.initUIData.paylines == null) return;

        if (totalLineCountText != null)
        {
            totalLineCountText.text = socketManager.initialData.lines.Count.ToString();
        }

        TMP_Text[] symbolTexts =
        {
            ruleSymbol0Text, ruleSymbol1Text, ruleSymbol2Text, ruleSymbol3Text,
            ruleSymbol4Text, ruleSymbol5Text, ruleSymbol6Text, ruleSymbol7Text,
            ruleSymbol8Text, ruleSymbol9Text
        };

        var symbols = socketManager.initUIData.paylines.symbols;
        if (symbols == null) return;

        for (int i = 0; i < symbolTexts.Length; i++)
        {
            if (symbolTexts[i] == null) continue;
            var symbol = symbols.Find(s => s.id == i);
            if (symbol == null || symbol.multiplier == null || symbol.multiplier.Count == 0) continue;

            string fullText = "";
            int currentMatch = 5;
            for (int m = 0; m < symbol.multiplier.Count; m++)
            {
                double win = symbol.multiplier[m];
                string line = $"{currentMatch}     {win}";
                fullText = (m == 0) ? line : fullText + $"\n{line}";
                currentMatch--;
            }

            symbolTexts[i].text = fullText;
        }
    }

    #endregion

    #region Guide Panel

    private void OpenGuidePanel()
    {
        if (IsActivePair(settingsPanel, settingsPanelPortrait)) CloseSettingsPanelImmediate();
        ShowGuidePanel();
    }

    private void ShowGuidePanel()
    {
        if (guidePanel == null) return;
        audioController.PlayUIButton(false);
        guidePanel.SetActive(true);
        if (guidePanelRect != null) AnimatePopupOpen(guidePanelRect);
    }

    private void CloseGuidePanel()
    {
        if (guidePanel == null) return;
        if (guidePanelRect != null)
        {
            AnimatePopupClose(guidePanelRect, () => guidePanel.SetActive(false));
        }
        else
        {
            guidePanel.SetActive(false);
        }
    }

    #endregion

    #region Free Spins Flow

    internal void OnFreeSpinsTriggered(int spinsAwarded)
    {
        ShowUniversalWinPopup(WinPopupType.FreeSpinTrigger, 0, spinsAwarded, () => StartFreeSpinsSequence(spinsAwarded));
    }

    private void StartFreeSpinsSequence(int spinsAwarded)
    {
        slotManager.isInFreeSpins = true;
        slotManager.freeSpinsRemaining = spinsAwarded;
        slotManager.freeSpinsUsed = 0;
        totalFreeSpinsAwarded = spinsAwarded;

        if (gameLogoObject) gameLogoObject.SetActive(false);
        UpdateFreeSpinCount(0, spinsAwarded);
        UpdateWinDisplay(0);
    }

    internal void OnFreeSpinsEnded(double serverTotalRoundWin, int serverTotalSpinsUsed)
    {
        slotManager.isInFreeSpins = false;
        totalFreeSpinsAwarded = 0;
        ShowUniversalWinPopup(WinPopupType.FreeSpinComplete, serverTotalRoundWin, 0, () => StartCoroutine(EndFreeSpinsTransitionSequence()));
    }

    private IEnumerator EndFreeSpinsTransitionSequence()
    {
        if (transitionBackFilm != null)
        {
            transitionBackFilm.gameObject.SetActive(true);
            transitionBackFilm.alpha = 0f;
            yield return transitionBackFilm.DOFade(1f, 0.5f).WaitForCompletion();
            yield return new WaitForSeconds(0.2f);
        }

        if (freeSpinCountContainer) freeSpinCountContainer.SetActive(false);
        if (gameLogoObject) gameLogoObject.SetActive(true);

        UpdateWinDisplay(0);
        SetSpinButtonReady();
        SetBetControlsEnabled(true);
        SetButtonInteractable(settingsOpenButton, settingsOpenButtonPortrait, true);

        if (transitionBackFilm != null)
        {
            yield return transitionBackFilm.DOFade(0f, 0.5f).WaitForCompletion();
            transitionBackFilm.gameObject.SetActive(false);
        }
    }

    internal void UpdateFreeSpinCount(int playedSpins, int totalSpins = -1)
    {
        if (totalSpins > 0) totalFreeSpinsAwarded = totalSpins;

        if (freeSpinCountContainer) freeSpinCountContainer.SetActive(true);
        if (remainingFreeSpinsText) remainingFreeSpinsText.text = $"FREE GAME  {playedSpins}  OF  {totalFreeSpinsAwarded}";
    }

    #endregion

    #region Expand / Shrink

    private void InitializeExpandShrink()
    {
        SetExpandShrinkButtons(isExpanded: false);
    }

    private void OnExpand()
    {
        isExpanded = true;
        jsFunctCalls?.RequestExpandGame();
        SetExpandShrinkButtons(isExpanded: true);
    }

    private void OnShrink()
    {
        isExpanded = false;
        jsFunctCalls?.RequestShrinkGame();
        SetExpandShrinkButtons(isExpanded: false);
    }

    private void SetExpandShrinkButtons(bool isExpanded)
    {
        SetButtonActive(expandButton, expandButtonPortrait, !isExpanded);
        SetButtonActive(shrinkButton, shrinkButtonPortrait, isExpanded);
    }

    #endregion

    #region Popup Animations (Generic)

    private void AnimatePopupOpen(RectTransform popupRect)
    {
        if (!popupRect) return;
        popupRect.localScale = Vector3.zero;
        popupRect.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
    }

    private void AnimatePopupClose(RectTransform popupRect, System.Action onComplete)
    {
        if (!popupRect)
        {
            onComplete?.Invoke();
            return;
        }

        audioController.PlayUIButton(false);

        Sequence closeSeq = DOTween.Sequence();
        closeSeq.Append(popupRect.DOScale(1.1f, 0.1f));
        closeSeq.Append(popupRect.DOScale(0f, 0.2f).SetEase(Ease.InBack));
        closeSeq.OnComplete(() =>
        {
            popupRect.localScale = Vector3.one;
            onComplete?.Invoke();
        });
    }

    #endregion

    #region Display Updates

    internal void UpdatePingDisplay(int pingMs)
    {
        SetTMPText(pingText, pingTextPortrait, $"{pingMs} ms");
    }

    internal void UpdatePingDisplay(string content)
    {
        SetTMPText(pingText, pingTextPortrait, content);
    }

    [System.Serializable]
    internal class JackpotValues
    {
        public string grandJackpot;
        public string majorJackpot;
        public string minorJackpot;
        public string miniJackpot;
    }

    internal void UpdateJackpotDisplay(JackpotValues values)
    {
        if (values == null) return;

        SetTMPText(grandJackpotText, grandJackpotTextPortrait, FormatJackpotValue(values.grandJackpot));
        SetTMPText(majorJackpotText, majorJackpotTextPortrait, FormatJackpotValue(values.majorJackpot));
        SetTMPText(minorJackpotText, minorJackpotTextPortrait, FormatJackpotValue(values.minorJackpot));
        SetTMPText(miniJackpotText, miniJackpotTextPortrait, FormatJackpotValue(values.miniJackpot));
    }

    private string FormatJackpotValue(string val)
    {
        if (string.IsNullOrEmpty(val)) return "$0.00";
        return val.StartsWith("$") ? val : "$" + val;
    }

    internal void UpdateBalance(double balance, bool doAnimate = false)
    {
        if (balanceTween != null) balanceTween.Kill();

        if (doAnimate)
        {
            double displayAmount = currentBalance;
            balanceTween = DOTween.To(() => displayAmount, val =>
            {
                displayAmount = val;
                SetTMPText(balanceText, balanceTextPortrait, val.ToString("F2"));
            }, balance, 1.5f);
        }
        else
        {
            SetTMPText(balanceText, balanceTextPortrait, balance.ToString("F2"));
        }
    }

    internal void UpdateBalanceDisplay(double newBalance)
    {
        currentBalance = newBalance;
        UpdateBalance(newBalance);
        if (currentBalance < currentTotalBet) LowBalPopup();
    }

    internal void UpdateWin(double winAmount, bool doAnimate = false)
    {
        UpdateWinDisplay(winAmount, doAnimate);
    }

    private void UpdateWinDisplay(double winAmount, bool doAnimate = false)
    {
        if (winTween != null) winTween.Kill();

        if (doAnimate)
        {
            double displayAmount = 0f;
            winTween = DOTween.To(() => displayAmount, val =>
            {
                displayAmount = val;
                SetTMPText(winAmountText, winAmountTextPortrait, val.ToString("F2"));
            }, winAmount, 1.5f);
        }
        else
        {
            SetTMPText(winAmountText, winAmountTextPortrait, winAmount.ToString("F2"));
        }

        bool showWinText = winAmount > 0 || (slotManager != null && slotManager.isInFreeSpins);
        SetGameObjectActive(goodLuckObject, goodLuckObjectPortrait, !showWinText);
        SetGameObjectActive(winTextObject, winTextObjectPortrait, showWinText);
    }

    #endregion

    #region Helper Methods

    private string FormatAmount(double amount)
    {
        return amount.ToString("0.###");
    }

    #endregion

    #region Popups

    private void CallOnExitFunction()
    {
        isExit = true;
        audioController.PlayUIButton(false);
        socketManager.CloseGame();
    }

    private void OpenPopup(GameObject popup, GameObject popupPortrait = null)
    {
        audioController.PlayUIButton(false);
        SetGameObjectActive(popup, popupPortrait, true);
        SetGameObjectActive(mainPopupObject, mainPopupObjectPortrait, true);
    }

    private void ClosePopup(GameObject popup, GameObject popupPortrait = null)
    {
        audioController.PlayUIButton(false);
        if (!IsActivePair(disconnectPopup, disconnectPopupPortrait))
        {
            if ((popup == quitPopup && IsActivePair(quitPopup, quitPopupPortrait))
                || (popup == lowBalancePopup && IsActivePair(lowBalancePopup, lowBalancePopupPortrait))
                || (popup == settingsPanel && IsActivePair(settingsPanel, settingsPanelPortrait))
                || (popup == autoPlayPanel && IsActivePair(autoPlayPanel, autoPlayPanelPortrait))
                || (popup == universalWinPopup && IsActivePair(universalWinPopup, universalWinPopupPortrait)))
            {
                SetGameObjectActive(mainPopupObject, mainPopupObjectPortrait, false);
            }
        }
        SetGameObjectActive(popup, popupPortrait, false);
    }

    internal void CheckAndClosePopups()
    {
        if (IsActivePair(reconnectPopup, reconnectPopupPortrait)) ClosePopup(reconnectPopup, reconnectPopupPortrait);
        if (IsActivePair(disconnectPopup, disconnectPopupPortrait)) ClosePopup(disconnectPopup, disconnectPopupPortrait);
    }

    internal void LowBalPopup()
    {
        OpenPopup(lowBalancePopup, lowBalancePopupPortrait);
    }

    internal void DisconnectionPopup()
    {
        if (!isExit)
        {
            isExit = true;
            OpenPopup(disconnectPopup, disconnectPopupPortrait);
        }
    }

    internal void ReconnectionPopup()
    {
        OpenPopup(reconnectPopup, reconnectPopupPortrait);
    }

    internal void ADfunction()
    {
        OpenPopup(adPopup, adPopupPortrait);
    }

    #endregion

    #region Universal Win Popup

    internal enum WinPopupType
    {
        BigWin,
        FreeSpinTrigger,
        FreeSpinComplete,
        BonusTrigger,
        BonusComplete
    }

    internal void ShowUniversalWinPopup(WinPopupType type, double winAmount, int freeSpinCount = 0, System.Action onTakePressed = null)
    {
        if (universalWinPopup == null && universalWinPopupPortrait == null) return;

        universalWinPopupCallback = onTakePressed;

        if (uwpWinTween != null)
        {
            uwpWinTween.Kill();
            uwpWinTween = null;
        }

        SetGameObjectActive(uwpCongratulationsTitle, uwpCongratulationsTitlePortrait, false);
        SetGameObjectActive(uwpYouWonSubtitle, uwpYouWonSubtitlePortrait, false);
        SetGameObjectActive(uwpBigWinTitle, uwpBigWinTitlePortrait, false);
        if (uwpWinAmountText) uwpWinAmountText.gameObject.SetActive(false);
        if (uwpWinAmountTextPortrait) uwpWinAmountTextPortrait.gameObject.SetActive(false);
        if (uwpFreeSpinCountText) uwpFreeSpinCountText.gameObject.SetActive(false);
        if (uwpFreeSpinCountTextPortrait) uwpFreeSpinCountTextPortrait.gameObject.SetActive(false);
        SetGameObjectActive(uwpFreeSpinTextObject, uwpFreeSpinObjectPortrait, false);

        switch (type)
        {
            case WinPopupType.FreeSpinTrigger:
                SetGameObjectActive(uwpCongratulationsTitle, uwpCongratulationsTitlePortrait, true);
                SetGameObjectActive(uwpYouWonSubtitle, uwpYouWonSubtitlePortrait, true);
                SetTMPText(uwpFreeSpinCountText, uwpFreeSpinCountTextPortrait, freeSpinCount.ToString());
                if (uwpFreeSpinCountText) uwpFreeSpinCountText.gameObject.SetActive(true);
                if (uwpFreeSpinCountTextPortrait) uwpFreeSpinCountTextPortrait.gameObject.SetActive(true);
                SetGameObjectActive(uwpFreeSpinTextObject, uwpFreeSpinObjectPortrait, true);
                break;

            case WinPopupType.FreeSpinComplete:
                SetGameObjectActive(uwpCongratulationsTitle, uwpCongratulationsTitlePortrait, true);
                SetGameObjectActive(uwpYouWonSubtitle, uwpYouWonSubtitlePortrait, true);
                if (uwpWinAmountText) { uwpWinAmountText.gameObject.SetActive(true); uwpWinAmountText.text = FormatAmount(winAmount); }
                if (uwpWinAmountTextPortrait) { uwpWinAmountTextPortrait.gameObject.SetActive(true); uwpWinAmountTextPortrait.text = FormatAmount(winAmount); }
                break;

            case WinPopupType.BigWin:
                SetGameObjectActive(uwpBigWinTitle, uwpBigWinTitlePortrait, true);
                if (uwpWinAmountText) { uwpWinAmountText.gameObject.SetActive(true); uwpWinAmountText.text = FormatAmount(winAmount); }
                if (uwpWinAmountTextPortrait) { uwpWinAmountTextPortrait.gameObject.SetActive(true); uwpWinAmountTextPortrait.text = FormatAmount(winAmount); }
                break;
        }

        bool showTakeButton = type != WinPopupType.BigWin;
        SetButtonActive(uwpTakeButton, uwpTakeButtonPortrait, showTakeButton);
        SetButtonInteractable(uwpTakeButton, uwpTakeButtonPortrait, showTakeButton);

        OpenPopup(universalWinPopup, universalWinPopupPortrait);

        AnimateUwpOpen(universalWinPopupRect);
        AnimateUwpOpen(universalWinPopupRectPortrait);

        if (winAmount > 0)
        {
            AnimateUwpWinCount(uwpWinAmountText, winAmount, type == WinPopupType.BigWin);
            AnimateUwpWinCount(uwpWinAmountTextPortrait, winAmount, type == WinPopupType.BigWin);
        }

        if (uwpAutoCloseCoroutine != null) StopCoroutine(uwpAutoCloseCoroutine);
        uwpAutoCloseCoroutine = StartCoroutine(AutoCloseUniversalWinPopup());
    }

    private void AnimateUwpOpen(RectTransform rect)
    {
        if (!rect) return;
        rect.localScale = Vector3.zero;
        Sequence openSeq = DOTween.Sequence();
        openSeq.Append(rect.DOScale(1.2f, 0.5f).SetEase(Ease.OutCubic));
        openSeq.Append(rect.DOScale(1f, 0.3f).SetEase(Ease.InOutSine));
    }

    private void AnimateUwpWinCount(TMP_Text text, double winAmount, bool isBigWin)
    {
        if (text == null || !text.gameObject.activeSelf) return;

        int decimals = GetDecimalPlaces(winAmount);
        string formatStr = decimals > 0 ? "0." + new string('0', decimals) : "0";
        text.text = (0.0).ToString(formatStr);

        float countUpDuration = isBigWin ? 1.5f : 1.0f;
        uwpWinTween = DOVirtual.Float(0f, (float)winAmount, countUpDuration, val => text.text = val.ToString(formatStr))
            .OnComplete(() => text.text = FormatAmount(winAmount));
    }

    private int GetDecimalPlaces(double amount)
    {
        double rounded = System.Math.Round(amount, 4);
        string str = rounded.ToString(System.Globalization.CultureInfo.InvariantCulture);
        int dotIndex = str.IndexOf('.');
        if (dotIndex < 0) return 0;
        return str.Length - dotIndex - 1;
    }

    private IEnumerator AutoCloseUniversalWinPopup()
    {
        yield return new WaitForSeconds(uwpAutoCloseDelay);
        uwpAutoCloseCoroutine = null;
        CloseUniversalWinPopup();
    }

    private void OnUniversalWinTakeButtonClicked()
    {
        audioController.PlayUIButton(false);
        CloseUniversalWinPopup();
    }

    private void CloseUniversalWinPopup()
    {
        if (uwpWinTween != null)
        {
            uwpWinTween.Kill();
            uwpWinTween = null;
        }

        if (uwpAutoCloseCoroutine != null)
        {
            StopCoroutine(uwpAutoCloseCoroutine);
            uwpAutoCloseCoroutine = null;
        }

        var callback = universalWinPopupCallback;
        universalWinPopupCallback = null;

        ClosePopup(universalWinPopup, universalWinPopupPortrait);
        callback?.Invoke();
    }

    #endregion

    #region Symbol Info Card

    internal void ShowSymbolInfoCard(int symbolId, int col, int row, RectTransform symbolRect)
    {
        if (symbolInfoCard == null) return;
        symbolInfoCard.ShowCard(symbolId, col, row, symbolRect, socketManager, betCounter);
    }

    internal void HideSymbolInfoCard()
    {
        if (symbolInfoCard == null) return;
        symbolInfoCard.HideCard();
    }

    #endregion

    #region Bonus Background Toggle

    internal void ToggleBonusBackground(bool isBonus)
    {
        if (bgImage == null) return;
        bgImage.sprite = isBonus ? redSprite : blueSprite;
    }

    #endregion

    #region Background Shine Animation

    internal void StartBGAnimation()
    {
        // if (bgAnimationRoutine != null) StopCoroutine(bgAnimationRoutine);
        // bgAnimationRoutine = StartCoroutine(BGAnimationRoutine());
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
        if (shinePrefab == null || shineParentRect == null) return;

        float x = Random.Range(-960f, 960f);
        float y = Random.Range(-540f, 540f);

        GameObject shine = Instantiate(shinePrefab, shineParentRect);
        shine.transform.localPosition = new Vector2(x, y);
        shine.transform.localScale = Vector3.zero;

        if (shine.TryGetComponent(out CanvasGroup cg))
        {
            cg.alpha = 0f;

            Sequence shineSeq = DOTween.Sequence();
            shineSeq.Append(shine.transform.DOScale(1f, 1f).SetEase(Ease.Linear));
            shineSeq.Join(cg.DOFade(1f, 1f).SetEase(Ease.Linear));
            shineSeq.Append(shine.transform.DOScale(0f, 1f).SetEase(Ease.Linear));
            shineSeq.Join(cg.DOFade(0f, 1f).SetEase(Ease.Linear));
            shineSeq.OnComplete(() => Destroy(shine));
        }
        else
        {
            Destroy(shine);
        }
    }

    #endregion

    #region Game Init / Bonus / Buttons Helper Surface

    // Kept as thin wrapper methods with these exact names/signatures — BonusManager.cs and
    // SlotManager.cs (both out of scope this pass) call directly into this surface.

    internal void InitialiseUIData(Root root)
    {
        currentBalance = root.player.balance;
        currentTotalBet = root.gameData.bets[betCounter] * root.gameData.lines.Count;

        UpdateBalance(root.player.balance);
        UpdateWin(0.00);
        UpdateBetDisplay();

        //bonusManager.IntializeBonusWheelValue();
        StartBGAnimation();
    }

    internal void SetBetButtonsInteractable(bool interactable)
    {
        SetBetControlsEnabled(interactable);
    }

    internal void SetSpinButtonInteractable(bool interactable)
    {
        SetButtonInteractable(spinButton, spinButtonPortrait, interactable);
    }

    internal void SetSpinButtonActive(bool active)
    {
        SetButtonActive(spinButton, spinButtonPortrait, active);
    }

    internal void SetStopSpinButtonActive(bool active)
    {
        SetButtonActive(stopButton, stopButtonPortrait, active);
    }

    internal void SetAutoSpinButtonInteractable(bool interactable)
    {
        SetButtonInteractable(autoSpinButton, autoSpinButtonPortrait, interactable);
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

    #region Cleanup

    private void OnDestroy()
    {
        if (balanceTween != null) balanceTween.Kill();
        if (winTween != null) winTween.Kill();
        DOTween.KillAll();
    }

    #endregion
}
