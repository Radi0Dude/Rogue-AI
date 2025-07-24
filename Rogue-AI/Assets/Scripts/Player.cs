using System;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Player : MonoBehaviour
{
    //public event Action OnPlayerDeath;
    
    [SerializeField] private TMP_Text healthText;
    private Deck _deck;
    

    private void Awake()
    {
        _deck = FindAnyObjectByType<Deck>();
    }

    private void Start()
    {
        UpdateHealthVisuals();
    }

    public void DrawHand()
    {
        _deck.DrawHand(GameManager.StartOfRoundDraw);
    }

    public void ChangeHealth(int value)
    {
        GameManager.ChangePlayerHealth(value);
        
        UpdateHealthVisuals();
    }

    private void UpdateHealthVisuals()
    {
        healthText.text = GameManager.PlayerHealth+"/"+GameManager.PlayerMaxHealth;
    }

    public void AddCardToCombat(CardData cardData)
    {
        _deck.AddCardToDeck(cardData, true);
    }

    public void DiscardAllCards()
    {
        _deck.DiscardAllCards();
    }
}
