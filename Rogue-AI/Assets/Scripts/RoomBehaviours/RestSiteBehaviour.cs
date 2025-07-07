using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class RestSiteBehaviour : MonoBehaviour
{
    private Player _player;
    private CardLibrary _cardLibrary;
    
    private bool _canRemoveCards = false;
    
    [SerializeField] private int healAmount = 25;
    [SerializeField] private CanvasRenderer canvasRenderer;

    private void Awake()
    {
        _player = FindAnyObjectByType<Player>();
        _cardLibrary = FindAnyObjectByType<CardLibrary>();
    }

    public void RestButtonPressed()
    {
        _player.ChangeHealth(healAmount);
        AfterButtonPressed();
    }

    public void ViewDeckButtonPressed()
    {
        canvasRenderer.gameObject.SetActive(false);
    }

    public void RefactorButtonPressed()
    {
        _cardLibrary.ViewCardsInList();
        _canRemoveCards = true;
        canvasRenderer.gameObject.SetActive(false);
    }

    private void ReturnToOptions()
    {
        _canRemoveCards = false;
        canvasRenderer.gameObject.SetActive(true);
    }

    private void DeleteCard()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            if (hit.transform.TryGetComponent(out Card card))
            {
                GameManager.PlayerCardCollection.RemoveCardFromCollection(card.GetData());
                _cardLibrary.ButtonPressed(false);
                
                _canRemoveCards = false;
                canvasRenderer.gameObject.SetActive(true);

                AfterButtonPressed();
            }
            else
            {
                print(hit.transform.name);
            }
        }
    }

    public void GetClick(InputAction.CallbackContext context)
    {
        if (context.performed && _canRemoveCards)
        {
            DeleteCard();
        }
    }

    public void CancelDeletionButtonPressed()
    {
        ReturnToOptions();
    }

    private void AfterButtonPressed()
    {
        GameManager.RemoveRoomFromListAndLoadNextScene();
    }
}