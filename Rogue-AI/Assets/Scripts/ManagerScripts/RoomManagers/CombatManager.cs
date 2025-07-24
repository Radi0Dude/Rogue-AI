using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CombatManager : MonoBehaviour
{
    [Header("Scripts from Scene")]
    [SerializeField] private InputField inputField;
    [SerializeField] private CardReward cardReward;
    [SerializeField] private PlayerVisual player;
    [SerializeField] private AI ai;
    [SerializeField] private Deck deck;

    
    private void Awake()
    {
        BeginCombat();
    }

    private void BeginCombat()
    {
        inputField.OnEndingTurnEvent += EndTurn;
        inputField.OnSendPromptEvent += SendPrompt;
        ai.OnAISane += EndCombat;
        deck.OnCardPlayed += PlayCard;
            
        GameManager.CanPlayCard = true;

        var room = (CombatRoom)GameManager.GetRoom();
        
        ai.Initialize(this, room.AIData);
    }

    private void Start()
    {
        StartTurn();
    }


    private void StartTurn()
    {
        // If there are no prompt present, give a new prompt
        // Player draws card
        deck.DrawHand(GameManager.StartOfRoundDraw);
    }
    
    private void PlayCard(Card card)
    {
        if (GameManager.CurrentPlayState == CardPlayState.Delete)
        {
            Debug.LogWarning("Deleting card");
            deck.DeleteCard(card);
            return;
        }
        
        var cardData = card.GetData();

        List<CardType> cardTypes = cardData.GetCardTypes();

        

        foreach (CardType cardType in cardTypes)
        {
            if (cardType == CardType.Prompt)
            {
                inputField.UpdatePrompt(cardData.PromptType);
            }
            else if (cardType == CardType.Draw)
            {
                deck.DrawHand(cardData.CardsToDraw);
            }
            else if (cardType == CardType.Delete)
            {
                deck.PlayedDeleteCard(cardData.CardsToDelete);
            }
            else if (cardType == CardType.Discard)
            {
                deck.DiscardRandomCards(cardData.CardsToDiscard);
            }
            else if (cardType == CardType.Status)
            {
                if (!cardData.IsPlayable)
                {
                    return;
                }
            }
            else
            {
                Debug.LogError(cardType + " has not been given an action");
            }
        }
        deck.DiscardCard(card);
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
        ai.ChangeSanity(sumSanity);
        EndTurn();
    }
    
    
    
    private void EndTurn()
    {
        deck.DiscardAllCards();
        if (!ai.IsSane())
        {
            EnemyAdvancement();
        }
    }



    private void EnemyAdvancement()
    {
        ai.ReduceCountDown();
        StartTurn();
    }
    
    private void EndCombat()
    {
        // Reward is presented to the player
        GameManager.CanPlayCard = false;
        Debug.Log("Ending Combat");
        cardReward.DisplayCardReward();
        // In UI Player can load next scene
    }
    public void ChangeHealth(int value)
    {
        GameManager.ChangePlayerHealth(value);
    }
    public void AddCardToCombat(CardData cardData)
    {
        deck.AddCardToDeck(cardData, true);
    }
    
    private void LoseCombat()
    {
        // Game Over the player
        
        GameManager.GameOver();
    }

    private void OnDisable()
    {
        inputField.OnEndingTurnEvent -= EndTurn;
        inputField.OnSendPromptEvent -= SendPrompt;
        ai.OnAISane -= EndCombat;
    }

}
