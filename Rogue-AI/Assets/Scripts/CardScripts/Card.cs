using System;
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

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
    public CardType cardType;
    public PromptType promptType;

    public string cardName;
    [TextArea]
    public string cardDescription;
    public Sprite cardSymbol;
    public Sprite cardImage;
}
