using UnityEngine;

[CreateAssetMenu(fileName = "CombatRoom", menuName = "CrossRoad/Rooms/Combat Room")]
public class CombatRoom : RoomData
{
    public override RoomType RoomType  => RoomType.Combat;
    public override string SceneNameToLoad => "2_AICombatScene";
    
    [Header("Combat Room Settings")]
    [SerializeField] private AIData aiData;
    
    public AIData AIData => aiData;
    
    
}
