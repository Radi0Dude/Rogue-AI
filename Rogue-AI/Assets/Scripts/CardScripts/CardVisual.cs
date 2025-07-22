using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardVisual : MonoBehaviour
{
    [Header("Visuals")]
    [SerializeField] private TMP_Text cardName;
    [SerializeField] private Image cardSymbol;
    
    [SerializeField] private Image[] smallerCardSymbols;

   

    public void UpdateCardVisuals(CardData cardDataData)
    {
        cardName.text = cardDataData.CardName;
        
        if (cardDataData.CardSymbol == null || cardDataData.CardSymbol.Length == 0)
            return;

        if (cardDataData.CardSymbol.Length == 1)
        {
            cardSymbol.enabled = true;
            cardSymbol.sprite = cardDataData.CardSymbol[0];
        }
        else
        {
            for (int i = 0; i < cardDataData.CardSymbol.Length; i++)
            {
                smallerCardSymbols[i].enabled = true;
                smallerCardSymbols[i].sprite = cardDataData.CardSymbol[i];
            }
        }
    }
}
