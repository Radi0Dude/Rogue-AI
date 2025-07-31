using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
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
    public event Action<float> AIHealthChange;


    private int _cardsToDelete;
    private List<Card> _deckPileList = new ();
    private List<Card> _discardPileList = new ();
    private List<Card> _handCardsList = new();

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
    }

    public void DrawHand(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (_handCardsList.Count > GameManager.MaxHandSize)
            { continue; }
            
            // Check if shuffle is necessary
            if (_deckPileList.Count <= 0)
            {
                _deckPileList.AddRange(_discardPileList);
                _discardPileList.Clear();
                ShuffleDeck();
            }
            
            // Draw card
            if (_deckPileList.Count > 0)
                DrawCard();
        }
    }
    
    private void DrawCard()
    {
        _handCardsList.Add(_deckPileList[0]);
        _deckPileList[0].transform.position = transform.position;
        _deckPileList[0].gameObject.SetActive(true);
        _deckPileList.RemoveAt(0);
        UpdateCardPositions();
    }

    public void DiscardRandomCards(int amount)
    {
        if (amount > _handCardsList.Count)
        {
            amount = _handCardsList.Count;
        }

        for (int i = 0; i < amount; i++)
        {
            Card card= _handCardsList[Random.Range(0, _handCardsList.Count)];
            DiscardCard(card);
        }
    }

    public void DiscardCard(Card card)
    {
        if (_handCardsList.Contains(card))
        {
            _handCardsList.Remove(card);
            _discardPileList.Add(card);
            UpdateCardPositions();
            discardPile.DiscardCard(card);
        }
    }

    public void DeleteCard(Card card)
    {
        if (_handCardsList.Contains(card))
        {
            card.gameObject.SetActive(false);
            _handCardsList.Remove(card);
            Destroy(card.gameObject);
            UpdateCardPositions();
            _cardsToDelete--;
            if (_cardsToDelete <= 0 || _handCardsList.Count == 0)
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
        if (_handCardsList.Count < numbToDelete)
        {
            numbToDelete = _handCardsList.Count;
            if (numbToDelete > 0)
                ChangePlayStateToDelete(numbToDelete);
        }
        else
            ChangePlayStateToDelete(numbToDelete);
    }
    
    public void CheckForVirusCardEffects()
    {
        foreach (Card card in _handCardsList.ToList())
        {
            var data = card.GetData();

            if (!data.GetCardTypes().Contains(CardType.Status)) { continue; }
            
            if (data.IsPlayedEndOfTurn)
            {
                if (data.GetCardTypes().Contains(CardType.Prompt))
                {
                    card.PlayCard();
                }
                continue;
            }

            switch (data.VirusEffect)
            {
                case VirusEffect.None:
                    continue;
                case VirusEffect.Duplicate:
                    AddCardToDeck(card.GetData());
                    break;
                case VirusEffect.LoseHealth:
                    GameManager.ChangePlayerHealth(-10);
                    break;
                case VirusEffect.LoseSanity:
                    AIHealthChange?.Invoke(5);
                    break;
            }
        }
    }
    
    public void DiscardAllCards()
    {
        for (int i = _handCardsList.Count - 1; i >= 0; i--)
        {
            DiscardCard(_handCardsList[i]);
        }
    }

    public void UpdateCardPositions()
    {
        hand.UpdateCardPositions(_handCardsList);
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
