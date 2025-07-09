using UnityEngine;

[CreateAssetMenu(fileName = "TreasureRoom", menuName = "CrossRoad/Rooms/Treasure Room")]
public class TreasureRoom : RoomData
{
    public override RoomType RoomType  => RoomType.Treasure;
    public override string SceneNameToLoad => "4_TreasureScene";

    [Header("Treasure Room Settings")]
    [SerializeField] private RewardType rewardType;
    [SerializeField] private Sprite rewardSprite;
    [Separator(5, 40)] 
    [SerializeField] private string rewardName;
    [TextArea(5, 20)]
    [SerializeField] private string rewardDescription;
    public RewardType RewardType => rewardType;
    public Sprite RewardSprite => rewardSprite;
    public string RewardName => rewardName;
    public string RewardDescription => rewardDescription;

}
