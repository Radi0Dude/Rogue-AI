using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private CardCollectionData startingCardCollection;
    [SerializeField] private SignRoom startingSignRoom;
    public void StartGame()
    {
        // Set Player's Starting Deck
        CardCollectionData newCollection = ScriptableObject.CreateInstance<CardCollectionData>();
        newCollection.InitializeCollection(startingCardCollection.CardsInCollection);
        GameManager.PlayerCardCollection = newCollection;
        
        
        // Reset all player Stats
        GameManager.PlayerMaxHealth = 100;
        GameManager.PlayerHealth = GameManager.PlayerMaxHealth;
        GameManager.StartOfRoundDraw = 4;
        
        // Set starting room
        GameManager.ClearRoomList();
        List<RoomData> signRoom = new List<RoomData>(); 
        signRoom.Add(startingSignRoom);
        GameManager.AddRoomsToList(signRoom);
        
        GameManager.LoadNextScene();
    }

    public void Options()
    {
        // Opens option panel when we have one
    }

    public void Quit()
    {
        Application.Quit();
    }
}
