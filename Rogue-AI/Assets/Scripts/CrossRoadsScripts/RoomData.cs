using UnityEngine;

public enum RoomType
{
    Combat,
    Treasure,
    Event,
    RestSite,
    Sign
}

public abstract class RoomData : ScriptableObject
{
    public abstract RoomType RoomType { get; }
    public abstract string SceneNameToLoad { get; }

    public string roomName;
    public Sprite roomIcon;
    
    
}
