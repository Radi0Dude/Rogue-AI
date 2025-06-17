using UnityEngine;

[CreateAssetMenu(fileName = "CombatRoom", menuName = "CrossRoad/Rooms/Combat Room")]
public class CombatRoom : RoomData
{
    public override RoomType RoomType  => RoomType.Combat;
    
    [Header("Combat Room Settings")]
    public AIData aiData;
}
