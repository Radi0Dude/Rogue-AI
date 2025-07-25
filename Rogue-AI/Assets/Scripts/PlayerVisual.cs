using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerVisual : MonoBehaviour
{
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private GameObject LoseScreen;

    private void Start()
    {
        GameManager.OnHealthChanged += UpdateHealthVisuals;
        //GameManager.OnGameLost += ShowLoseGameVisual;
        UpdateHealthVisuals();
    }

    

    private void UpdateHealthVisuals()
    {
        healthText.text = "Tokens: " + GameManager.PlayerHealth+"/"+GameManager.PlayerMaxHealth;
    }
    
    private void ShowLoseGameVisual()
    {
        LoseScreen.SetActive(true);
        var panel = LoseScreen.GetComponent<Image>();
        panel.material.DOColor(Color.white, 1f);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
