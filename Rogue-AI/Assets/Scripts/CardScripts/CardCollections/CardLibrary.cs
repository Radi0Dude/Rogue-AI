using System;
using System.Collections.Generic;
using UnityEngine;

public class CardLibrary : MonoBehaviour
{

    [SerializeField] private Card cardPrefab;
    
    private List<Card> _cards = new ();
    private const int AmountToSpawn = 30;

    private void Start()
    {
        // Create Empty Blank cards for display
        InitiateCards();
    }

    public void ViewCardsInList(List<CardData> cardsInCollection)
    {
        for (int i = 0; i < cardsInCollection.Count; i++)
        {
            _cards[i].gameObject.SetActive(true);
            _cards[i].SetUp(cardsInCollection[i]);
            // Fix position
        }
    }

    public void HideCards()
    {
        foreach (var card in _cards)
        {
            card.gameObject.SetActive(false);
        }
    }
    
    private void InitiateCards()
    {
        for (int i = 0; i < AmountToSpawn; i++)
        {
            var cardInstance = Instantiate(cardPrefab);
            _cards.Add(cardInstance);
            cardInstance.gameObject.SetActive(false);
        }
    }
}
