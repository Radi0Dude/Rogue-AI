using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Splines;

public class Hand : MonoBehaviour
{
    [SerializeField] private SplineContainer splineContainer;
    
    
    
    public void UpdateCardPositions(List<Card> handCards)
    {
        if (handCards.Count == 0) return;

        
        
        float cardspacing = 1f / GameManager.MaxHandSize;
        float firstCardPosition = 0.5f - (handCards.Count - 1) * cardspacing / 2;
        Spline spline = splineContainer.Spline;
        for (int i = 0; i < handCards.Count; i++)
        {
            float p = firstCardPosition + i * cardspacing;
            Vector3 splinePos = spline.EvaluatePosition(p);
            Vector3 forward = spline.EvaluateTangent(p);
            float angleZ = Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg;
            Vector3 localEuler = new Vector3(0f, 0f, angleZ);
            handCards[i].transform.DOMove(splinePos, 0.25f);
            handCards[i].transform.DOLocalRotate(localEuler, 0.5f);

        }
    }
}