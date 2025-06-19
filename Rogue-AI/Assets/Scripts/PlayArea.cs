using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayArea : MonoBehaviour
{
    private InputField _inputField;
    private Deck _deck;

    private void Awake()
    {
        _inputField = FindAnyObjectByType<InputField>();
        _deck = FindAnyObjectByType<Deck>();
    }

    public void PlayCard(Card card)
    {
        var cardData = card.GetData();

        List<CardType> cardTypes = cardData.GetCardTypes();

        foreach (CardType cardType in cardTypes)
        {
            if (cardType == CardType.Prompt)
            {
                _inputField.UpdatePrompt(cardData.promptType);
            }
            else if (cardType == CardType.Draw)
            {
                _deck.DrawHand(cardData.cardsToDraw);
            }
            else if (cardType == CardType.Delete)
            {
                //Delete x card
            }
            else if (cardType == CardType.Virus)
            {
                // Use up space
            }
            else
            {
                Debug.LogError(cardType + " has not been given an action");
            }
        }
    }
}
