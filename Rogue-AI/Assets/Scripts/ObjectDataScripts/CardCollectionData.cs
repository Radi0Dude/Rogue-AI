using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardCollection", menuName = "Card/Card Collection")]
public class CardCollectionData : ScriptableObject
{
    [SerializeField] private List<CardData> cardsInCollection = new List<CardData>();
    
    public List<CardData> CardsInCollection => cardsInCollection;

    public void InitializeCollection(List<CardData> initialCollection)
    {
        foreach (var card in initialCollection)
        {
            cardsInCollection.Add(card);
        }
    }

    public void RemoveCardFromCollection(CardData card)
    {
        if (cardsInCollection.Contains(card))
        {
            cardsInCollection.Remove(card);
        }
        else
        {
            Debug.LogError("CardData not in collection");
        }
    }

    public void AddCardToCollection(CardData card)
    {
        cardsInCollection.Add(card);
    }
}
