using UnityEngine;

public class CardHoverManager : MonoBehaviour
{
    private string _tooltipText;

    public void Init(CardData data)
    {
        _tooltipText = data.CardDescription;
    }
    
    private void OnMouseEnter()
    {
        TooltipManager.Instance.SetAndShowTooltip(_tooltipText);
    }

    private void OnMouseExit()
    {
        TooltipManager.Instance.HideTooltip();
    }
}
