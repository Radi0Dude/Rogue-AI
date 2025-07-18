using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventChoice : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI eventChoiceText;
    [SerializeField] private TooltipText evenChoiceTooltip;
    
    public event Action<EventStruct>  OnChoiceSelected;

    private EventStruct _eventStruct;

    public void Init(EventStruct eventInfo)
    {
        eventChoiceText.text = eventInfo.eventDescription;
        evenChoiceTooltip.SetTooltipText(eventInfo.eventEffectTooltip);
        _eventStruct = eventInfo;
    }
    
    
    public void ChoiceButtonPressed()
    {
        OnChoiceSelected?.Invoke(_eventStruct);    
    }
}
