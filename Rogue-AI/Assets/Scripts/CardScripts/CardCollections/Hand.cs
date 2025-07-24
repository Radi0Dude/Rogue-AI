using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField] private List<Transform> cardSlots = new List<Transform>();
    [SerializeField] private TooltipText circleTooltip;

    private void Start()
    {
        circleTooltip.SetTooltipText("End your turn and draw a new hand");
    }

    public void UpdateCardPositions(List<Card> handCards)
    {
        if (handCards.Count == 0) return;
        
        for (int i = 0; i < handCards.Count; i++)
        {
            var pos = cardSlots[i].position;
            var localEuler = cardSlots[i].localEulerAngles;
            if (handCards[i] == null) continue;
            handCards[i].transform.DOMove(pos, 0.25f);
            handCards[i].transform.DOLocalRotate(localEuler, 0.5f);
        }
    }
}
