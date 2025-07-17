using System;
using UnityEngine;



[Serializable]
public struct EventStruct
{
    public string eventDescription;
    public string eventEffectTooltip;
    public EventResults eventResults;
    
    public int currentHealthChange;
    public int maxHealthChange;
    public int drawAmountChange;
    public int numberOfCardsToDelete;
    public CardData cardToAdd;
}
