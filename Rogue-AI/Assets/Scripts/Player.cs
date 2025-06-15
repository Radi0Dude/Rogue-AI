using System;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Player : MonoBehaviour
{
    public event Action OnPlayerDeath;
    
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private Deck deck;
    
    private static int _currentHealth = GameManager.PlayerMaxHealth;

    private void Start()
    {
        UpdateHealthVisuals();
    }

    public void DrawHand()
    {
        deck.DrawHand(GameManager.StartOfRoundDraw);
    }

    public void ChangeHealth(int value)
    {
        _currentHealth += value;
        UpdateHealthVisuals();
        if (_currentHealth <= 0)
        {
            Debug.Log("Player Died");
            OnPlayerDeath?.Invoke();
        }
    }

    private void UpdateHealthVisuals()
    {
        healthText.text = _currentHealth+"/"+GameManager.PlayerMaxHealth;
    }


    public void DiscardAllCards()
    {
        deck.DiscardAllCards();
    }
}
