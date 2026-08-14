using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using DG.Tweening;
using System;
using System.Collections;
using Unity.VisualScripting;

public class SlotManager : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] private Sprite[] _symbolSprites;

    [Header("Slot Images")]
    [SerializeField] private List<SlotImage> _totalImages;
    [SerializeField] internal List<SlotImage> _resultImages;
    [SerializeField] internal List<CanvasGroup> redSlotBgs;

    [Header("Slots Transforms")]
    [SerializeField] private Transform[] _slotTransforms;

    [Header("Win Popup UI")]
    [SerializeField] private GameObject MainWinPopup;
    [SerializeField] private GameObject BlackBG;
    [SerializeField] private ImageAnimation WinBg;
    [SerializeField] private Image WinImage;
    [SerializeField] private Sprite BigWinSprite;
    [SerializeField] private Sprite HugeWinSprite;
    [SerializeField] private Sprite MegaWinSprite;
    [SerializeField] private TMP_Text WinBigText;
    [SerializeField] private Button SkipButton;
    [SerializeField] private List<Image> payLines;
    [SerializeField] private List<CanvasGroup> lineWinObjects;
    [SerializeField] private List<TMP_Text> lineWinTexts;

    [SerializeField] private List<AnimationSprites> symbolAnimations;

    [Header("Bonus Win Popup UI")]
    [SerializeField] private GameObject MainBonusWinPopup;
    [SerializeField] private GameObject BonusBlackBG;
    [SerializeField] private ImageAnimation BonusWinBg;
    [SerializeField] private CoinFountainPool coinAnimation;
    [SerializeField] private ImageAnimation BonusSymbolAnimation;
    [SerializeField] private Image BonusWinImage;
    [SerializeField] private TMP_Text BonusWinBigText;
    [SerializeField] private Button BonusSkipButton;

    [Header("Managers")]
    [SerializeField] private AudioController audioController;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private BonusManager bonusManager;
    [SerializeField] private SocketIOManager socketManager;


    internal bool SocketConnected = false;
    internal bool _isAutoSpin = false;
    private bool _wasAutoSpinOn;
    private List<Tween> _alltweens = new List<Tween>();
    private Coroutine _autoSpinRoutine = null;
    private Coroutine _tweenRoutine;
    private Coroutine _winLinesLoopRoutine;
    private bool _isSpinning = false;
    internal int _numberOfSlots = 3;
    private bool _stopSpinToggle;
    private float _spinDelay = 0.3f;
    private bool _isTurboOn;
    private bool isTweening = false;

    private Tween WinBigTextTween;

    // Skip flags - set true when the corresponding skip button is pressed
    // while its popup is active. Checked by the popup coroutines to fast-forward.
    private bool _skipWinPopup = false;
    private bool _skipBonusPopup = false;

    #region Initial Functions

    private void Start()
    {
        shuffleSlotImages();
        //StartCoroutine(ShowWinLineAnimation(14.99f));
        //StartCoroutine(BonusWinPopup(17.99f));

        if (SkipButton != null)
            SkipButton.onClick.AddListener(RequestSkipWinPopup);

        if (BonusSkipButton != null)
            BonusSkipButton.onClick.AddListener(RequestSkipBonusWinPopup);
    }

    /// <summary>
    /// Call this from the win-popup skip button. Only takes effect while the
    /// win-line popup is actually showing.
    /// </summary>
    internal void RequestSkipWinPopup()
    {
        if (MainWinPopup != null && MainWinPopup.activeSelf)
            _skipWinPopup = true;
    }

    /// <summary>
    /// Call this from the bonus-popup skip button. Only takes effect while the
    /// bonus win popup is actually showing.
    /// </summary>
    internal void RequestSkipBonusWinPopup()
    {
        if (MainBonusWinPopup != null && MainBonusWinPopup.activeSelf)
            _skipBonusPopup = true;
    }

    /// <summary>
    /// Drop-in replacement for WaitForSeconds that returns early the moment
    /// skipCondition() becomes true, instead of waiting out the full duration.
    /// </summary>
    private IEnumerator WaitOrSkip(float seconds, Func<bool> skipCondition)
    {
        float elapsed = 0f;
        while (elapsed < seconds)
        {
            if (skipCondition())
                yield break;
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    internal void shuffleSlotImages(bool midTween = false)
    {
        foreach (var slotImg in _totalImages)
        {
            for (int j = 0; j < slotImg.slotImages.Count; j++)
            {
                int randomSprite = UnityEngine.Random.Range(1, _symbolSprites.Length);
                Sprite image = (j % 2 == 0)
                    ? _symbolSprites[randomSprite] // random symbol
                    : _symbolSprites[0]; // blank

                slotImg.slotImages[j].sprite = image;
                SetSymbolSize(slotImg.slotImages[j], randomSprite);
            }
        }
    }

    #endregion

    #region Autospin
    internal void AutoSpin()
    {
        if (!_isAutoSpin)
        {
            _isAutoSpin = true;

            if (_autoSpinRoutine != null)
            {
                StopCoroutine(_autoSpinRoutine);
                _autoSpinRoutine = null;
            }
            _autoSpinRoutine = StartCoroutine(AutoSpinCoroutine());
        }
    }

    internal void StopAutoSpin()
    {
        //_audioController.PlayButtonAudio();
        if (_isAutoSpin)
        {
            _isAutoSpin = false;
            _wasAutoSpinOn = false;
            StartCoroutine(StopAutoSpinCoroutine());
        }
    }

    private IEnumerator AutoSpinCoroutine()
    {
        while (_isAutoSpin)
        {
            StartSlots(_isAutoSpin);
            yield return new WaitUntil(() => !_isSpinning);
            yield return new WaitForSeconds(_spinDelay);
        }
        if (_wasAutoSpinOn)
            _wasAutoSpinOn = false;
    }

    private IEnumerator StopAutoSpinCoroutine()
    {
        yield return new WaitUntil(() => !_isSpinning);
        _isAutoSpin = false;
        uiManager.AutoSpinButton.interactable = true;
        if (_autoSpinRoutine != null)
        {
            StopCoroutine(_autoSpinRoutine);
            _autoSpinRoutine = null;
        }
        // The current spin just finished and autospin is now fully cancelled.
        // Tell UIManager to restore the spin button now that we are truly idle.
        uiManager.SetSpinButtonReady();
    }
    #endregion

    #region SlotSpin

    internal void RequestInstantStop()
    {
        if (isTweening)
        {
            _stopSpinToggle = true;
        }
    }

    internal void StartSlots(bool autoSpin = false)
    {
        if (_isSpinning) return;

        if (!autoSpin)
        {
            if (_autoSpinRoutine != null)
            {
                StopCoroutine(_autoSpinRoutine);
                StopCoroutine(_tweenRoutine);
                _tweenRoutine = null;
                _autoSpinRoutine = null;
            }
        }
        _tweenRoutine = StartCoroutine(TweenRoutine());
    }

    private IEnumerator TweenRoutine()
    {
        _isSpinning = true;

        StopWinLinesLoop();

        if (uiManager.currentBalance < uiManager.currentTotalBet)
        {
            StopAutoSpin();
            _isSpinning = false;
            yield return new WaitForSeconds(1);
            uiManager.LowBalPopup();
            uiManager.SetBetButtonsInteractable(true);
            yield break;
        }

        uiManager.UpdateBalance(uiManager.currentBalance - uiManager.currentTotalBet);
        uiManager.currentBalance = uiManager.currentBalance - uiManager.currentTotalBet;
        //Debug.Log(uiManager.currentTotalBet);
        uiManager.UpdateWin(0.00);
        yield return null;

        //if (_audioController) _audioController.PlayWLAudio("spin");

        _isSpinning = true;
        isTweening = true;

        for (int i = 0; i < _numberOfSlots; i++)
        {
            InitializeTweening(_slotTransforms[i]);
            yield return new WaitForSeconds(0.1f);
        }
        audioController.PlayReelSpinning(true);

        ResetAllSymbol();

        socketManager.AccumulateResult(uiManager.betCounter);
        yield return new WaitUntil(() => socketManager.isResultdone);

        // // Load result matrix into result images
        for (int j = 0; j < socketManager.resultData.payload.reels.Count; j++)
        {
            for (int i = 0; i < socketManager.resultData.payload.reels[j].Count; i++)
            {
                if (int.TryParse(socketManager.resultData.payload.reels[j][i], out int symbolId))
                {
                    _resultImages[i].slotImages[j].sprite = _symbolSprites[symbolId];
                    SetSymbolSize(_resultImages[i].slotImages[j], symbolId);
                }
            }
        }

        if (_isTurboOn)
        {
            _stopSpinToggle = true;
        }

        if (!_stopSpinToggle)
        {
            for (int i = 0; i < _numberOfSlots; i++)
            {
                yield return new WaitForSeconds(0.2f);
                if (_stopSpinToggle) break;
            }
        }
        int bonusSymbolCount = 0;
        for (int i = 0; i < _numberOfSlots; i++)
        {
            if (bonusSymbolCount > 1)
            {
                foreach (var obj in redSlotBgs)
                {
                    obj.DOFade(1f, 1.3f).SetEase(Ease.Linear);
                }
                yield return new WaitForSeconds(1.3f);
            }
            yield return StopTweening(_slotTransforms[i], i, _stopSpinToggle);
            for (int j = 0; j < _resultImages[i].slotImages.Count; j++)
            {
                if (_resultImages[i].slotImages[j].sprite == _symbolSprites[7])
                {
                    bonusSymbolCount++;
                    break;
                }
            }
            if (!socketManager.resultData.payload.features.cashSpinnerBonus.triggered && !socketManager.resultData.payload.features.doubleCashSpinnerBonus.triggered)
            {
                foreach (var obj in redSlotBgs)
                {
                    obj.alpha = 0f;
                }
            }
            audioController.PlayReelHit(false);
        }
        isTweening = false;
        _stopSpinToggle = false;

        //if (_audioController) _audioController.StopWLAaudio();
        yield return _alltweens[^1].WaitForCompletion();
        KillAllTweens();

        if (socketManager.resultData.payload.features.cashSpinnerBonus.triggered || socketManager.resultData.payload.features.doubleCashSpinnerBonus.triggered)
        {
            if (socketManager.resultData.payload.features.cashSpinnerBonus.triggered)
            {
                foreach (var slotObj in socketManager.resultData.payload.features.cashSpinnerBonus.positions)
                {
                    int row = slotObj[0];
                    int col = slotObj[1];

                    int.TryParse(socketManager.resultData.payload.reels[row][col], out int symbolID);
                    ImageAnimation anim = _resultImages[col].slotImages[row].GetComponent<ImageAnimation>();
                    anim.textureArray = GetAnimationSprite(symbolID);
                    anim.AnimationSpeed = 30f;
                    anim.doLoopAnimation = false;
                    anim.StartAnimation();
                }
            }
            else
            {
                foreach (var slotObj in socketManager.resultData.payload.features.doubleCashSpinnerBonus.positions)
                {
                    int row = slotObj[0];
                    int col = slotObj[1];

                    int.TryParse(socketManager.resultData.payload.reels[row][col], out int symbolID);
                    ImageAnimation anim = _resultImages[col].slotImages[row].GetComponent<ImageAnimation>();
                    anim.textureArray = GetAnimationSprite(symbolID);
                    anim.AnimationSpeed = 30f;
                    anim.doLoopAnimation = false;
                    anim.StartAnimation();
                }
            }
            yield return new WaitForSeconds(1.5f);

            bonusManager.isBonusFinished = false;
            bonusManager.BonusWheel();
            yield return new WaitUntil(() => bonusManager.isBonusFinished);

            if (socketManager.resultData.payload.features.doubleCashSpinnerBonus.triggered)
            {
                yield return BonusWinPopup(socketManager.resultData.payload.features.doubleCashSpinnerBonus.award);
            }
            else
            {
                yield return BonusWinPopup(socketManager.resultData.payload.features.cashSpinnerBonus.award);
            }
            foreach (var obj in redSlotBgs)
            {
                obj.alpha = 0f;
            }
        }

        if (socketManager.resultData.payload.winningLines.Count > 0)
        {
            yield return ShowWinLineAnimation(socketManager.resultData.payload.winningLines, socketManager.resultData.payload.totalWin);
        }

        _isSpinning = false;
        uiManager.currentBalance = socketManager.resultData.player.balance;

        //if (!_isAutoSpin)
        {
            uiManager.SetSpinButtonReady();
        }
    }
    #endregion

    #region Win Line Animation

    private IEnumerator ShowWinLineAnimation(List<WinningLine> winLines, double winAmount)
    {
        _skipWinPopup = false;
        MainWinPopup.GetComponent<CanvasGroup>().alpha = 0f;
        MainWinPopup.SetActive(true);
        WinBigTextTween = null;
        WinBigText.gameObject.SetActive(true);

        if (winAmount < uiManager.currentTotalBet * 5)
        {
            audioController.PlayNormalWin(false);
        }
        else
        {
            audioController.PlayBigWin(false);
        }

        if (winAmount > uiManager.currentTotalBet * 5)
        {
            BlackBG.SetActive(true);
            WinBg.gameObject.SetActive(true);
            WinBg.StartAnimation();
        }
        if (winAmount > uiManager.currentTotalBet * 10)
        {
            BlackBG.SetActive(true);
            WinImage.sprite = BigWinSprite;
            WinImage.gameObject.SetActive(true);
            WinBg.gameObject.SetActive(true);
            WinBg.StartAnimation();
        }
        if (winAmount > uiManager.currentTotalBet * 20)
        {
            BlackBG.SetActive(true);
            WinImage.sprite = HugeWinSprite;
            WinImage.gameObject.SetActive(true);
            WinBg.gameObject.SetActive(true);
            WinBg.StartAnimation();
        }
        if (winAmount > uiManager.currentTotalBet * 30)
        {
            BlackBG.SetActive(true);
            WinImage.sprite = MegaWinSprite;
            WinImage.gameObject.SetActive(true);
            WinBg.gameObject.SetActive(true);
            WinBg.StartAnimation();
        }

        WinBigText.text = UIManager.ToSpriteString(0.00);
        MainWinPopup.GetComponent<CanvasGroup>().DOFade(1f, 0.1f);

        double displayAmount = 0f;
        WinBigTextTween = DOTween.To(() => displayAmount, val =>
        {
            displayAmount = val;
            WinBigText.text = UIManager.ToSpriteString(val, "F2");
        }, winAmount, 1.5f);

        uiManager.UpdateWin(winAmount, true);
        uiManager.UpdateBalance(uiManager.currentBalance + winAmount, true);

        foreach (var winLine in winLines)
        {
            foreach (var line in winLine.positions)
            {
                int col = line[0];
                int row = line[1];
                float currentScale = _resultImages[row].slotImages[col].transform.localScale.x;
                _resultImages[row].slotImages[col].transform.DOScale(currentScale + 0.15f, 0.7f);
            }
        }

        yield return WaitOrSkip(2.5f, () => _skipWinPopup);

        if (_skipWinPopup)
        {
            yield return SkipCloseWinPopup(winAmount);
            //yield break;
        }

        foreach (var winLine in winLines)
        {
            foreach (var line in winLine.positions)
            {
                int col = line[0];
                int row = line[1];
                int symbolID = GetSymbolIndex(_resultImages[row].slotImages[col].sprite);
                SetSymbolSize(_resultImages[row].slotImages[col], symbolID, 0.4f);
            }
        }
        if (!_skipWinPopup)
        {
            MainWinPopup.GetComponent<CanvasGroup>().DOFade(0f, 0.4f).OnComplete(() =>
            {
                BlackBG.SetActive(false);
                WinImage.gameObject.SetActive(false);
                WinBg.gameObject.SetActive(false);
                MainWinPopup.SetActive(false);
            });

        }
        yield return new WaitForSeconds(0.5f);

        if (_isAutoSpin)
        {
            // Autospin: show every winning line once, then let the next spin proceed.
            yield return LoopWinLines(winLines, oneShot: true);
        }
        else
        {
            // Manual spin: keep cycling the winning lines until the player starts the next spin.
            _winLinesLoopRoutine = StartCoroutine(LoopWinLines(winLines, oneShot: false));
        }
        // for (int i = 0; i < winLines.Count; i++)
        // {
        //     Color32 c = payLines[winLines[i].lineIndex].color;
        //     c.a = 255; // Fully visible on a 0-255 scale
        //     payLines[winLines[i].lineIndex].color = c;
        //     foreach (var line in winLines[i].positions)
        //     {
        //         int col = line[0];
        //         int row = line[1];
        //         int.TryParse(socketManager.resultData.payload.reels[col][row], out int symbolID);
        //         ImageAnimation anim = _resultImages[row].slotImages[col].GetComponent<ImageAnimation>();
        //         anim.textureArray = GetAnimationSprite(symbolID);
        //         if (symbolID == 1)
        //         {
        //             anim.AnimationSpeed = 15f;
        //         }
        //         else
        //         {
        //             anim.AnimationSpeed = 37f;
        //         }
        //         anim.doLoopAnimation = false;
        //         anim.StartAnimation();
        //     }
        //     int tempIndex = GetPerLineWinIndex(winLines[i].lineIndex);
        //     lineWinObjects[tempIndex].transform.localScale = new Vector3(0f, 0f, 0f);
        //     lineWinTexts[tempIndex].text = winLines[i].payout.ToString("F2");
        //     lineWinObjects[tempIndex].DOFade(1f, 1f).SetEase(Ease.Linear);
        //     lineWinObjects[tempIndex].transform.DOScale(1f, 0.7f).SetEase(Ease.Linear);
        //     yield return new WaitForSeconds(1.5f);
        //     c.a = 150;
        //     payLines[winLines[i].lineIndex].color = c;
        //     lineWinObjects[tempIndex].DOFade(0f, 0.7f).SetEase(Ease.Linear);
        // }
    }

    private IEnumerator LoopWinLines(List<WinningLine> winLines, bool oneShot)
    {
        // oneShot = true (autospin): runs through the winning lines exactly once, then returns
        //           so autospin can proceed to the next spin.
        // oneShot = false (manual spin): cycles forever. StopWinLinesLoop() (called from
        //           TweenRoutine the moment a new spin starts) is what actually ends this.
        do
        {
            for (int i = 0; i < winLines.Count; i++)
            {
                Color32 c = payLines[winLines[i].lineIndex].color;
                c.a = 255; // Fully visible on a 0-255 scale
                payLines[winLines[i].lineIndex].color = c;
                foreach (var line in winLines[i].positions)
                {
                    int col = line[0];
                    int row = line[1];
                    int.TryParse(socketManager.resultData.payload.reels[col][row], out int symbolID);
                    ImageAnimation anim = _resultImages[row].slotImages[col].GetComponent<ImageAnimation>();
                    anim.textureArray = GetAnimationSprite(symbolID);
                    if (symbolID == 1)
                    {
                        anim.AnimationSpeed = 15f;
                    }
                    else
                    {
                        anim.AnimationSpeed = 37f;
                    }
                    anim.doLoopAnimation = false;
                    anim.StartAnimation();
                }
                int tempIndex = GetPerLineWinIndex(winLines[i].lineIndex);
                lineWinObjects[tempIndex].transform.localScale = new Vector3(0f, 0f, 0f);
                lineWinTexts[tempIndex].text = winLines[i].payout.ToString("F2");
                lineWinObjects[tempIndex].DOFade(1f, 1f).SetEase(Ease.Linear);
                lineWinObjects[tempIndex].transform.DOScale(1f, 0.7f).SetEase(Ease.Linear);
                yield return new WaitForSeconds(1.5f);
                c.a = 150;
                payLines[winLines[i].lineIndex].color = c;
                lineWinObjects[tempIndex].DOFade(0f, 0.7f).SetEase(Ease.Linear);
            }
        } while (!oneShot);
    }

    /// <summary>
    /// Stops the repeating win-lines display (if running) and snaps its visuals
    /// back to idle so nothing is left mid-fade or mid-scale when the next spin starts.
    /// </summary>
    private void StopWinLinesLoop()
    {
        if (_winLinesLoopRoutine == null) return;

        StopCoroutine(_winLinesLoopRoutine);
        _winLinesLoopRoutine = null;

        foreach (var payLine in payLines)
        {
            payLine.DOKill();
            Color32 c = payLine.color;
            c.a = 150;
            payLine.color = c;
        }

        foreach (var lineWinObject in lineWinObjects)
        {
            lineWinObject.DOKill();
            lineWinObject.transform.DOKill();
            lineWinObject.alpha = 0f;
            lineWinObject.transform.localScale = Vector3.zero;
        }
    }

    /// <summary>
    /// Fast-forward path for the win-line popup: kills the counting tween,
    /// snaps the win text straight to the final amount, and fades the popup
    /// closed over 1.5s (instead of the normal 0.4s close + per-line breakdown).
    /// </summary>
    private IEnumerator SkipCloseWinPopup(double winAmount)
    {
        if (WinBigTextTween != null && WinBigTextTween.IsActive())
        {
            WinBigTextTween.Kill();
        }
        WinBigText.text = UIManager.ToSpriteString(winAmount, "F2");

        yield return new WaitForSeconds(1f);

        MainWinPopup.GetComponent<CanvasGroup>().DOFade(0f, 0.5f).OnComplete(() =>
        {
            BlackBG.SetActive(false);
            WinImage.gameObject.SetActive(false);
            WinBg.gameObject.SetActive(false);
            MainWinPopup.SetActive(false);
        });

    }

    private IEnumerator BonusWinPopup(double winAmount)
    {
        _skipBonusPopup = false;
        MainBonusWinPopup.GetComponent<CanvasGroup>().alpha = 0f;
        MainBonusWinPopup.SetActive(true);
        WinBigTextTween = null;
        BonusWinBigText.gameObject.SetActive(true);
        BonusWinImage.sprite = BigWinSprite;

        if (winAmount > uiManager.currentTotalBet * 20)
        {
            BonusWinImage.sprite = HugeWinSprite;
        }
        if (winAmount > uiManager.currentTotalBet * 30)
        {
            BonusWinImage.sprite = MegaWinSprite;
        }

        BonusBlackBG.SetActive(true);
        BonusWinImage.gameObject.SetActive(true);
        BonusWinBg.gameObject.SetActive(true);
        BonusSymbolAnimation.StartAnimation();
        coinAnimation.StartFountain();
        BonusWinBg.StartAnimation();

        BonusWinBigText.text = UIManager.ToSpriteString(0.00);
        MainBonusWinPopup.GetComponent<CanvasGroup>().DOFade(1f, 0.3f);

        double displayAmount = 0f;
        WinBigTextTween = DOTween.To(() => displayAmount, val =>
        {
            displayAmount = val;
            BonusWinBigText.text = UIManager.ToSpriteString(val, "F2");
        }, winAmount, 4f);

        uiManager.UpdateWin(winAmount, true);
        uiManager.UpdateBalance(uiManager.currentBalance + winAmount, true);

        yield return WaitOrSkip(6f, () => _skipBonusPopup);

        if (_skipBonusPopup)
        {
            yield return SkipCloseBonusPopup(winAmount);
            yield break;
        }

        MainBonusWinPopup.GetComponent<CanvasGroup>().DOFade(0f, 0.5f).OnComplete(() =>
        {
            coinAnimation.ClearAll();
            BonusBlackBG.SetActive(false);
            BonusWinImage.gameObject.SetActive(false);
            BonusWinBg.gameObject.SetActive(false);
            MainBonusWinPopup.SetActive(false);
        });
        yield return new WaitForSeconds(0.5f);
    }

    /// <summary>
    /// Fast-forward path for the bonus win popup: kills the counting tween,
    /// snaps the win text straight to the final amount, and fades the popup
    /// closed over 1.5s.
    /// </summary>
    private IEnumerator SkipCloseBonusPopup(double winAmount)
    {
        if (WinBigTextTween != null && WinBigTextTween.IsActive())
        {
            WinBigTextTween.Kill();
        }
        BonusWinBigText.text = UIManager.ToSpriteString(winAmount, "F2");

        yield return new WaitForSeconds(1.5f);

        MainBonusWinPopup.GetComponent<CanvasGroup>().DOFade(0f, 0.5f).OnComplete(() =>
        {
            coinAnimation.ClearAll();
            BonusBlackBG.SetActive(false);
            BonusWinImage.gameObject.SetActive(false);
            BonusWinBg.gameObject.SetActive(false);
            MainBonusWinPopup.SetActive(false);
        });

    }

    #endregion

    #region TweeningCode

    private void InitializeTweening(Transform slotTransform)
    {
        // Snap to top off-screen
        slotTransform.localPosition = new Vector2(slotTransform.localPosition.x, 800f);

        Tween tween = slotTransform
            .DOLocalMoveY(-800f, 0.27f)
            .SetLoops(-1, LoopType.Restart)
            .SetEase(Ease.Flash);          // accelerate in, decelerate out → feels weighted
                                           // .OnStepComplete(() =>
                                           // {
                                           //     shuffleSlotImages(midTween: true);
                                           // });

        _alltweens.Add(tween);
    }

    private IEnumerator StopTweening(Transform slotTransform, int index, bool isStop)
    {
        if (!isStop)
        {
            // Wait for the current loop cycle to naturally complete
            // (the reel is at the top — off screen — at this moment)
            bool isComplete = false;
            _alltweens[index].OnStepComplete(() => isComplete = true);
            yield return new WaitUntil(() => isComplete);
        }

        _alltweens[index].Kill();

        // Start from just above the visible area, not from 600
        // This makes the landing feel like a natural continuation of the scroll
        slotTransform.localPosition = new Vector2(slotTransform.localPosition.x, 350f);

        // Smooth slide down with a gentle overshoot — reduce elastic strength
        _alltweens[index] = slotTransform
            .DOLocalMoveY(0f, 0.25f)        // SlotManager landing y (use 400f for BonusManager)
            .SetEase(Ease.OutBack)            // subtle overshoot, not a full elastic bounce
            .SetSpeedBased(false);

        if (!isStop)
            yield return new WaitForSeconds(0.2f);
        else
            yield return null;
    }

    private void KillAllTweens()
    {
        if (_alltweens.Count > 0)
        {
            for (int i = 0; i < _alltweens.Count; i++)
                _alltweens[i].Kill();
            _alltweens.Clear();
        }
    }

    #endregion
    #region Helper Function

    private int GetSymbolIndex(Sprite sprite)
    {
        for (int i = 0; i < _symbolSprites.Length; i++)
        {
            if (sprite == _symbolSprites[i])
            {
                return i;
            }
        }
        return 0;
    }

    private int GetPerLineWinIndex(int lineIndex)
    {
        if (lineIndex == 0 || lineIndex == 3 || lineIndex == 4 || lineIndex == 7 || lineIndex == 8)
        {
            return 1;
        }
        else if (lineIndex == 1 || lineIndex == 5)
        {
            return 0;
        }
        else
        {
            return 2;
        }
    }

    private List<Sprite> GetAnimationSprite(int symbolID)
    {
        return symbolAnimations[symbolID - 1].sprites;
    }

    private void SetSymbolSize(Image slotImage, int symbolID, float time = 0f)
    {
        switch (symbolID)
        {
            case 1:
                //slotImage.transform.localScale = new Vector2(1.15f, 1.15f);
                slotImage.transform.DOScale(1.15f, time);
                break;

            case 5:
                //slotImage.transform.localScale = new Vector2(0.87f, 0.87f);
                slotImage.transform.DOScale(0.87f, time);
                break;

            case 6:
                //slotImage.transform.localScale = new Vector2(0.87f, 0.87f);
                slotImage.transform.DOScale(0.87f, time);
                break;

            case 7:
                //slotImage.transform.localScale = new Vector2(1.7f, 1.7f);
                slotImage.transform.DOScale(1.7f, time);
                break;

            default:
                slotImage.transform.DOScale(1f, time);
                break;
        }
    }

    private void ResetAllSymbol()
    {
        foreach (var slotimage in _resultImages)
        {
            foreach (var image in slotimage.slotImages)
            {
                image.transform.localScale = Vector2.one;
                image.GetComponent<ImageAnimation>().StopAnimation();
            }
        }
        foreach (var obj in redSlotBgs)
        {
            obj.alpha = 0f;
        }
    }

    #endregion
}

[Serializable]
public class SlotImage
{
    public List<Image> slotImages = new List<Image>(10);
}

[Serializable]
public class AnimationSprites
{
    public List<Sprite> sprites = new List<Sprite>();
}