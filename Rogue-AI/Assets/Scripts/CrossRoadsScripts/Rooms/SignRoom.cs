using UnityEngine;

[CreateAssetMenu(fileName = "SignRoom", menuName = "CrossRoad/Rooms/Sign Room")]
public class SignRoom : RoomData
{
    public override RoomType RoomType  => RoomType.Sign;
    public override string SceneNameToLoad => "1_PathTestScene";

    [Header("Sign Room Settings")]
    public int numberOfSigns = 3;
    public int placesToGo = 3;
    public SignRoom nextSignRoom;
}
