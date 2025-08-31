using System;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum CardType
{
    Prompt = 1 << 0,
    Draw = 1 << 1,
    Delete = 1 << 2,
    Discard = 1 << 3,
    Status = 1 << 4,
}

[Flags]
public enum PromptType
{
    Specify = 1 << 0,
    Persona = 1 << 1,
    Format = 1 << 2,
    Iterate = 1 << 3,
    Context = 1 << 4,
    Constraints = 1 << 5,
    Steps = 1 << 6,
    Goal = 1 << 7,
    False = 1 << 8,
}

public enum CardRarity
{
    None = 0,
    Starting = 1,
    Common = 2,
    Rare = 4,
    Legendary = 8,
}

public enum VirusEffect
{
    None = 0,
    LoseHealth = 1,
    LoseSanity = 2,
    Duplicate = 3,
}

[CreateAssetMenu(fileName = "CardData", menuName = "Card/Card Data")]
public class CardData : ScriptableObject
{
    [SerializeField] public CardType cardType;
    [SerializeField] private PromptType promptType;
    [SerializeField] public CardRarity cardRarity;

    public CardData[] cardata;
    [SerializeField] public PromptPlacement[] promptPlacement;

	[SerializeField] public string cardName;
    [SerializeField, TextArea] private string cardDescription;
    [SerializeField] private Sprite[] cardSymbol;
    [SerializeField]
    public PormptAndTag cardPromptUpdate;
    

    [SerializeField] private int cardsToDraw = 1;
    [SerializeField] private int cardsToDelete = 1;
    [SerializeField] private int cardsToDiscard = 1;
    [SerializeField] private bool isPlayedEndOfTurn;  
    [SerializeField] private VirusEffect virusEffect;

    public string CardName => cardName;
    public string CardDescription => cardDescription;
    public Sprite[] CardSymbol => cardSymbol;
    public CardRarity Rarity => cardRarity;
    
    public PromptPlacement[] Placement => promptPlacement;

    public PormptAndTag CardPromptUpdate => cardPromptUpdate;
    // Prompt var
    public PromptType PromptType => promptType;
    // Draw var
    public int CardsToDraw => cardsToDraw;
    // Delete var
    public int CardsToDelete => cardsToDelete;
    public int CardsToDiscard => cardsToDiscard;
    // Status var
    public bool IsPlayedEndOfTurn => isPlayedEndOfTurn;
    public VirusEffect VirusEffect => virusEffect;
    
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

    public List<PromptType> GetPromptTypes()
    {
        List<PromptType> result = new List<PromptType>();

        foreach (PromptType type in Enum.GetValues(typeof(PromptType)))
        {
            if (promptType.HasFlag(type))
            {
                result.Add(type);
            }
        }
        
        return result;
    }

}
