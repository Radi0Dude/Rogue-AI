using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Card : MonoBehaviour
{
    [Header("Attached Scripts")]
    [SerializeField] private CardVisual cardVisual;
    [SerializeField] private CardInteraction cardInteraction;
 
    public event Action<Card> OnRewardSelected;
    
    private CardData cardData;
    private CombatManager _combatManager;
    
    

    
    
    public void SetUp(CardData data)
    {
        cardData = data;
        cardVisual.UpdateCardVisuals(data);
        cardInteraction.Init(data);
        cardInteraction.OnCardPressed += CardPressed;
        cardInteraction.OnRewardSelected += RewardSelected;
        _combatManager = FindAnyObjectByType<CombatManager>();
    }
    
    private void CardPressed()
    {
        if (GameManager.CanPlayCard)
        {
            _combatManager.PlayCard(this);
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
