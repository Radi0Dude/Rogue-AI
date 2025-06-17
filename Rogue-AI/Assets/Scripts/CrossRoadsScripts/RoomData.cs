using UnityEngine;

public enum RoomType
{
    Combat,
    Treasure,
    Event,
    RestSite
}

public abstract class RoomData : ScriptableObject
{
    public string roomName;
    public abstract RoomType roomType { get; }
    public Sprite roomIcon;
    
    [Header("Room Settings")]
    public RoomData[] rooms;
    
    
}
