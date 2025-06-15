using UnityEngine;

public class AI : MonoBehaviour
{
    [SerializeField] private AIData data;
    
    private float _maxSanity;
    private float _currentSanity;
    private int _countDown;

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

        if (_countDown <= 0)
        {
            _currentAction.PerformAction();
        }
    }

    public void ChangeSanity(float value)
    {
        _currentSanity += value;
    }
}
