using System;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    [SerializeField] private CardLibrary cardLibrary;
    [SerializeField] private VerticalLayoutGroup eventChoiceGroup;
    [SerializeField] private EventChoice eventChoicePrefab;
    
    private EventRoom _data;
    private bool _canLoad = true;
    
    private void Start()
    {
        if (GameManager.GetRoom() is EventRoom room)
        {
            _data = room;
        }
        else
        {
            Debug.LogError("The current room in GameManager is not a TreasureRoom");
            return;
        }
        // Updates visuals to show player
        foreach (var option in _data.OptionDescriptions)
        {
            var choiceInstance = Instantiate(eventChoicePrefab, eventChoiceGroup.transform);
            choiceInstance.transform.SetParent(eventChoiceGroup.transform);
            choiceInstance.Init(option);
            choiceInstance.OnChoiceSelected += EventChoiceSelected;
        }
    }
    
    // When selected complete results
    private void EventChoiceSelected(EventStruct eventStruct)
    {
        
        HandleEventResult(eventStruct);

        if (_canLoad)
        {
            LoadNextScene();
        }
    }    
    
    private void HandleEventResult(EventStruct thisEventStruct)
    {
        EventResults result = thisEventStruct.eventResults;
        
        if (result.HasFlag(EventResults.ChangeHealth))
        {
            GameManager.ChangePlayerHealth(thisEventStruct.currentHealthChange);
        }

        if (result.HasFlag(EventResults.ChangeMaxHealth))
        {
            GameManager.ChangeMaxHealth(thisEventStruct.maxHealthChange);
        }

        if (result.HasFlag(EventResults.ChangeDrawAmount))
        {
            GameManager.StartOfRoundDraw += thisEventStruct.drawAmountChange;
        }

        if (result.HasFlag(EventResults.AddCard))
        {
            GameManager.PlayerCardCollection.AddCardToCollection(thisEventStruct.cardToAdd);
        }

        if (result.HasFlag(EventResults.RemoveCard))
        {
            cardLibrary.StartDeleteCards(thisEventStruct.numberOfCardsToDelete, false);
            _canLoad = false;
        }

    }
    
    // When worked through all results load next scene
    private void LoadNextScene()
    {
        GameManager.RemoveRoomFromListAndLoadNextScene();
    }
}
