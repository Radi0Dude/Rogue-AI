using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestSiteManager : MonoBehaviour
{
    
    [SerializeField] private int healAmount = 25;
    [SerializeField] private int numbToDelete = 1;
    [Header("Scripts")]
    [SerializeField] private CardLibrary cardLibrary;
    
    
    public void RestButtonPressed()
    {
        AfterButtonPressed();
    }
    

    public void RefactorButtonPressed()
    {
        cardLibrary.StartDeleteCards(numbToDelete, true);
    }
    
    
    private void AfterButtonPressed()
    {
        GameManager.RemoveRoomFromListAndLoadNextScene();
    }
}