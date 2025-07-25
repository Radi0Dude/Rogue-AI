using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
public enum RoundState
{
    BeginCombat,
    StartRound,
    EndRound,
    EndCombat,
}
    


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
        GameManager.RoundState = RoundState.BeginCombat;

        inputField.OnEndingTurnEvent += EndTurn;
        inputField.OnSendPromptEvent += SendPrompt;
        ai.OnAISane += EndCombat;
        deck.OnCardPlayed += PlayCard;
        deck.AIHealthChange += ChangeAIHealth;
        
            
        var room = (CombatRoom)GameManager.GetRoom();
        
        ai.Initialize(this, room.AIData);
    }

    

    private void Start()
    {
        StartTurn();
    }


    private void StartTurn()
    {
        GameManager.RoundState = RoundState.StartRound;
        // If there are no prompt present, give a new prompt
        // Player draws card
        deck.DrawHand(GameManager.StartOfRoundDraw);
    }
    
    private void SendPrompt(List<PromptType> prompts)
    {
        //TODO: Send prompt to the prompt manager, and gain the amount of sanity that should be given to the AI
        int sumSanity = 1;
        foreach (PromptType prompt in prompts)
        {
            if (prompt == PromptType.False)
                sumSanity /= 2;
            else
                sumSanity += sumSanity;
        }
        ai.ChangeSanity(sumSanity);
        EndTurn();
    }
    private void ChangeAIHealth(float percentage)
    {
        ai.LoseSanityByPercentage(percentage);
    }
    
    private void EndTurn()
    {
        GameManager.RoundState = RoundState.EndRound;
        deck.CheckForVirusCardEffects();
        deck.DiscardAllCards();
        if (!ai.IsSane())
        {
            EnemyAdvancement();
        }
    }

    
    private void PlayCard(Card card)
    {
        if (GameManager.CurrentPlayState == CardPlayState.Delete)
        {
            Debug.LogWarning("Deleting card");
            deck.DeleteCard(card);
            return;
        }
        
        var data = card.GetData();

        List<CardType> cardTypesEnum = data.GetCardTypes();

        var cardTypes = new HashSet<CardType>(cardTypesEnum);

        // Check status playability first to stop other cards from being played
        if (cardTypes.Contains(CardType.Status))
        {
            if (GameManager.RoundState == RoundState.StartRound)
            {
                return;
            }
        }
        
        if (cardTypes.Contains(CardType.Prompt))
        {
            inputField.UpdatePrompt(data.PromptType);
        }
        
        if (cardTypes.Contains(CardType.Discard))
        {
            deck.DiscardRandomCards(data.CardsToDiscard);
        }

        if (cardTypes.Contains(CardType.Draw))
        {
            deck.DrawHand(data.CardsToDraw);
        }

        if (cardTypes.Contains(CardType.Delete))
        {
            deck.PlayedDeleteCard(data.CardsToDelete);
        }
        
        deck.DiscardCard(card);
    }
    
   


    private void EnemyAdvancement()
    {
        ai.ReduceCountDown();
        StartTurn();
    }
    
    private void EndCombat()
    {
        GameManager.RoundState = RoundState.EndCombat;
        // Reward is presented to the player
        Debug.Log("Ending Combat");
        cardReward.DisplayCardReward();
        // In UI Player can load next scene
    }
    public void ChangeHealth(int value)
    {
        GameManager.ChangePlayerHealth(value);
    }
    public void AddCardToCombat(CardData data)
    {
        deck.AddCardToDeck(data, true);
    }
    
    private void LoseCombat()
    {
        // Game Over the player
        GameManager.RoundState = RoundState.EndCombat;

        GameManager.GameOver();
    }

    private void OnDisable()
    {
        inputField.OnEndingTurnEvent -= EndTurn;
        inputField.OnSendPromptEvent -= SendPrompt;
        ai.OnAISane -= EndCombat;
        deck.OnCardPlayed -= PlayCard;
        deck.AIHealthChange -= ChangeAIHealth;
    }

}
