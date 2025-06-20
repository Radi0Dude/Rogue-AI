using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardLibrary : MonoBehaviour
{

    [SerializeField] private GameObject uiCardPrefab;
    [SerializeField] private MyGridLayoutGroup cardLibraryPanel;
    [SerializeField] private Image openButton, closeButton, scrollView;

    private RectTransform _rectTransform;
    private List<Card> _cards = new ();

    private void Start()
    {
        // Create Empty Blank cards for display
        InitiateCards();
        _rectTransform = cardLibraryPanel.gameObject.GetComponent<RectTransform>();
    }


    public void ViewCardsInList(List<CardData> cardsInCollection = null)
    {
        OpenLibrary();
        cardsInCollection ??= GameManager.PlayerCardCollection.CardsInCollection;
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

    public void ButtonPressed(bool shouldOpen)
    {
        if (shouldOpen)
        {
            ViewCardsInList();
        }
        else
        {
            CloseLibrary();
        }
    }
    
    private void CloseLibrary()
    {
        scrollView.gameObject.SetActive(false);
        closeButton.gameObject.SetActive(false);
        openButton.gameObject.SetActive(true);
        foreach (var card in _cards)
        {
            card.transform.parent.gameObject.SetActive(false);
        }
    }

    private void OpenLibrary()
    {
        scrollView.gameObject.SetActive(true);
        closeButton.gameObject.SetActive(true);
        openButton.gameObject.SetActive(false);
    }
}
