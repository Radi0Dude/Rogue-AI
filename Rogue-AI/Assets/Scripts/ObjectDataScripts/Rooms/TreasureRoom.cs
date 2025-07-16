using UnityEngine;

[CreateAssetMenu(fileName = "TreasureRoom", menuName = "CrossRoad/Rooms/Treasure Room")]
public class TreasureRoom : RoomData
{
    public override RoomType RoomType  => RoomType.Treasure;
    public override string SceneNameToLoad => "4_TreasureScene";

    [SerializeField] private RewardType rewardType;
    [SerializeField] private Sprite rewardSprite;
    [SerializeField] private string rewardName;
    [SerializeField] private string rewardDescription;

    [SerializeField] private Card rewardCard;
    
    public RewardType RewardType => rewardType;
    public Sprite RewardSprite => rewardSprite;
    public string RewardName => rewardName;
    public string RewardDescription => rewardDescription;
    public Card RewardCard => rewardCard;

}
