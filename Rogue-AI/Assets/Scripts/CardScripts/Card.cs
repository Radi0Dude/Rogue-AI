using System;
using UnityEngine;

public class Card : MonoBehaviour
{
    [Header("Attached Scripts")]
    [SerializeField] private CardVisual cardVisual;
    [SerializeField] private CardInteraction cardInteraction;
    [SerializeField] private TooltipText cardTooltip;
 
    public event Action<Card> OnRewardSelected;
    public event Action<Card> OnCardPlayed;
    
    private CardData _cardData;
    
    public void SetUp(CardData data)
    {
        _cardData = data;
        cardVisual.UpdateCardVisuals(_cardData);
        cardTooltip.SetTooltipText(_cardData.CardDescription);
        cardInteraction.OnCardPressed += CardPressed;
        cardInteraction.OnRewardSelected += RewardSelected;
        // TODO: Use card Rarity to change model colour 
    }

    private void CardPressed()
    {
        OnCardPlayed?.Invoke(this);
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
        return _cardData;
    }
    
    
    
}
