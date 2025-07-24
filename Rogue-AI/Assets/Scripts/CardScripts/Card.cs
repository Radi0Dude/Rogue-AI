using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Card : MonoBehaviour
{
    [Header("Attached Scripts")]
    [SerializeField] private CardVisual cardVisual;
    [SerializeField] private CardInteraction cardInteraction;
    [SerializeField] private TooltipText cardTooltip;
    [SerializeField] private AbilityTooltipText cardAbilityTooltip;
 
    public event Action<Card> OnRewardSelected;
    public event Action<Card> OnCardPlayed;
    
    private CardData cardData;
    
    public void SetUp(CardData data)
    {
        cardData = data;
        cardVisual.UpdateCardVisuals(data);
        cardTooltip.SetTooltipText(data.CardDescription);
        SetAbilityTooltipText(data.GetCardTypes());
        cardInteraction.OnCardPressed += CardPressed;
        cardInteraction.OnRewardSelected += RewardSelected;
        // TODO: Use card Rarity to change model colour 
    }

    private void SetAbilityTooltipText(List<CardType> cardTypes)
    {
        string tooltipText = "";
        
        foreach (var cardType in cardTypes)
        {
            switch (cardType)
            {
                case CardType.Discard:
                    tooltipText += $"<color=yellow>{cardType}</color> " + cardData.CardsToDiscard + " card(s)\n";
                    break;
                case CardType.Draw:
                    tooltipText += $"<color=yellow>{cardType}</color> " + cardData.CardsToDraw + " card(s)\n";
                    break;
                case CardType.Delete:
                    tooltipText += $"<color=yellow>{cardType}</color> " + cardData.CardsToDelete + " card(s)\n";
                    break;
                case CardType.Prompt:
                    tooltipText += "Prompt term(s): ";
                    foreach (PromptType promptType in cardData.GetPromptTypes())
                    {
                        tooltipText += promptType + " ";
                    }
                    tooltipText += "\n";
                    break;
                case CardType.Status:
                    tooltipText += $"<color=purple>{cardType}</color> ";
                    break;
            }
        }
        
        cardAbilityTooltip.SetAbilityTooltipText(tooltipText);
    }

    private void CardPressed()
    {
        if (GameManager.CanPlayCard)
        {
            OnCardPlayed?.Invoke(this);
        }
    }

    public void PlayCard()
    {
        OnCardPlayed?.Invoke(this);
    }
    
    private void RewardSelected()
    {
        OnRewardSelected?.Invoke(this);
    }

    public CardData GetData()
    {
        return cardData;
    }
    
    
}
