using UnityEngine;

[CreateAssetMenu(fileName = "SignRoom", menuName = "CrossRoad/Rooms/Sign Room")]
public class SignRoom : RoomData
{
    public override RoomType RoomType  => RoomType.Sign;
    public override string SceneNameToLoad => "1_PathTestScene";

    [Header("Sign Room Settings")]
    [SerializeField] private int numberOfSigns = 3;
    [SerializeField] private int placesToGo = 3;
    [SerializeField] private SignRoom nextSignRoom;

    public int NumberOfSigns => numberOfSigns;
    public int PlacesToGo => placesToGo;
    public SignRoom NextSignRoom => nextSignRoom;

}
