using UnityEngine;

[CreateAssetMenu(fileName = "SignRoom", menuName = "CrossRoad/Rooms/Sign Room")]
public class SignRoom : RoomData
{
    public override RoomType RoomType  => RoomType.Sign;
    
}
