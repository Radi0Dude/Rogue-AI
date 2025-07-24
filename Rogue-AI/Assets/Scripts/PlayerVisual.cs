using System;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerVisual : MonoBehaviour
{
    [SerializeField] private TMP_Text healthText;

    private void Start()
    {
        GameManager.OnHealthChanged += UpdateHealthVisuals;
        UpdateHealthVisuals();
    }

    private void UpdateHealthVisuals()
    {
        healthText.text = GameManager.PlayerHealth+"/"+GameManager.PlayerMaxHealth;
    }
}
