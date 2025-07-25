using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private CardCollectionData startingCardCollection;
    public void StartGame()
    {
        CardCollectionData newCollection = ScriptableObject.CreateInstance<CardCollectionData>();
        newCollection.InitializeCollection(startingCardCollection.CardsInCollection);
        GameManager.PlayerCardCollection = newCollection;
        
        // Reset all player Stats
        GameManager.PlayerMaxHealth = 100;
        GameManager.PlayerHealth = GameManager.PlayerMaxHealth;
        GameManager.StartOfRoundDraw = 4;
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
