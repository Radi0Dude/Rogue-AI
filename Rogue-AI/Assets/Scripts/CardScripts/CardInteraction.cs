using System;
using UnityEngine;

public class CardInteraction : MonoBehaviour
{
    private string _tooltipText = null;

    public event Action OnCardPressed;
    public event Action OnRewardSelected;

    public void Init(CardData data)
    {
        _tooltipText = data.CardDescription;
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

    private void OnMouseDown()
    {
        OnCardPressed?.Invoke();
        OnRewardSelected?.Invoke();
    }
}
