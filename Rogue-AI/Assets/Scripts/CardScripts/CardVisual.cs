using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardVisual : MonoBehaviour
{
    [Header("Visuals")]
    [SerializeField] private TMP_Text cardName;
    [SerializeField] private Image cardSymbol;

   

    public void UpdateCardVisuals(CardData cardDataData)
    {
        cardName.text = cardDataData.CardName;
        
        if (cardDataData.CardSymbol != null)
        {
            cardSymbol.sprite = cardDataData.CardSymbol;
        }
    }
}
