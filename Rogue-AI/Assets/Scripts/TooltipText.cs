using UnityEngine;

public class TooltipText : MonoBehaviour
{
    private string _tooltipText = null;

    private void OnMouseEnter()
    {
        if (_tooltipText == null)
        {
            _tooltipText = "Debugging";
        }
        
        TooltipManager.Instance.SetAndShowTooltip(_tooltipText);
    }

    private void OnMouseExit()
    {
        TooltipManager.Instance.HideTooltip();
    }

    public void SetTooltipText(string tooltipText)
    {
        _tooltipText = tooltipText;
    }
}
