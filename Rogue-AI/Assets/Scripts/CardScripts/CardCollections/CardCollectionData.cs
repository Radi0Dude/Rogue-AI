using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardCollection", menuName = "Card/Card Collection")]
public class CardCollectionData : ScriptableObject
{
    [SerializeField] private List<CardData> cardsInCollection;
    
    public List<CardData> CardsInCollection => cardsInCollection;

    public void InitializeCollection(List<CardData> initialCollection)
    {
        foreach (var card in initialCollection)
        {
            AddCardToCollection(card);
        }
    }

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
