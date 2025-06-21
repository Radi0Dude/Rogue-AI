using UnityEngine;

[CreateAssetMenu(fileName = "EventRoom", menuName = "CrossRoad/Rooms/Event Room")]
public class EventRoom : RoomData
{
    public override RoomType RoomType  => RoomType.Event;
    public override string SceneNameToLoad => "2_AICombatScene";

}
