using UnityEngine;

public class AbilityTooltipText : MonoBehaviour
{
    private string _tooltipText = null;

    private void OnMouseEnter()
    {
        if (_tooltipText == null)
        {
            return;
        }
        
        TooltipManager.Instance.SetAndShowAbilityTooltip(_tooltipText);
    }

    private void OnMouseExit()
    {
        TooltipManager.Instance.HideAbilityTooltip();
    }

    public void SetAbilityTooltipText(string tooltipText)
    {
        _tooltipText = tooltipText;
    }
}
