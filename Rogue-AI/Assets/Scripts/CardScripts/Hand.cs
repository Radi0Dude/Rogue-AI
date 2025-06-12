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
            Vector3 up = spline.EvaluateUpVector(p);
            Quaternion rotation = Quaternion.LookRotation(up, Vector3.Cross(Vector3.up, forward).normalized);
            handCards[i].transform.DOMove(splinePos, 0.25f);
            handCards[i].transform.DOLocalRotateQuaternion(rotation, 0.25f);
        }
    }
}