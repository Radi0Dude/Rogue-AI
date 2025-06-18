using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class AI : MonoBehaviour
{
    public event Action OnAISane;
    
    
    [Header("UI Elements")]
    [SerializeField] private TMP_Text aiSanityText;
    [SerializeField] private Image sanityBar;
    
    private float _maxSanity;
    private float _currentSanity;
    private int _countDown;
    
    private AIData _data;
    private Player _player;
    private AIActionData _currentAction;

    public void Initialize(Player player)
    {
        if (GameManager.GetRoom() is CombatRoom room)
        {
            _data = room.aiData;
        }
        else
        {
            Debug.LogError("The current room in GameManager is not a CombatRoom");
            return;
        }
        
        
        _maxSanity = _data.maxSanity;
        _currentSanity = _data.startSanity;
        _player = player;
        GetNextAction();
        UpdateUI();
    }

    private void GetNextAction()
    {
        _currentAction = _data.aiActions[Random.Range(0, _data.aiActions.Count)];
        _countDown = _currentAction.roundsUntilAction;
    }
    
    public void ReduceCountDown()
    {
        _countDown--;
        
        if (_data.endOfTurnAction != null)
        {
            _data.endOfTurnAction.PerformAction(this, _player);
        }
        
        if (_countDown <= 0)
        {
            _currentAction.PerformAction(this, _player);
            GetNextAction();
        }
    }

    public void ChangeSanity(float value)
    {
        _currentSanity += value;
        
        if (_currentSanity <= 0.0f)
        {
            _currentSanity = 0.0f;
        }
        else if (_currentSanity >= _maxSanity)
        {
            _currentSanity = _maxSanity;
            Debug.Log("AI was made sane");
            OnAISane?.Invoke();
        }
        UpdateUI();

    }

    private void UpdateUI()
    {
        // Calculate Percentage
        var percentageInDecimal = _currentSanity / _maxSanity;
        var percentage = percentageInDecimal * 100f;

        // Update SanityBar
        DOTween.To(()=> sanityBar.fillAmount, x=> sanityBar.fillAmount = x, percentageInDecimal, 1f);

        // Update SanityTextPercentage
        aiSanityText.text = percentage.ToString("0.0") + "%";
    }
}
