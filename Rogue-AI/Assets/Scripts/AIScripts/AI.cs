using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class AI : MonoBehaviour
{
    public event Action OnAISane;
    
    [SerializeField] private AIVisual visual;
    
    private float _maxSanity;
    private float _currentSanity;
    private int _countDown;
    
    private AIData _data;
    private CombatManager _combatManager;
    //private PlayerVisual _player;
    private AIActionData _currentAction;
    private AIActionData _endOfTurnAction;


    public void Initialize(CombatManager combatManager, AIData data)
    {
        _data = data;
        _combatManager = combatManager;
        _currentSanity = _data.StartSanity;
        _maxSanity = _data.MaxSanity;
        GetNextAction();
        _endOfTurnAction = _data.EndOfTurnAction;

        visual.UpdateSanityBar(_currentSanity, _maxSanity);

        if (_data.EndOfTurnAction != null)
        {
            visual.SetEndOfTurnAction(_data.EndOfTurnAction);
        }
    }

    private void GetNextAction()
    {
        _currentAction = _data.AIActions[Random.Range(0, _data.AIActions.Count)];
        visual.SetMainAction(_currentAction);
        _countDown = _currentAction.RoundsUntilAction;
    }
    
    public void ReduceCountDown()
    {
        _countDown--;
        visual.SetMainCountdown(_countDown);
        
        if (_endOfTurnAction != null)
        {
            _endOfTurnAction.PerformAction(this, _combatManager);
        }
        
        if (_countDown <= 0)
        {
            _currentAction.PerformAction(this, _combatManager);
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
            visual.HideCanvas();
            OnAISane?.Invoke();
        }
        visual.UpdateSanityBar(_currentSanity, _maxSanity);

    }

    public void LoseSanityByPercentage(float value)
    {
        var sanityLoss = -_currentSanity * value / 100.0f; 
        ChangeSanity(sanityLoss);
    }

    public bool IsSane()
    {
        return Mathf.Approximately(_currentSanity, _maxSanity);
    }

    
}
