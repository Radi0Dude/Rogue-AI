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
    public abstract RoomType RoomType { get; }

    public string roomName;
    public Sprite roomIcon;
    public string sceneNameToLoad;
    
    
}
