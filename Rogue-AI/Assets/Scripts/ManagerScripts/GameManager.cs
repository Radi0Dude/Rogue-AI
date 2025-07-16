using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum CardPlayState
{
    Play,
    Delete
}

public static class GameManager
{
    // Card/Hand related Variables
    public const int MaxHandSize = 6;
    public static int StartOfRoundDraw = 4;
    public static CardPlayState CurrentPlayState;
    public static bool CanPlayCard = false;
    
    
    // Player Variables
    public static int PlayerMaxHealth = 100;
    public static int PlayerHealth = PlayerMaxHealth;
    public static CardCollectionData PlayerCardCollection;
    
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

    public static void ChangePlayerHealth(int changeValue)
    {
        PlayerHealth += changeValue;
        if (PlayerHealth <= 0)
        {
            Debug.Log("Player Died");
            GameOver();
        }
        else if (PlayerHealth > PlayerMaxHealth)
        {
            PlayerHealth = PlayerMaxHealth;
        }
    }

    public static void ChangeMaxHealth(int changeValue)
    {
        PlayerMaxHealth += changeValue;
        // If max health increases also heal the player, if it decreases do not decrease health,
        // but check if current health exceeds mac health
        if (changeValue < 0) 
            changeValue = 0; 
        ChangePlayerHealth(changeValue);
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
