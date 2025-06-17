using UnityEngine;

[CreateAssetMenu(fileName = "CombatRoom", menuName = "CrossRoad/Rooms/Combat Room")]
public class CombatRoom : RoomData
{
    public override RoomType roomType  => RoomType.Combat;
}
