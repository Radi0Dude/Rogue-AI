using System.Collections;
using DG.Tweening;
using UnityEngine;

public class DiscardPile : MonoBehaviour
{
    [SerializeField] private float distance = 0.1f;
    [SerializeField] private float downRotation = -100.0f;
    
    public void DiscardCard(Card card)
    {
        var eulerAngle = card.transform.localEulerAngles;
        eulerAngle.x = downRotation;
        
        card.transform.DOMove(transform.position, 0.25f);
        card.transform.DOLocalRotate(eulerAngle, 0.5f);
        
        StartCoroutine(HideCardWhenInDiscardPile(card));
    }

    private IEnumerator HideCardWhenInDiscardPile(Card card)
    {
        while (true)
        {
            if (Vector3.Distance(transform.position, card.transform.position) < distance)
            {
                card.gameObject.SetActive(false);

                break;
            }
            
            yield return new WaitForEndOfFrame();
        }
    }
}
