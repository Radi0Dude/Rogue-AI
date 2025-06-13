using System;
using UnityEngine;

public class Card : MonoBehaviour
{
    private CardData cardData;
    [Header("Attached Scripts")]
    [SerializeField] private CardVisual cardVisual;
    

    public void SetUp(CardData data)
    {
        cardData = data;
        cardVisual.UpdateCardVisuals(data);
    }

    public CardData GetData()
    {
        return cardData;
    }
}
