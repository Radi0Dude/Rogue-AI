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

    [SerializeField] private string roomName;
    [SerializeField] private Sprite roomIcon;

    public string RoomName => roomName;
    public Sprite RoomIcon => roomIcon;
    
    
}
