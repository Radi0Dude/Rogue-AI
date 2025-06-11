using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardVisual : MonoBehaviour
{
    [Header("Scriptable Card")]
    [SerializeField] private Card cardData;
    [Header("Visuals")]
    [SerializeField] private TMP_Text cardName;
    [SerializeField] private Image cardSymbol;
    [SerializeField] private Image cardImage;
    [SerializeField] private TMP_Text cardDescription;

    private void Start()
    {
        UpdateCardVisuals();
    }

    private void UpdateCardVisuals()
    {
        cardName.text = cardData.cardName;
        
        if (cardData.cardSymbol != null)
        {
            cardSymbol.sprite = cardData.cardSymbol;
        }
        else
        {
            Debug.LogError(cardData.name + " is missing a card symbol");
        }

        if (cardData.cardImage != null)
        {
            cardImage.sprite = cardData.cardImage;
        }
        else
        {
            Debug.LogError(cardData.name + " is missing a card image");
        }
        
        cardDescription.text = cardData.cardDescription;
    }
}
