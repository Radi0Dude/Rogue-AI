using UnityEngine;

[CreateAssetMenu(fileName = "Sign", menuName = "CrossRoad/Rooms/Sign Room")]
public class SignRoom : RoomData
{
    public override RoomType RoomType  => RoomType.Sign;
    public override string SceneNameToLoad => "1_PathScene";

    [Header("Sign Room Settings")]
    [SerializeField] private int numberOfSigns = 3;
    [SerializeField] private CrossRoadData crossRoadData;
    [SerializeField] private SignRoom nextSignRoom;
    
    
    public int NumberOfSigns => numberOfSigns;
    public CrossRoadData CrossRoadData => crossRoadData;
    public SignRoom NextSignRoom => nextSignRoom;

}
