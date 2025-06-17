using System.Collections.Generic;
using UnityEngine.SceneManagement;

public static class GameManager
{
    // Card/Hand related Variables
    public const int MaxHandSize = 10;
    public static int StartOfRoundDraw = 5;
    
    
    // Player Variables
    public static int PlayerMaxHealth = 100;

    private static List<RoomData> _rooms = new ();

    public static void AddToRoomList(List<RoomData> addedRooms)
    {
        foreach (var newRoom in addedRooms)
        {
            _rooms.Add(newRoom);
        }
    }

    private static void LoadNextRoom()
    {
        SceneManager.LoadScene(_rooms[0].sceneNameToLoad);
    }

    public static void RemoveCompletedRoom()
    {
        _rooms.RemoveAt(0);

        LoadNextRoom();
    }
    
    
}
