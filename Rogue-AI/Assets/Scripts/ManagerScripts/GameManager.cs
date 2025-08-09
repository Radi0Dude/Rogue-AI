using System;
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
    
    // Combat Variable
    public static RoundState RoundState;

    
    // Player Variables
    private static int _playerMaxHealth = 100;
    private static int _playerHealth = PlayerMaxHealth;
    
    public static event Action OnHealthChanged;
    public static event Action OnGameLost;
    
    public static CardCollectionData PlayerCardCollection;

    
    public static int PlayerMaxHealth
    {
        get { return _playerMaxHealth; }
        set
        {
            if (_playerMaxHealth == value) return;
            
            _playerMaxHealth = value;
            OnHealthChanged?.Invoke();
        }
    }

    public static int PlayerHealth
    {
        get => _playerHealth;
        set
        {
            if (_playerHealth == value) return;
            
            _playerHealth = value;
            OnHealthChanged?.Invoke();
        }
    }
    
    
    private static List<RoomData> _rooms = new ();
    
    public static void AddRoomsToList(List<RoomData> addedRooms)
    {
        foreach (var newRoom in addedRooms)
        {
            _rooms.Add(newRoom);
        }
    }

    public static void ClearRoomList()
    {
        _rooms.Clear();
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

    public static void RemoveRoomFromListAndLoadNextScene()
    {
        _rooms.RemoveAt(0);
        LoadNextScene();
    }

    public static void LoadNextScene()
    {
        // Load scene in room else load the final scene
        SceneManager.LoadScene(_rooms[0] != null ? _rooms[0].SceneNameToLoad : "6_EndMenu");
    }


    public static void GameOver()
    {
        Debug.Log("Game Over, You Lost The Game");
        OnGameLost?.Invoke();
    }
    
}
