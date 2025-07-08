using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Card : MonoBehaviour
{
    private CardData cardData;
    private CombatManager _combatManager;
    [Header("Attached Scripts")]
    [SerializeField] private CardVisual cardVisual;
    [SerializeField] private CardInteractionManager cardInteractionManager;
    

    public void SetUp(CardData data)
    {
        cardData = data;
        cardVisual.UpdateCardVisuals(data);
        cardInteractionManager.Init(data);
        cardInteractionManager.OnCardPressed += PlayCard;
        _combatManager = FindAnyObjectByType<CombatManager>();
    }

    private void PlayCard()
    {
        if (GameManager.CanPlayCard)
        {
            _combatManager.PlayCard(this);
        }
    }

    public CardData GetData()
    {
        return cardData;
    }
    
    
}
