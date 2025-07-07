using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class AI : MonoBehaviour
{
    public event Action OnAISane;


    [FormerlySerializedAs("Canvas")]
    [Header("UI Elements")] 
    [SerializeField] private Canvas canvas;
    [SerializeField] private TMP_Text aiSanityText;
    [SerializeField] private Image sanityBar;
    
    private float _maxSanity;
    private float _currentSanity;
    private int _countDown;
    
    [SerializeField] private AIData _data;
    private Player _player;
    private AIActionData _currentAction;

    public void Initialize(Player player)
    {
        if (GameManager.GetRoom() is CombatRoom room)
        {
            _data = room.AIData;
        }
        else
        {
            Debug.LogError("The current room in GameManager is not a CombatRoom");
            return;
        }
        
        
        _maxSanity = _data.MaxSanity;
        _currentSanity = _data.StartSanity;
        _player = player;
        GetNextAction();
        UpdateUI();
    }

    private void GetNextAction()
    {
        _currentAction = _data.AIActions[Random.Range(0, _data.AIActions.Count)];
        _countDown = _currentAction.RoundsUntilAction;
    }
    
    public void ReduceCountDown()
    {
        _countDown--;
        
        if (_data.EndOfTurnAction != null)
        {
            _data.EndOfTurnAction.PerformAction(this, _player);
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
            canvas.enabled = false;
            Debug.Log("AI was made sane");
            OnAISane?.Invoke();
        }
        UpdateUI();

    }

    public bool IsSane()
    {
        return Mathf.Approximately(_currentSanity, _maxSanity);
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
