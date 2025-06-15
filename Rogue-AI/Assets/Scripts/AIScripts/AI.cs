using UnityEngine;

public class AI : MonoBehaviour
{
    [SerializeField] private AIData data;
    
    private float _maxSanity;
    private float _currentSanity;
    private int _countDown;
    
    private Player _player;
    private AIAction _currentAction;

    public void Initialize()
    {
        _maxSanity = data.maxSanity;
        _currentSanity = data.startSanity;
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
    }
}
