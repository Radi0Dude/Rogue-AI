
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;

public enum CardPlayState
{
    Play,
    Delete
}

public class CardMovement : MonoBehaviour
{
    private Deck _deck;
    private Card _card;
    private CardPlayState _cardPlayState;
    
    private bool _isMoving;
    private int _cardsToDelete;

    [SerializeField] private Image deleteWarningPanel;

    private void Awake()
    {
        _deck = FindAnyObjectByType<Deck>();
        
    }

    private void Start()
    {
        _deck.OnDeletePlayed += ChangePlayStateToDelete;
    }

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
            if (hit.transform.TryGetComponent(out Card card))
            {
                switch (_cardPlayState)
                {
                    case CardPlayState.Play:
                        _isMoving = true;

                        _card = card;
                        break;
                    case CardPlayState.Delete:
                        _deck.DeleteCard(card);

                        _cardsToDelete--;
                        if (_cardsToDelete == 0)
                        {
                            ChangePlayStateToPlay();
                        }
                        break;
                }
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
        if (_card == null) return;
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            if (hit.transform.TryGetComponent(out PlayArea playArea))
            {
                // Play Card
                _deck.DiscardCard(_card);
                playArea.PlayCard(_card);
                // TODO: Send signal to prompt bar and perform actions
            }
            else
            {
                // Return card to hand
                _deck.UpdateCardPositionsHand();
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

    private void ChangePlayStateToDelete(int numb)
    {
        // Adds UI 
        deleteWarningPanel.gameObject.SetActive(true);
        
        _cardPlayState = CardPlayState.Delete;
        _cardsToDelete = numb;
    }

    private void ChangePlayStateToPlay()
    {
        deleteWarningPanel.gameObject.SetActive(false);
        
        _cardPlayState = CardPlayState.Play;
    }
}
