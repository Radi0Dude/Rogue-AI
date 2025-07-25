using System;
using System.Collections.Generic;
using UnityEngine;

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

    private void SetAbilityTooltipText(List<CardType> cardTypesEnum)
    {
        string tooltipText = "";
        
        var cardTypes = new HashSet<CardType>(cardTypesEnum);
        
        if (cardTypes.Contains(CardType.Prompt))
        {
            tooltipText += "Prompt term(s): ";
            foreach (PromptType promptType in cardData.GetPromptTypes())
            {
                tooltipText += promptType + " ";
            }
            tooltipText += "\n";
        }
        
        if (cardTypes.Contains(CardType.Discard))
        {
            tooltipText += $"<color=yellow>{CardType.Discard}</color> " + cardData.CardsToDiscard + " card(s)\n";
        }

        if (cardTypes.Contains(CardType.Draw))
        {
            tooltipText += $"<color=yellow>{CardType.Draw}</color> " + cardData.CardsToDraw + " card(s)\n";
        }

        if (cardTypes.Contains(CardType.Delete))
        {
            tooltipText += $"<color=yellow>{CardType.Delete}</color> " + cardData.CardsToDelete + " card(s)\n";
        }

        

        if (cardTypes.Contains(CardType.Status))
        {
            tooltipText += $"<color=purple>{CardType.Status}</color> ";
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
