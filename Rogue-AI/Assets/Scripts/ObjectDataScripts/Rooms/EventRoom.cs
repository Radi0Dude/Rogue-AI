using System;
using UnityEngine;

[Flags]
public enum EventResults
{
    ChangeHealth = 1 << 0,
    ChangeMaxHealth = 1 << 1,
    ChangeDrawAmount = 1 << 2,
    AddCard = 1 << 3,
    RemoveCard = 1 << 4,
    
}

[CreateAssetMenu(fileName = "EventRoom", menuName = "CrossRoad/Rooms/Event Room")]
public class EventRoom : RoomData
{
    public override RoomType RoomType  => RoomType.Event;
    public override string SceneNameToLoad => "5_EventScene";

    [Header("Event Room Variables")]
    [SerializeField] private string eventDescription;
    [SerializeField] private EventStruct[] optionDescriptions;

    
    
    public string EventDescription => eventDescription;
    public EventStruct[] OptionDescriptions => optionDescriptions;

}
