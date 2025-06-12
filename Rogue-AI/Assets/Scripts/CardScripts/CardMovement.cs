
using UnityEngine;
using UnityEngine.InputSystem;

public class CardMovement : MonoBehaviour
{
    [SerializeField] private Deck deck;
    
    private bool _isMoving;
    private Card _card;
    
    
    private void Update()
    {
        if (_isMoving)
        {
            MoveCard();
        }
    }
    
    private void PickupCard()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            if (hit.transform.TryGetComponent<Card>(out Card card))
            {
                _isMoving = true;

                _card = card;
            }
        }
    }
    
    private void MoveCard()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = _card.transform.position.z;
        _card.transform.position = Camera.main.ScreenToWorldPoint(mousePosition);
    }

    private void TryPlayCard()
    {
        if (_card != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                // Goal Area
                deck.DiscardCard(_card);
            }
        }
    }
    
    public void GetClick(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PickupCard();
        }
        if (context.canceled)
        {
            _isMoving = false;
            TryPlayCard();
            _card = null;
        }
    }
}
