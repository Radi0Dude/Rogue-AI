using UnityEngine;

public enum EventResults
{
    ChangeHealth,
    ChangeMaxHealth,
    ChangeDrawAmount,
    AddCard,
    RemoveCard,
    
}

[CreateAssetMenu(fileName = "EventRoom", menuName = "CrossRoad/Rooms/Event Room")]
public class EventRoom : RoomData
{
    public override RoomType RoomType  => RoomType.Event;
    public override string SceneNameToLoad => "5_EventScene";


    [SerializeField] private int numberOfOptions;
    [SerializeField] private string[] optionDescriptions;
    
    
}
