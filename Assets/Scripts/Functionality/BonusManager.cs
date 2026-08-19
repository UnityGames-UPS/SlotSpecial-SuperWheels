using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using DG.Tweening;
using System;
using System.Collections;

public class BonusManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup BlackBG;
    [SerializeField] private GameObject SlotPanel;
    [SerializeField] private GameObject BonusPanel;
    [SerializeField] private SpineAnimController bonusWheelAnimation;
    [SerializeField] private GameObject WheelStartPanel;
    [SerializeField] private GameObject singleWheelInfo;
    [SerializeField] private GameObject doubleWheelInfo;
    [SerializeField] private Button startPanelButton;
    [SerializeField] private Button SpinButton;
    [SerializeField] private GameObject WheelEndPanel;
    [SerializeField] private TMP_Text wheelWinText;
    [SerializeField] private RectTransform multiplyTwoText;
    [SerializeField] private TMP_Text lineMultiplierText;
    [SerializeField] private TMP_Text TotalWinText;
    [SerializeField] private RectTransform MainWheelParent;
    [SerializeField] private RectTransform ArrowObject;
    [SerializeField] private RectTransform SingleBonusWheel;
    [SerializeField] private RectTransform DoubleBonusWheel;
    [SerializeField] private CanvasGroup InnerWheeltext;
    [SerializeField] private GameObject BlackWheelOverlayObject;
    [SerializeField] private List<TMP_Text> singleBonusWheelSegmentTexts;
    [SerializeField] private List<TMP_Text> doubleBonusWheelSegmentTexts;

    [Header("Animation Sprites")]
    [SerializeField] private ImageAnimation OuterBorder;
    [SerializeField] private ImageAnimation InnerBorder;
    [SerializeField] private ImageAnimation Pointer;
    [SerializeField] private List<Sprite> OuterBorderOnebyOneBlinkLoop;
    [SerializeField] private List<Sprite> InnerBorderOnebyOneBlinkLoop;
    [SerializeField] private List<Sprite> OuterBorderBlinkLoop;
    [SerializeField] private List<Sprite> InnerBorderBlinkLoop;
    [SerializeField] private List<Sprite> PointerBlinkLoop;

    [SerializeField] private int rotation = 4;
    [SerializeField] private float time = 4f;

    [SerializeField] private SocketIOManager socketManager;
    [SerializeField] private UIManager uIManager;
    [SerializeField] private AudioController audioController;

    private static readonly int wheelSegment = 18;
    internal bool isBonusFinished = true;
    internal bool wheelButtonPressed;
    private Tween wheelWinTextTween;
    private Tween lineMultiplierTextTween;
    private Tween TotalWinTextTween;

    private void Start()
    {
        ResetWheels();
        //StartCoroutine(RotateWheel(19, SingleBonusWheel));
        if (startPanelButton) startPanelButton.onClick.AddListener(() =>
        {
            WheelStartPanel.SetActive(false);
            uIManager.SetSpinButtonInteractable(true);
        });
    }

    internal void IntializeBonusWheelValue()
    {
        // for (int i = 0; i < singleBonusWheelSegmentTexts.Count; i++)
        // {
        //     string sliceValue = socketManager.features.cashSpinnerBonus.wheelSlices[i].ToString();

        //     if (sliceValue != "STAR_ARROW_SLICE")
        //     {
        //         // Converts "1000" into "1\n0\n0\n0"
        //         singleBonusWheelSegmentTexts[i].text = string.Join("\n", sliceValue.ToCharArray());
        //     }
        // }
        // for (int i = 0; i < doubleBonusWheelSegmentTexts.Count; i++)
        // {
        //     string sliceValue = socketManager.features.doubleCashSpinnerBonus.wheelSlices[i].ToString();

        //     //if (sliceValue != "STAR_ARROW_SLICE")
        //     {
        //         // Converts "1000" into "1\n0\n0\n0"
        //         doubleBonusWheelSegmentTexts[i].text = string.Join("\n", sliceValue.ToCharArray());
        //     }
        // }
    }

    internal void BonusWheel()
    {
        StartCoroutine(StartBonusAnimation());
    }

    private IEnumerator StartBonusAnimation()
    {
        BlackBG.alpha = 0f;
        BlackBG.gameObject.SetActive(true);
        BlackBG.DOFade(1f, 0.7f).SetEase(Ease.Linear).OnComplete(() =>
        {
            uIManager.ToggleBonusBackground(true);
            SlotPanel.SetActive(false);
            BonusPanel.SetActive(true);
            uIManager.SetSpinButtonActive(true);
            uIManager.SetStopSpinButtonActive(false);
        });

        audioController.PlayBonusHit(false);
        //bonusWheelAnimation.animName = "wheel_right";
        yield return new WaitForSeconds(0.3f);
        //bonusWheelAnimation.Play(false);
        bonusWheelAnimation.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);

        BlackBG.DOFade(0f, 0.7f).SetEase(Ease.Linear).OnComplete(() =>
        {
            BlackBG.gameObject.SetActive(false);
        });

        yield return new WaitForSeconds(0.5f);

        //bonusWheelAnimation.Stop();
        bonusWheelAnimation.gameObject.SetActive(false);

        //yield return new WaitForSeconds(1f);

        double awardValue = 0f;

        // if (socketManager.resultData.payload.features.cashSpinnerBonus.triggered)
        // {
        //     var rawSlice = socketManager.features.cashSpinnerBonus.wheelSlices[socketManager.resultData.payload.features.cashSpinnerBonus.wheelStopIndex];
        //     if (double.TryParse(rawSlice?.ToString(), out double parsedValue))
        //     {
        //         awardValue = parsedValue;
        //     }
        //     yield return SingleWheel();
        // }

        // if (socketManager.resultData.payload.features.doubleCashSpinnerBonus.triggered)
        // {
        //     awardValue = socketManager.features.doubleCashSpinnerBonus.wheelSlices[socketManager.resultData.payload.features.doubleCashSpinnerBonus.wheelStopIndex];
        //     yield return DoubleWheel();
        // }

        wheelWinText.GetComponent<CanvasGroup>().alpha = 0f;
        lineMultiplierText.GetComponent<CanvasGroup>().alpha = 0f;
        TotalWinText.GetComponent<CanvasGroup>().alpha = 0f;

        audioController.PlayBonusComplete(false);

        WheelEndPanel.SetActive(true);

        wheelWinText.GetComponent<CanvasGroup>().DOFade(1f, 0.7f).SetEase(Ease.Linear);
        double displayAmount = 0f;
        wheelWinTextTween = DOTween.To(() => displayAmount, val =>
        {
            displayAmount = val;
            wheelWinText.text = val.ToString("F2");
        }, awardValue, 1.5f);

        yield return new WaitForSeconds(1.7f);

        // if (socketManager.resultData.payload.features.doubleCashSpinnerBonus.triggered)
        // {
        //     multiplyTwoText.localPosition = new Vector3(400f, -22.515f, 0f);
        //     multiplyTwoText.GetComponent<CanvasGroup>().DOFade(1f, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
        //     {
        //         multiplyTwoText.DOLocalMoveX(100f, 0.7f).SetEase(Ease.Linear);
        //         multiplyTwoText.GetComponent<CanvasGroup>().DOFade(0f, 0.7f).SetEase(Ease.Linear);
        //     });
        //     yield return new WaitForSeconds(0.2f);
        //     displayAmount = awardValue;
        //     wheelWinTextTween = DOTween.To(() => displayAmount, val =>
        //     {
        //         displayAmount = val;
        //         wheelWinText.text = val.ToString("F2");
        //     }, awardValue * 2, 1f);
        //     yield return new WaitForSeconds(0.7f);
        // }

        lineMultiplierText.GetComponent<CanvasGroup>().DOFade(1f, 0.7f).SetEase(Ease.Linear);
        displayAmount = 0f;
        lineMultiplierTextTween = DOTween.To(() => displayAmount, val =>
        {
            displayAmount = val;
            lineMultiplierText.text = val.ToString("F2");
        }, socketManager.initialData.bets[uIManager.betCounter], 1f);

        yield return new WaitForSeconds(1.2f);

        TotalWinText.GetComponent<CanvasGroup>().DOFade(1f, 0.7f).SetEase(Ease.Linear);
        displayAmount = 0f;
        TotalWinTextTween = DOTween.To(() => displayAmount, val =>
        {
            displayAmount = val;
            TotalWinText.text = val.ToString("F2");
        }, awardValue * socketManager.initialData.bets[uIManager.betCounter], 1f);

        yield return new WaitForSeconds(2f);

        WheelEndPanel.SetActive(false);

        BlackBG.alpha = 0f;
        BlackBG.gameObject.SetActive(true);
        BlackBG.DOFade(1f, 0.7f).SetEase(Ease.Linear).OnComplete(() =>
        {
            uIManager.ToggleBonusBackground(false);
            SlotPanel.SetActive(true);
            BonusPanel.SetActive(false);
            uIManager.SetSpinButtonActive(true);
            uIManager.SetStopSpinButtonActive(true);
        });

        audioController.PlayBonusHit(false);
        //bonusWheelAnimation.animName = "wheel_left";
        yield return new WaitForSeconds(0.3f);
        //bonusWheelAnimation.Play(false);
        bonusWheelAnimation.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);

        BlackBG.DOFade(0f, 0.7f).SetEase(Ease.Linear).OnComplete(() =>
        {
            BlackBG.gameObject.SetActive(false);
        });

        yield return new WaitForSeconds(1f);

        //bonusWheelAnimation.Stop();
        bonusWheelAnimation.gameObject.SetActive(false);

        ResetWheels();
        isBonusFinished = true;
    }

    private IEnumerator SingleWheel()
    {
        doubleWheelInfo.SetActive(false);
        singleWheelInfo.SetActive(true);
        WheelStartPanel.SetActive(true);
        wheelButtonPressed = false;

        yield return new WaitUntil(() => wheelButtonPressed);

        //yield return RotateWheel(socketManager.resultData.payload.features.cashSpinnerBonus.wheelStopIndex, SingleBonusWheel);

        BlackWheelOverlayObject.SetActive(true);
        yield return new WaitForSeconds(3f);

    }

    public IEnumerator DoubleWheel()
    {
        singleWheelInfo.SetActive(false);
        doubleWheelInfo.SetActive(true);
        WheelStartPanel.SetActive(true);
        BlackWheelOverlayObject.SetActive(false);
        wheelButtonPressed = false;

        OuterBorder.StopAnimation();
        InnerBorder.StopAnimation();
        Pointer.StopAnimation();
        InnerBorder.rendererDelegate.sprite = InnerBorderBlinkLoop[1];

        yield return new WaitUntil(() => wheelButtonPressed);

        DoubleBonusWheel.gameObject.SetActive(true);
        DoubleBonusWheel.DOScale(1f, 1f).SetEase(Ease.Linear);
        DoubleBonusWheel.DOLocalRotate(new Vector3(0f, 0f, 0f), 1f).SetEase(Ease.Linear);

        InnerWheeltext.DOFade(1f, 0.5f).SetLoops(7, LoopType.Yoyo).SetEase(Ease.Linear);

        yield return new WaitForSeconds(1.5f);

        //yield return RotateWheel(socketManager.resultData.payload.features.doubleCashSpinnerBonus.wheelStopIndex, DoubleBonusWheel);

        audioController.PlayWheelBlackOverlay(false);
        BlackWheelOverlayObject.SetActive(true);
        yield return new WaitForSeconds(3f);

    }

    internal IEnumerator RotateWheel(int stopIndex, RectTransform wheel)
    {
        OuterBorder.textureArray = OuterBorderOnebyOneBlinkLoop;
        OuterBorder.AnimationSpeed = 7f;
        OuterBorder.doLoopAnimation = true;
        OuterBorder.StartAnimation();

        InnerBorder.textureArray = InnerBorderOnebyOneBlinkLoop;
        InnerBorder.AnimationSpeed = 4f;
        InnerBorder.doLoopAnimation = true;
        InnerBorder.StartAnimation();

        // Reset wheel angle before calculating the next spin target
        wheel.localEulerAngles = Vector3.zero;

        float wheelStopAngle = -(rotation * 360f) + (stopIndex * wheelSegment);
        float tempAngle = -(1 * 360f) + (stopIndex * wheelSegment);
        float finalangle = stopIndex * wheelSegment + 9f;

        float looptime = 14f;              // constant-speed duration
        float decelTime = time - looptime; // remaining time to decelerate + stop

        if (decelTime <= 0f)
        {
            Debug.LogWarning("BonusManager: 'time' must be greater than 'looptime' so there's room to decelerate.");
            decelTime = 0.01f;
        }

        float phase1Angle = wheelStopAngle / (1f + decelTime / (2f * looptime));

        Sequence spinSequence = DOTween.Sequence();

        audioController.PlayWheelArrowTick(true);
        // --- ARROW ANIMATION TIMELINE ---
        spinSequence.InsertCallback(0.5f, () =>
        {
            // Create a sub-sequence dedicated to the arrow
            Sequence arrowSeq = DOTween.Sequence();

            // 1. Fast back-and-forth loop (0° to -20° to 0°)
            // Set loops to run until 15s mark (14.5s total duration with 0.15s per cycle = ~96 half-swings)
            arrowSeq.Append(
                ArrowObject.DOLocalRotate(new Vector3(0f, 0f, 20f), 0.11f)
                           .SetEase(Ease.Linear)
                           .SetLoops(150, LoopType.Yoyo)
            );

            // 2. Slowing down back-and-forth movement for 2 seconds (15s to 17s mark)
            // First back-and-forth swing (1.2s total) - wider, slower
            arrowSeq.Append(ArrowObject.DOLocalRotate(new Vector3(0f, 0f, 15f), 0.5f).SetEase(Ease.OutQuad));
            arrowSeq.Append(ArrowObject.DOLocalRotate(new Vector3(0f, 0f, 0f), 0.5f).SetEase(Ease.InOutQuad));

            // Second back-and-forth swing (0.8s total) - smaller, settling back to 0°
            arrowSeq.Append(ArrowObject.DOLocalRotate(new Vector3(0f, 0f, 5f), 0.3f).SetEase(Ease.OutQuad));
            arrowSeq.Append(ArrowObject.DOLocalRotate(new Vector3(0f, 0f, 0f), 0.3f).SetEase(Ease.OutSine));

            audioController.PlayWheelArrowStop(false);
        });

        // Phase 1: constant speed for exactly `looptime` seconds
        spinSequence.Append(
            wheel.DOLocalRotate(new Vector3(0f, 0f, phase1Angle), looptime, RotateMode.FastBeyond360)
                 .SetEase(Ease.Linear)
        );

        // Zooming & Positioning at 10s
        spinSequence.InsertCallback(10f, () =>
        {
            MainWheelParent.DOScale(1.7f, 2f).SetEase(Ease.Linear);
            MainWheelParent.DOLocalMoveY(-450f, 2f).SetEase(Ease.Linear);
        });

        // Phase 2: Deceleration phase
        spinSequence.Append(
            wheel.DOLocalRotate(new Vector3(0f, 0f, tempAngle), decelTime, RotateMode.FastBeyond360)
                 .SetEase(Ease.OutQuad)
        );

        spinSequence.OnComplete(() =>
        {
            wheel.DOLocalRotate(new Vector3(0f, 0f, finalangle), 1f, RotateMode.Fast).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                MainWheelParent.DOScale(1f, 1f).SetEase(Ease.Linear);
                MainWheelParent.DOLocalMoveY(0f, 1f).SetEase(Ease.Linear);
            });
            Pointer.textureArray = PointerBlinkLoop;
            Pointer.AnimationSpeed = 0.3f;
            Pointer.doLoopAnimation = true;
            Pointer.StartAnimation();

            OuterBorder.textureArray = OuterBorderBlinkLoop;
            OuterBorder.AnimationSpeed = 0.3f;
            OuterBorder.doLoopAnimation = true;
            OuterBorder.StartAnimation();

            InnerBorder.textureArray = InnerBorderBlinkLoop;
            InnerBorder.AnimationSpeed = 0.3f;
            InnerBorder.doLoopAnimation = true;
            InnerBorder.StartAnimation();
        });
        yield return new WaitForSeconds(19.5f);

        //yield return new WaitUntil(() => spinSequence.active);
    }

    private void ResetWheels()
    {
        OuterBorder.StopAnimation();
        Pointer.StopAnimation();
        InnerBorder.StopAnimation();
        InnerBorder.rendererDelegate.sprite = InnerBorderBlinkLoop[1];

        InnerWheeltext.alpha = 0f;

        BlackWheelOverlayObject.SetActive(false);
        SingleBonusWheel.localEulerAngles = Vector3.zero;
        DoubleBonusWheel.localEulerAngles = Vector3.zero;

        DoubleBonusWheel.localEulerAngles = new Vector3(0f, 0f, 100f);
        DoubleBonusWheel.localScale = Vector3.zero;
        DoubleBonusWheel.gameObject.SetActive(false);
    }
}