using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] public CardData cardData;
    [Header("Attached Scripts")]
    [SerializeField] private CardVisual cardVisual;
    

    public void SetUp(CardData data)
    {
        cardData = data;
        cardVisual.UpdateCardVisuals(data);
    }
}
