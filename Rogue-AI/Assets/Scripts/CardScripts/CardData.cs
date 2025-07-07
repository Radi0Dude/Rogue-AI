using System;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum CardType
{
    Prompt = 1 << 0,
    Draw = 1 << 1,
    Delete = 1 << 2,
    Virus = 1 << 3,
}

[Flags]
public enum PromptType
{
    Specify = 1 << 0,
    Persona = 1 << 1,
    Format = 1 << 2,
    Iterate = 1 << 3,
}

[CreateAssetMenu(fileName = "CardData", menuName = "Card/Card Data")]
public class CardData : ScriptableObject
{
    [SerializeField] private CardType cardType;
    [SerializeField] private PromptType promptType;
    [SerializeField] private AITypes effectiveAgainst;

    [SerializeField] private string cardName;
    [SerializeField, TextArea] private string cardDescription;
    [SerializeField] private Sprite cardSymbol;
    [SerializeField] private Sprite cardImage;

    [Header("Only need to be filled for the respective card type\nE.g. cardsToDraw only matters if cardType 'Draw' is selected")]
    [SerializeField] private int cardsToDraw = 1;
    [SerializeField] private int cardsToDelete = 1;

    public PromptType PromptType => promptType;
    public AITypes EffectiveAgainst => effectiveAgainst;

    public string CardName => cardName;
    public string CardDescription => cardDescription;
    public Sprite CardSymbol => cardSymbol;
    public Sprite CardImage => cardImage;

    public int CardsToDraw => cardsToDraw;
    public int CardsToDelete => cardsToDelete;

    
    public List<CardType> GetCardTypes()
    {
        List<CardType> result = new List<CardType>();

        foreach (CardType type in Enum.GetValues(typeof(CardType)))
        {
            if (cardType.HasFlag(type))
            {
                result.Add(type);
            }
        }

        return result;
    }

}
