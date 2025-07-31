using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardVisual : MonoBehaviour
{
    [Header("Visuals")]
    [SerializeField] private TMP_Text cardName;
    [SerializeField] private Image cardSymbol;
    
    [SerializeField] private Image[] smallerCardSymbols;
    [SerializeField] private AbilityTooltipText cardAbilityTooltip;


   

    public void UpdateCardVisuals(CardData data)
    {
        cardName.text = data.CardName;
        
        if (data.CardSymbol == null || data.CardSymbol.Length == 0)
            return;

        if (data.CardSymbol.Length == 1)
        {
            cardSymbol.enabled = true;
            cardSymbol.sprite = data.CardSymbol[0];
        }
        else
        {
            for (int i = 0; i < data.CardSymbol.Length; i++)
            {
                cardSymbol.enabled = false;
                smallerCardSymbols[i].enabled = true;
                smallerCardSymbols[i].sprite = data.CardSymbol[i];
            }
        }
        
        SetAbilityTooltipText(data);
    }
    
    private void SetAbilityTooltipText(CardData data)
    {
        string tooltipText = "";
        List<CardType> cardTypesEnum = data.GetCardTypes();
        var cardTypes = new HashSet<CardType>(cardTypesEnum);
        

        if (cardTypes.Contains(CardType.Status))
        {
            tooltipText += $"<color=purple>{CardType.Status}</color> ";
        }
        
        if (cardTypes.Contains(CardType.Prompt))
        {
            tooltipText += "Prompt term(s): ";
            foreach (PromptType promptType in data.GetPromptTypes())
            {
                tooltipText += promptType + " ";
            }
            tooltipText += "\n";
        }
        
        if (cardTypes.Contains(CardType.Discard))
        {
            tooltipText += $"<color=yellow>{CardType.Discard}</color> " + data.CardsToDiscard + " card(s)\n";
        }

        if (cardTypes.Contains(CardType.Draw))
        {
            tooltipText += $"<color=yellow>{CardType.Draw}</color> " + data.CardsToDraw + " card(s)\n";
        }

        if (cardTypes.Contains(CardType.Delete))
        {
            tooltipText += $"<color=yellow>{CardType.Delete}</color> " + data.CardsToDelete + " card(s)\n";
        }
        
        cardAbilityTooltip.SetAbilityTooltipText(tooltipText);
    }
}
