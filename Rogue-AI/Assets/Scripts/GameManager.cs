using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameManager
{
    // Card/Hand related Variables
    public const int MaxHandSize = 10;
    public static int StartOfRoundDraw = 5;
    
    
    // Player Variables
    public static int PlayerMaxHealth = 100;

    private static List<RoomData> _rooms = new ();

    public static void AddRoomsToList(List<RoomData> addedRooms)
    {
        foreach (var newRoom in addedRooms)
        {
            _rooms.Add(newRoom);
        }
        LoadNextRoom();
    }

    public static RoomData GetRoom()
    {
        return _rooms[0];
    }
    
    
    private static void LoadNextRoom()
    {
        SceneManager.LoadScene(_rooms[0].sceneNameToLoad);
    }
    

    private static void RemoveRoomFromList(RoomData room)
    {
        _rooms.Remove(room);
    }
    
    
    
}
