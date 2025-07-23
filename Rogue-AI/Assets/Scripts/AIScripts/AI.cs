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
    private Player _player;
    private AIActionData _currentAction;
    private AIActionData _endOfTurnAction;


    public void Initialize(Player player)
    {
        
        if (GameManager.GetRoom() is CombatRoom room)
        {
            _data = room.AIData;
        }
        else
        {
            Debug.LogError("Room is not a CombatRoom");
            return;
        }
        
        _currentSanity = _data.StartSanity;
        _maxSanity = _data.MaxSanity;
        _player = player;
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
            _endOfTurnAction.PerformAction(this, _player);
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
            visual.HideCanvas();
            OnAISane?.Invoke();
        }
        visual.UpdateSanityBar(_currentSanity, _maxSanity);

    }

    public bool IsSane()
    {
        return Mathf.Approximately(_currentSanity, _maxSanity);
    }

    
}
