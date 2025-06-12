using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Deck : MonoBehaviour
{
    public static Deck Instance {get; private set;} //Singleton

    [SerializeField] private CardCollection playerDeck;
    [SerializeField] private Card cardPrefab;

    public List<Card> _deckPile = new ();
    public List<Card> _discardPile = new ();

    [SerializeField] public List<Card> HandCards { get; private set; } = new();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
    }

    private void Start()
    {
        InstantiateDeck();
    }

    private void InstantiateDeck()
    {
        for (int i = 0; i < playerDeck.CardsInCollection.Count; i++)
        {
            Card card = Instantiate(cardPrefab, transform.position + new Vector3(i,0,0), quaternion.identity); //Add Transform and shit later
            card.SetUp(playerDeck.CardsInCollection[i]);
            _deckPile.Add(card); // All cards starts in the deck
            card.gameObject.SetActive(false); // Will later be activated when needed
        }
        
        ShuffleDeck();
    }

    // Fisher-Yates Shuffle Algorithm
    private void ShuffleDeck()
    {
        for (int i = _deckPile.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (_deckPile[i], _deckPile[j]) = (_deckPile[j], _deckPile[i]);
        }
    }

    public void DrawHand(int amount = 5)
    {
        for (int i = 0; i < amount; i++)
        {
            // Check if shuffle is necessary
            if (_deckPile.Count <= 0)
            {
                 _deckPile.AddRange(_discardPile);
                _discardPile.Clear();
                ShuffleDeck();
            }

            if (_deckPile.Count > 0)
            {
                HandCards.Add(_deckPile[0]);
                _deckPile[0].gameObject.SetActive(true);
                _deckPile.RemoveAt(0);
            }
        }
    }

    public void DiscardCard(Card card)
    {
        if (HandCards.Contains(card))
        {
            card.gameObject.SetActive(false);
            HandCards.Remove(card);
            _discardPile.Add(card);
        }
    }
    
}
