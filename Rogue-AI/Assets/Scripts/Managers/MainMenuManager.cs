using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private CardCollection startingCardCollection;
    public void StartGame()
    {
        CardCollection newCollection = ScriptableObject.CreateInstance<CardCollection>();
        newCollection.CardsInCollection = new List<CardData>(startingCardCollection.CardsInCollection);
        GameManager.PlayerCardCollection = newCollection;

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
