using UnityEngine;

[CreateAssetMenu(fileName = "SignRoom", menuName = "CrossRoad/Rooms/Sign Room")]
public class SignRoom : RoomData
{
    public override RoomType RoomType  => RoomType.Sign;

    [Header("Sign Room Settings")]
    public int numberOfSigns = 3;
    public int placesToGo = 3;
    public SignRoom nextSignRoom;
}
