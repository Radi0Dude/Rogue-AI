using System;
using UnityEngine;



[Serializable]
public struct EventStruct
{
    [TextArea] public string eventDescription;
    [TextArea] public string eventEffectTooltip;
    public EventResults[] eventResults;
    
}
