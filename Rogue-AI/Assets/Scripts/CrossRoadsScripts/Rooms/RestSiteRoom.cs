using System;
using UnityEngine;

[CreateAssetMenu(fileName = "RestSiteRoom", menuName = "CrossRoad/Rooms/Rest Site")]
public class RestSiteRoom : RoomData
{
    public override RoomType roomType  => RoomType.RestSite;
}
