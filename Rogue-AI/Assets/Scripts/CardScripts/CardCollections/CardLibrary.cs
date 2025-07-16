using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardLibrary : MonoBehaviour
{

    [SerializeField] private GameObject uiCardPrefab;
    [SerializeField] private MyGridLayoutGroup cardLibraryPanel;
    [SerializeField] private GameObject openButton, closeButton, scrollView, canvasToHide;

    private RectTransform _rectTransform;
    private List<Card> _cards = new ();
    
    // Delete card variables
    private bool _canRemoveCards = false;
    private int _cardsToRemove = 0;

    private void Awake()
    {
        // Create Empty Blank cards for display
        InitiateCards();
        _rectTransform = cardLibraryPanel.gameObject.GetComponent<RectTransform>();
    }

    private void Start()
    {
        foreach (var card in _cards)
        {
            card.OnRewardSelected += DeleteCard;
        }
    }
    
    private void InitiateCards()
    {
        var amountToSpawn = GameManager.PlayerCardCollection.CardsInCollection.Count;
        for (int i = 0; i < amountToSpawn; i++)
        {
            var uiInstance = Instantiate(uiCardPrefab, cardLibraryPanel.transform, false);
            _cards.Add(uiInstance.GetComponentInChildren<Card>());
            uiInstance.gameObject.SetActive(false);
        }
        CloseLibrary();
    }

    public void LibraryToggle(bool shouldOpen)
    {
        if (shouldOpen)
        {
            OpenLibrary();
        }
        else
        {
            CloseLibrary();
        }
    }
    
    private void CloseLibrary()
    {
        if (canvasToHide != null)
            canvasToHide.SetActive(true);
        
        scrollView.SetActive(false);
        closeButton.SetActive(false);
        openButton.SetActive(true);
        _canRemoveCards = false;
        
        foreach (var card in _cards)
        {
            card.transform.parent.gameObject.SetActive(false);
        }
    }

   
    private void OpenLibrary(bool showCloseButton = true)
    {
        if (canvasToHide != null)
            canvasToHide.SetActive(false);
        
        scrollView.SetActive(true);
        closeButton.SetActive(showCloseButton);
        openButton.SetActive(false);
        
        var cardsInCollection = GameManager.PlayerCardCollection.CardsInCollection;
        for (int i = 0; i < cardsInCollection.Count; i++)
        {
            _cards[i].transform.parent.gameObject.SetActive(true);
            _cards[i].SetUp(cardsInCollection[i]);
        }

        StartCoroutine(ChangeHeightOfLibrary());
    }

    private IEnumerator ChangeHeightOfLibrary()
    {
        yield return new WaitForEndOfFrame();
        
        Vector2 size = _rectTransform.sizeDelta;
        size.y = cardLibraryPanel.preferredHeight;
        _rectTransform.sizeDelta = size;

    }
    
    public void StartDeleteCards(int count, bool showCloseButton = true)
    {
        OpenLibrary(showCloseButton);
        _canRemoveCards = true;
        _cardsToRemove = count;
    }
    
    private void DeleteCard(Card card)
    {
        if (!_canRemoveCards) return;
        
        GameManager.PlayerCardCollection.RemoveCardFromCollection(card.GetData());
        HideCloseButton();
        
        _cardsToRemove--;

        if (_cardsToRemove >= 0)
        {
            LoadNextScene();
        }
    }

    private void HideCloseButton()
    {
        closeButton.SetActive(false);
    }
    
    private void LoadNextScene()
    {
        GameManager.RemoveRoomFromListAndLoadNextScene();
    }
}
