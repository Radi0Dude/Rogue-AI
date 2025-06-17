using DG.Tweening;
using UnityEngine;

public class DiscardPile : MonoBehaviour
{
    public void DiscardCard(Card card)
    {
        card.transform.DOMove(transform.position, 0.25f);
    }
}
