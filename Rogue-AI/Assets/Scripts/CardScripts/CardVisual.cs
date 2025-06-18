using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;

public class CardVisual : MonoBehaviour
{
    [Header("Visuals")]
    [SerializeField] private TMP_Text cardName;
    [SerializeField] private Image cardSymbol;
    [SerializeField] private Image cardImage;
    [SerializeField] private TMP_Text cardDescription;

   

    public void UpdateCardVisuals(CardData cardDataData)
    {
        cardName.text = cardDataData.cardName;
        
        if (cardDataData.cardSymbol != null)
        {
            cardSymbol.sprite = cardDataData.cardSymbol;
        }
        else
        {
            Debug.LogWarning(cardDataData.name + " is missing a card symbol");
        }

        if (cardDataData.cardImage != null)
        {
            cardImage.sprite = cardDataData.cardImage;
        }
        else
        {
            Debug.LogWarning(cardDataData.name + " is missing a card image");
        }
        
        cardDescription.text = cardDataData.cardDescription;
    }
}
