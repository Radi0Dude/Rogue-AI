using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private TMP_Text healthText;
    
    private int _currentHealth = GameManager.PlayerMaxHealth;
    
    public void ChangeHealth(int value)
    {
        _currentHealth += value;
        UpdateHealthVisuals();
    }

    private void UpdateHealthVisuals()
    {
        healthText.text = _currentHealth+"/"+GameManager.PlayerMaxHealth;
    }
}
