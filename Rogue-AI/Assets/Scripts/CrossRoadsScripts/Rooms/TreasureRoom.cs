using UnityEngine;

[CreateAssetMenu(fileName = "TreasureRoom", menuName = "CrossRoad/Rooms/Treasure Room")]
public class TreasureRoom : RoomData
{
    public override RoomType RoomType  => RoomType.Treasure;
    public override string SceneNameToLoad => "4_TreasureScene";

    [Header("Treasure Room Settings")]
    [SerializeField] private RewardType rewardType;
    public RewardType RewardType => rewardType;

}
