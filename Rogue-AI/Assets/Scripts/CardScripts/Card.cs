using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Card : MonoBehaviour
{
    [Header("Attached Scripts")]
    [SerializeField] private CardVisual cardVisual;
    [SerializeField] private CardInteraction cardInteraction;
    [SerializeField] private TooltipText cardTooltip;
 
    public event Action<Card> OnRewardSelected;
    public event Action<Card> OnCardPlayed;
    
    private CardData cardData;
    
    public void SetUp(CardData data)
    {
        cardData = data;
        cardVisual.UpdateCardVisuals(data);
        cardTooltip.SetTooltipText(data.CardDescription);
        cardInteraction.OnCardPressed += CardPressed;
        cardInteraction.OnRewardSelected += RewardSelected;
        // TODO: Use card Rarity to change model colour 
    }
    
    private void CardPressed()
    {
        if (GameManager.CanPlayCard)
        {
            OnCardPlayed?.Invoke(this);
        }
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
