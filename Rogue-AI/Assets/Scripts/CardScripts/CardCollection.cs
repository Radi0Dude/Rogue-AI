using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card Collection", menuName = "Card Collection")]
public class CardCollection : ScriptableObject
{
    [SerializeField] public List<CardData> CardsInCollection;

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
