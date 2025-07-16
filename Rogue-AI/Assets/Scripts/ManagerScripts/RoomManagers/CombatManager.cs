using System;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    
    private InputField _inputField;
    private CardReward _cardReward;
    private Player _player;
    private AI _ai;
    private Deck _deck;


    private void Awake()
    {
        BeginCombat();
    }

    private void BeginCombat()
    {
        _inputField = FindAnyObjectByType<InputField>();
        _cardReward = FindAnyObjectByType<CardReward>();
        _player = FindAnyObjectByType<Player>();
        _ai = FindAnyObjectByType<AI>();
        _deck = FindAnyObjectByType<Deck>();


   
        _inputField.OnEndingTurnEvent += EndTurn;
        _inputField.OnSendPromptEvent += SendPrompt;
        _ai.OnAISane += EndCombat;
            
        GameManager.CanPlayCard = true;
        _ai.Initialize(_player);
    }

    private void Start()
    {
        StartTurn();
    }


    private void StartTurn()
    {
        // If there are no prompt present, give a new prompt
        // Player draws card
        _player.DrawHand();
    }
    
    public void PlayCard(Card card)
    {
        if (GameManager.CurrentPlayState == CardPlayState.Delete)
        {
            _deck.DeleteCard(card);
            return;
        }
        
        var cardData = card.GetData();

        List<CardType> cardTypes = cardData.GetCardTypes();

        _deck.DiscardCard(card);

        foreach (CardType cardType in cardTypes)
        {
            if (cardType == CardType.Prompt)
            {
                _inputField.UpdatePrompt(cardData.PromptType);
            }
            else if (cardType == CardType.Draw)
            {
                _deck.DrawHand(cardData.CardsToDraw);
            }
            else if (cardType == CardType.Delete)
            {
                _deck.PlayedDeleteCard(cardData.CardsToDelete);
            }
            else if (cardType == CardType.Virus)
            {
                // Use up space
            }
            else
            {
                Debug.LogError(cardType + " has not been given an action");
            }
        }
    }
    
    private void SendPrompt(List<PromptType> prompts)
    {
        //TODO: Send prompt to the prompt manager, and gain the amount of sanity that should be given to the AI
        Debug.Log("Sending Prompt.");
        int sumSanity = 0;
        foreach (var prompt in prompts)
        {
            sumSanity += 10;
        }
        _ai.ChangeSanity(sumSanity);
        EndTurn();
    }
    
    
    
    private void EndTurn()
    {
        Debug.Log("Ending Turn");
        _player.DiscardAllCards();
        if (!_ai.IsSane())
        {
            EnemyAdvancement();
        }
    }



    private void EnemyAdvancement()
    {
        _ai.ReduceCountDown();
        StartTurn();
    }

    
    private void EndCombat()
    {
        // Reward is presented to the player
        GameManager.CanPlayCard = false;
        Debug.Log("Ending Combat");
        _cardReward.DisplayCardReward();
        // In UI Player can load next scene
        
    }

    public void LoadNextScene()
    {
        GameManager.RemoveRoomFromListAndLoadNextScene();
        
    }
    
    private void LoseCombat()
    {
        // Game Over the player
        
        GameManager.GameOver();
    }

    private void OnDisable()
    {
        _inputField.OnEndingTurnEvent -= EndTurn;
        _inputField.OnSendPromptEvent -= SendPrompt;
        _ai.OnAISane -= EndCombat;
    }

}
