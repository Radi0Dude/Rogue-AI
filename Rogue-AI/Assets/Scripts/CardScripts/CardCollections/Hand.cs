using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Splines;

public class Hand : MonoBehaviour
{
    [SerializeField] private List<Transform> cardSlots = new List<Transform>();
    
    
    public void UpdateCardPositions(List<Card> handCards)
    {
        if (handCards.Count == 0) return;
        
        for (int i = 0; i < handCards.Count; i++)
        {
            var pos = cardSlots[i].position;
            var localEuler = cardSlots[i].localEulerAngles;
            handCards[i].transform.DOMove(pos, 0.25f);
            handCards[i].transform.DOLocalRotate(localEuler, 0.5f);

        }
    }
}
