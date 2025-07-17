using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventChoice : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI choiceDescription;
    
    public event Action<EventResults[]>  OnChoiceSelected;
    
    private EventResults[] _eventResults;
    private string _tooltipText;

    public void Init(EventStruct eventInfo)
    {
        choiceDescription.text = eventInfo.eventDescription;
        _eventResults = eventInfo.eventResults;
        _tooltipText = eventInfo.eventEffectTooltip;
    }
    
    
    public void ChoiceButtonPressed()
    {
        OnChoiceSelected?.Invoke(_eventResults);    
    }
}
