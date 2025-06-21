using UnityEngine;

[CreateAssetMenu(fileName = "TreasureRoom", menuName = "CrossRoad/Rooms/Treasure Room")]
public class TreasureRoom : RoomData
{
    public override RoomType RoomType  => RoomType.Treasure;
    public override string SceneNameToLoad => "2_AICombatScene";

    
}
