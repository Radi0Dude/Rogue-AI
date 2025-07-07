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
        cardName.text = cardDataData.CardName;
        
        if (cardDataData.CardSymbol != null)
        {
            cardSymbol.sprite = cardDataData.CardSymbol;
        }
        else
        {
            Debug.LogWarning(cardDataData.name + " is missing a card symbol");
        }
        
        cardDescription.text = cardDataData.CardDescription;
    }
}
