using UnityEngine;

public class TooltipText : MonoBehaviour
{
    private string _tooltipText = null;

    public void SetTooltipText(string tooltipText)
    {
        _tooltipText = tooltipText;
    }
    
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
}
