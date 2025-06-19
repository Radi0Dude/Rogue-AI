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
    public CardType cardType;
    public PromptType promptType;
    public AITypes effectiveAgainst;

    public string cardName;
    [TextArea]
    public string cardDescription;
    public Sprite cardSymbol;
    public Sprite cardImage;

    [Header("Only need to be filled for the respective card type\n E.g. cardsToDraw only matter if cardType Draw is selected")]
    public int cardsToDraw = 1;
    public int cardsToDelete = 1;
    
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
