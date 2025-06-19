using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CardReward : MonoBehaviour
{
    [SerializeField] private List<Card> cards;
    
    [SerializeField] private CardCollection rewardCollection;
    
    private PlayArea playArea;

    private void Start()
    {
        playArea = FindAnyObjectByType<PlayArea>();
    }

    public void DisplayCardReward()
    {
        playArea.gameObject.SetActive(false);
        foreach (var card in cards)
        {
            card.gameObject.SetActive(true);
            
            CardData cardData = rewardCollection.CardsInCollection[Random.Range(0, rewardCollection.CardsInCollection.Count)];
            
            card.SetUp(cardData);
        }
    }

    public void SelectReward(Card card)
    {
        CardData data = card.GetData();
        
        GameManager.PlayerCardCollection.AddCardToCollection(data);

        foreach (var playerCard in GameManager.PlayerCardCollection.CardsInCollection)
        {
            print(playerCard.name);
        }
        
        GameManager.RemoveRoomFromListAndLoadNextScene();
    }
}
