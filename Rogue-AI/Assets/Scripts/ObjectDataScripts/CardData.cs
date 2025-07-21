using System;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum CardType
{
    Prompt = 1 << 0,
    Draw = 1 << 1,
    Delete = 1 << 2,
    Status = 1 << 3,
}

[Flags]
public enum PromptType
{
    Specify = 1 << 0,
    Persona = 1 << 1,
    Format = 1 << 2,
    Iterate = 1 << 3,
}

public enum CardRarity
{
    None = 0,
    Common = 1,
    Uncommon = 2,
    Rare = 4,
    Epic = 8,
    Legendary = 16,
    
}

[CreateAssetMenu(fileName = "CardData", menuName = "Card/Card Data")]
public class CardData : ScriptableObject
{
    [SerializeField] private CardType cardType;
    [SerializeField] private PromptType promptType;
    [SerializeField] private AITypes effectiveAgainst;
    [SerializeField] private CardRarity cardRarity;

    [SerializeField] private string cardName;
    [SerializeField, TextArea] private string cardDescription;
    [SerializeField] private Sprite cardSymbol;

    [SerializeField] private int cardsToDraw = 1;
    [SerializeField] private int cardsToDelete = 1;
    [SerializeField] private bool isPlayable;
    [SerializeField] private bool isPlayedEndOfTurn;    

    public string CardName => cardName;
    public string CardDescription => cardDescription;
    public Sprite CardSymbol => cardSymbol;
    public CardRarity Rarity => cardRarity;
    
    // Prompt var
    public PromptType PromptType => promptType;
    public AITypes EffectiveAgainst => effectiveAgainst;
    // Draw var
    public int CardsToDraw => cardsToDraw;
    // Delete var
    public int CardsToDelete => cardsToDelete;
    // Status var
    public bool IsPlayable => isPlayable;
    public bool IsPlayedEndOfTurn => isPlayedEndOfTurn;
    
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
