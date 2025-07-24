using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AIVisual : MonoBehaviour
{
    [Header("Sanity Bar")] 
    [SerializeField] private Canvas canvas;
    [SerializeField] private Image sanityBar;
    [SerializeField] private TooltipText sanityBarTooltip;

    [Header("Action Indicator")] 
    [SerializeField] private Image mainActionIndicator;
    [SerializeField] private TextMeshProUGUI mainActionCountdownText;
    [SerializeField] private TooltipText mainActionIndicatorTooltip;
    [SerializeField] private Image endOfTurnActionIndicator;
    [SerializeField] private TooltipText endOfTurnActionIndicatorTooltip;


    public void HideCanvas()
    {
        canvas.enabled = false;
    }
    public void UpdateSanityBar(float current, float max)
    {
        // Calculate Percentage
        var percentageInDecimal = current / max;
        var percentage = percentageInDecimal * 100f;

        // Update SanityBar
        DOTween.To(()=> sanityBar.fillAmount, x=> sanityBar.fillAmount = x, percentageInDecimal, 1f);

        // Update SanityTextPercentage
        if (sanityBarTooltip != null) 
            sanityBarTooltip.SetTooltipText("Sanity level: " + percentage.ToString("0.0") + "%");
    }

    public void SetMainAction(AIActionData data)
    {
        mainActionIndicator.sprite = data.ActionIcon;
        mainActionIndicator.color = data.BarColor;
        mainActionIndicatorTooltip.SetTooltipText(data.ActionDescription);
        SetMainCountdown(data.RoundsUntilAction);
    }
    
    public void SetEndOfTurnAction(AIActionData data)
    {
        endOfTurnActionIndicator.gameObject.SetActive(true);
        endOfTurnActionIndicator.sprite = data.ActionIcon;
        mainActionIndicator.color = data.BarColor;
        endOfTurnActionIndicatorTooltip.SetTooltipText(data.ActionDescription);

    }

    public void SetMainCountdown(int roundsLeft)
    {
        mainActionCountdownText.text = roundsLeft.ToString();
    }

    
}
