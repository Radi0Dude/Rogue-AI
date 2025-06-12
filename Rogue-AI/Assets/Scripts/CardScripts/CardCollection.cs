using System.Collections.Generic;
using UnityEngine;

public class CardCollection : MonoBehaviour
{
    [SerializeField] public List<CardData> CardsInCollection { get; private set; } = new();

    public void RemoveCardFromCollection(CardData card)
    {
        if (CardsInCollection.Contains(card))
        {
            CardsInCollection.Remove(card);
        }
        else
        {
            Debug.LogError("CardData not in collection");
        }
    }

    public void AddCardToCollection(CardData card)
    {
        CardsInCollection.Add(card);
    }
}
