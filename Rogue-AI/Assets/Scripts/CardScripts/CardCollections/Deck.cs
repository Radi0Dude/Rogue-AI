using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Deck : MonoBehaviour
{
    [Header("Scriptable Objects")]
    [SerializeField] private CardCollectionData playerDeck;

    [Header("UI Elements")]
    [SerializeField] private Image deleteWarningPanel;

    [Header("Scripts")]
    [SerializeField] private Card cardPrefab;
    [SerializeField] private Hand hand;
    [SerializeField] private DiscardPile discardPile;


    public event Action<Card> OnCardPlayed;


    private int _cardsToDelete;
    private List<Card> _deckPileList = new ();
    private List<Card> _discardPileList = new ();

    [SerializeField] private List<Card> HandCards { get; set; } = new();

    private void Awake()
    {
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
        foreach (var data in playerDeck.CardsInCollection)
        {
            AddCardToDeck(data);
        }
        
        GameManager.CurrentPlayState = CardPlayState.Play;

        ShuffleDeck();
    }

    private void CardPlayed(Card card)
    {
        OnCardPlayed?.Invoke(card);
    }
    
    // Fisher-Yates Shuffle Algorithm
    private void ShuffleDeck()
    {
        for (int i = _deckPileList.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (_deckPileList[i], _deckPileList[j]) = (_deckPileList[j], _deckPileList[i]);
        }
        Debug.Log("Shuffled Deck");
    }

    public void DrawHand(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (HandCards.Count >= GameManager.MaxHandSize)
            { continue; }

            switch (_deckPileList.Count)
            {
                // Check if shuffle is necessary
                case <= 0:
                    _deckPileList.AddRange(_discardPileList);
                    _discardPileList.Clear();
                    ShuffleDeck();
                    break;
                // Draw card
                case > 0:
                    HandCards.Add(_deckPileList[0]);
                    _deckPileList[0].transform.position = transform.position;
                    _deckPileList[0].gameObject.SetActive(true);
                    _deckPileList.RemoveAt(0);
                    UpdateCardPositions();
                    break;
            }
        }
    }

    public void DiscardRandomCards(int amount)
    {
        if (amount > HandCards.Count)
        {
            amount = HandCards.Count;
        }

        for (int i = 0; i < amount; i++)
        {
            Card card= HandCards[Random.Range(0, HandCards.Count)];
            DiscardCard(card);
        }
    }

    public void DiscardCard(Card card)
    {
        if (HandCards.Contains(card))
        {
            HandCards.Remove(card);
            _discardPileList.Add(card);
            UpdateCardPositions();
            discardPile.DiscardCard(card);
        }
    }

    public void DeleteCard(Card card)
    {
        if (HandCards.Contains(card))
        {
            card.gameObject.SetActive(false);
            HandCards.Remove(card);
            Destroy(card.gameObject);
            UpdateCardPositions();
            _cardsToDelete--;
            if (_cardsToDelete <= 0)
            {
                ChangePlayStateToPlay();
            }
        }
    }
    
    private void ChangePlayStateToDelete(int numb)
    {
        // Adds UI 
        deleteWarningPanel.gameObject.SetActive(true);
        
        GameManager.CurrentPlayState = CardPlayState.Delete;
        _cardsToDelete = numb;
    }

    private void ChangePlayStateToPlay()
    {
        deleteWarningPanel.gameObject.SetActive(false);
        
        GameManager.CurrentPlayState = CardPlayState.Play;
    }

    public void PlayedDeleteCard(int numbToDelete)
    {
        if (HandCards.Count < numbToDelete)
        {
            numbToDelete = HandCards.Count;
            if (numbToDelete > 0)
                ChangePlayStateToDelete(numbToDelete);
        }
        else
            ChangePlayStateToDelete(numbToDelete);
    }

    public void DiscardAllCards()
    {
        foreach (Card card in HandCards)
        {
            var data = card.GetData();
            if (data.IsPlayedEndOfTurn && data.GetCardTypes().Contains(CardType.Status))
            {
                // Perform any status card effect if any
                if (data.GetCardTypes().Contains(CardType.Prompt))
                {
                    card.PlayCard();
                }
            }
        }
        for (int i = HandCards.Count - 1; i >= 0; i--)
        {
            DiscardCard(HandCards[i]);
        }
    }

    public void UpdateCardPositions()
    {
        hand.UpdateCardPositions(HandCards);
    }

    public void AddCardToDeck(CardData cardData, bool shouldShuffle = false)
    {
        Card cardInstance = Instantiate(cardPrefab);
        cardInstance.transform.position = transform.position;
        cardInstance.SetUp(cardData);
        cardInstance.OnCardPlayed += CardPlayed;
        cardInstance.transform.localEulerAngles = new Vector3(-100.0f, 0.0f, 0.0f);
        cardInstance.gameObject.SetActive(false);
        _deckPileList.Add(cardInstance);

        if (shouldShuffle)
        {
            ShuffleDeck();
        }
    }
}
