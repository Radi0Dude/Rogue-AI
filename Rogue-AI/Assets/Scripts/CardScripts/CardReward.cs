using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CardReward : MonoBehaviour
{
    [SerializeField] private List<Card> cards;
    
    [SerializeField] private GameObject background;
    [SerializeField] private CardCollectionData rewardCollection;
    
    
    public void DisplayCardReward()
    {
        foreach (var card in cards)
        {
            card.gameObject.SetActive(true);
            
            CardData cardData = rewardCollection.CardsInCollection[Random.Range(0, rewardCollection.CardsInCollection.Count)];
            
            card.SetUp(cardData);

            card.OnRewardSelected += SelectReward;
        }
    }

    public void SelectReward(Card card)
    {
        CardData data = card.GetData();
        
        GameManager.PlayerCardCollection.AddCardToCollection(data);
        
        GameManager.RemoveRoomFromListAndLoadNextScene();
    }
}
