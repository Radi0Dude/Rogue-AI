using UnityEngine;

public class Card : MonoBehaviour
{
    public CardData cardData;
    [Header("Attached Scripts")]
    [SerializeField] private CardVisual cardVisual;
    

    public void SetUp(CardData data)
    {
        cardData = data;
        cardVisual.UpdateCardVisuals(data);
    }
}
