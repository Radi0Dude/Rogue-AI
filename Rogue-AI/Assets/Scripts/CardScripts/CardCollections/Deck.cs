using System;
using System.Collections.Generic;
using System.Globalization;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Deck : MonoBehaviour
{
    public static Deck Instance {get; private set;} //Singleton

    [SerializeField] private SplineControllHand hand;
    
    [SerializeField] private Card cardPrefab;
    
    [SerializeField] private CardCollectionData playerDeck;

    public event Action<int> OnDeletePlayed;


    private List<Card> _deckPile = new ();
    private List<Card> _discardPile = new ();

    [SerializeField] private List<Card> HandCards { get; set; } = new();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        hand = FindAnyObjectByType<SplineControllHand>();
        
        
        CardCollectionData deck = GameManager.PlayerCardCollection;
        if (deck)
        {
            if (deck.CardsInCollection.Count > 0)
            {
                playerDeck = deck;
            }
        }
        
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

    public void DrawHand(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (HandCards.Count > GameManager.MaxHandSize)
            { continue; }
            // Check if shuffle is necessary
            if (_deckPile.Count <= 0)
            {
                 _deckPile.AddRange(_discardPile);
                _discardPile.Clear();
                ShuffleDeck();
            }
            // Draw card
            if (_deckPile.Count > 0)
            {
                HandCards.Add(_deckPile[0]);
                _deckPile[0].transform.position = transform.position;
                _deckPile[0].gameObject.SetActive(true);
                _deckPile.RemoveAt(0);
                UpdateCardPositionsHand();
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
            UpdateCardPositionsHand();
        }
    }

    public void DeleteCard(Card card)
    {
        if (HandCards.Contains(card))
        {
            card.gameObject.SetActive(false);
            HandCards.Remove(card);
            Destroy(card.gameObject);
            UpdateCardPositionsHand();
        }
    }

    public void PlayedDeleteCard(int numbToDelete)
    {
        if (HandCards.Count < numbToDelete)
        {
            numbToDelete = HandCards.Count;
            if (numbToDelete > 0)
                OnDeletePlayed?.Invoke(numbToDelete); 
        }
        else
            OnDeletePlayed?.Invoke(numbToDelete);
    }

    public void DiscardAllCards()
    {
        for (int i = HandCards.Count - 1; i >= 0; i--)
        {
            DiscardCard(HandCards[i]);
        }
    }

    public void UpdateCardPositionsHand()
    {
        hand.UpdateCardPositions(HandCards);
    }
    
}
