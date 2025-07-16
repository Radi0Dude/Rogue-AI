using System.Collections.Generic;
using UnityEngine;

public class RestSiteManager : MonoBehaviour
{
    private Player _player;
    private CardLibrary _cardLibrary;
    
    private bool _canRemoveCards = false;
    private int _cardsToRemove = 0;
    
    [SerializeField] private int healAmount = 25;
    [SerializeField] private CanvasRenderer canvasRenderer;

    private void Awake()
    {
        _player = FindAnyObjectByType<Player>();
        _cardLibrary = FindAnyObjectByType<CardLibrary>();
    }

    private void Start()
    {
        List<Card> cards = _cardLibrary.GetCards();
        foreach (var card in cards)
        {
            card.OnRewardSelected += DeleteCard;
        }
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

    public void RefactorButtonPressed(int numbToDelete = 1)
    {
        _cardLibrary.ViewCardsInList();
        _canRemoveCards = true;
        _cardsToRemove = numbToDelete;
        canvasRenderer.gameObject.SetActive(false);
    }

    private void ReturnToOptions()
    {
        _canRemoveCards = false;
        canvasRenderer.gameObject.SetActive(true);
    }

    private void DeleteCard(Card card)
    {
        if (!_canRemoveCards) return;
        
        GameManager.PlayerCardCollection.RemoveCardFromCollection(card.GetData());
        _cardLibrary.ButtonPressed(false);
        
        _cardsToRemove--;

        if (_cardsToRemove >= 0)
        {
            AfterButtonPressed();
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