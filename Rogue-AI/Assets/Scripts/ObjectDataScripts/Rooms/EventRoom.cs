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

    [Header("Event Room Variables")]
    [SerializeField] private string eventRoomName;
    [SerializeField] private string eventDescription;
    [SerializeField] private EventStruct[] optionDescriptions;

    [SerializeField] private int currentHealthChange;
    [SerializeField] private int maxHealthChange;
    [SerializeField] private int drawAmountChange;
    [SerializeField] private CardData cardToAdd;
    [SerializeField] private int numberOfCardsToDelete;
    
    public EventStruct[] OptionDescriptions => optionDescriptions;
    public int CurrentHealthChange => currentHealthChange;
    public int MaxHealthChange => maxHealthChange;
    public int DrawAmountChange => drawAmountChange;
    public CardData CardToAdd => cardToAdd;
    public int NumberOfCardsToDelete => numberOfCardsToDelete;
    

}
