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
            choiceInstance.transform.parent = eventChoiceGroup.transform;
            choiceInstance.Init(option);
            choiceInstance.OnChoiceSelected += EventChoiceSelected;
        }
    }
    
    // When selected complete results
    private void EventChoiceSelected(EventResults[] eventResults)
    {
        foreach (var result in eventResults)
        {
            HandleEventResult(result);
        }

        if (_canLoad)
        {
            LoadNextScene();
        }
    }    
    
    private void HandleEventResult(EventResults result)
    {
        switch (result)
        {
            case EventResults.ChangeHealth:
                GameManager.ChangePlayerHealth(_data.CurrentHealthChange);
                break;
            case EventResults.ChangeMaxHealth:
                GameManager.ChangeMaxHealth(_data.MaxHealthChange);
                break;
            case EventResults.ChangeDrawAmount:
                GameManager.StartOfRoundDraw += _data.DrawAmountChange;
                break;
            case EventResults.AddCard:
                GameManager.PlayerCardCollection.AddCardToCollection(_data.CardToAdd);
                break;
            case EventResults.RemoveCard:
                cardLibrary.StartDeleteCards(_data.NumberOfCardsToDelete, false);
                _canLoad = false;    
                break;
        }
    }
    
    
    // When worked through all results load next scene
    private void LoadNextScene()
    {
        GameManager.RemoveRoomFromListAndLoadNextScene();
    }
}
