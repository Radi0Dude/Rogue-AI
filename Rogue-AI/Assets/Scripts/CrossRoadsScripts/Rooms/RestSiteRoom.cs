using System;
using UnityEngine;

[CreateAssetMenu(fileName = "RestSiteRoom", menuName = "CrossRoad/Rooms/Rest Site")]
public class RestSiteRoom : RoomData
{
    public override RoomType RoomType  => RoomType.RestSite;
    public override string SceneNameToLoad => "3_RestSiteScene";

}
