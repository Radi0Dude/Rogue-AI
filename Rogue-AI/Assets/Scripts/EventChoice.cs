using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventChoice : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI choiceDescription;
    
    public event Action<EventStruct>  OnChoiceSelected;

    private EventStruct _eventStruct;

    public void Init(EventStruct eventInfo)
    {
        choiceDescription.text = eventInfo.eventDescription;
        _eventStruct = eventInfo;
    }
    
    
    public void ChoiceButtonPressed()
    {
        OnChoiceSelected?.Invoke(_eventStruct);    
    }
}
