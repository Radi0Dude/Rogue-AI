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
    public static CardCollection PlayerCardCollection;
    
    private static List<RoomData> _rooms = new ();
    
    private static bool _isGameStarted = false;
    
    public static void AddRoomsToList(List<RoomData> addedRooms)
    {
        foreach (var newRoom in addedRooms)
        {
            _rooms.Add(newRoom);
        }

        if (_isGameStarted)
        {
            RemoveRoomFromListAndLoadNextScene();
        }
        else
        {
            LoadNextRoom();
            _isGameStarted = true;
        }
    }

    public static RoomData GetRoom()
    {
        return _rooms.Count == 0 ? null : _rooms[0];
    }
    
    
    private static void LoadNextRoom()
    {
        SceneManager.LoadScene(_rooms[0].SceneNameToLoad);
    }
    

    public static void RemoveRoomFromListAndLoadNextScene()
    {
        _rooms.RemoveAt(0);
        LoadNextRoom();
    }


    public static void GameOver()
    {
        Debug.Log("Game Over, You Lost The Game");
    }
    
}
