using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerVisual : MonoBehaviour
{
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private Image gameOverPanel;

    private void Start()
    {
        GameManager.OnHealthChanged += UpdateHealthVisuals;
        GameManager.OnGameLost += ShowLoseGameVisual;
        UpdateHealthVisuals();
    }
    
    private void UpdateHealthVisuals()
    {
        healthText.text = "Tokens: " + GameManager.PlayerHealth+"/"+GameManager.PlayerMaxHealth;
    }
    
    private void ShowLoseGameVisual()
    {
        gameOverPanel.gameObject.SetActive(true);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    
    private void OnDisable(){
        GameManager.OnHealthChanged -= UpdateHealthVisuals;
        GameManager.OnGameLost -= ShowLoseGameVisual;
    }
}
