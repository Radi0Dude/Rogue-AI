using System;
using UnityEngine;

public class Card : MonoBehaviour
{
    private CardData cardData;
    [Header("Attached Scripts")]
    [SerializeField] private CardVisual cardVisual;
    [SerializeField] private CardHoverManager cardHoverManager;
    

    public void SetUp(CardData data)
    {
        cardData = data;
        cardVisual.UpdateCardVisuals(data);
        cardHoverManager.Init(data);
    }

    public CardData GetData()
    {
        return cardData;
    }
}
