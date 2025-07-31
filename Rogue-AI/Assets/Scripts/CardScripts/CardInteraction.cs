using System;
using UnityEngine;

public class CardInteraction : MonoBehaviour
{

    public event Action OnCardPressed;
    public event Action OnRewardSelected;
    

    private void OnMouseDown()
    {
        OnCardPressed?.Invoke();
        OnRewardSelected?.Invoke();
    }
}
