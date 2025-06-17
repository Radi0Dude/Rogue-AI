using UnityEngine;

[CreateAssetMenu(fileName = "EventRoom", menuName = "CrossRoad/Rooms/Event Room")]
public class EventRoom : RoomData
{
    public override RoomType roomType  => RoomType.Event;
}
