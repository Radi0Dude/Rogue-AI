using System.Collections.Generic;
using UnityEngine;

namespace Deprecated
{
    public class PlayArea : MonoBehaviour
    { /*
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
                    _inputField.UpdatePrompt(cardData.PromptType);
                }
                else if (cardType == CardType.Draw)
                {
                    _deck.DrawHand(cardData.CardsToDraw);
                }
                else if (cardType == CardType.Delete)
                {
                    _deck.PlayedDeleteCard(cardData.CardsToDelete);
                }
                else if (cardType == CardType.Status)
                {
                    // Use up space
                }
                else
                {
                    Debug.LogError(cardType + " has not been given an action");
                }
            }
        } */
    }
}
