using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class AI : MonoBehaviour
{
    public event Action OnAISane;
    
    [SerializeField] private AIData data;
    
    private float _maxSanity;
    private float _currentSanity;
    private int _countDown;
    
    private Player _player;
    private AIAction _currentAction;

    public void Initialize(Player player)
    {
        _maxSanity = data.maxSanity;
        _currentSanity = data.startSanity;
        _player = player;
        GetNextAction();
    }

    private void GetNextAction()
    {
        _currentAction = data.aiActions[Random.Range(0, data.aiActions.Count)];
        _countDown = _currentAction.roundsUntilAction;
    }
    
    public void ReduceCountDown()
    {
        _countDown--;
        
        if (data.endOfTurnAction != null)
        {
            data.endOfTurnAction.PerformAction(this, _player);
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

        if (_currentSanity >= _maxSanity)
        {
            Debug.Log("AI was made sane");
            OnAISane?.Invoke();
        }
    }
}
