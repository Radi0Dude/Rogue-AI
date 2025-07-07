using UnityEngine;

public abstract class AIActionData : ScriptableObject
{
    [SerializeField] private string actionName;
    [SerializeField] private int roundsUntilAction;

    public string ActionName => actionName;
    public int RoundsUntilAction => roundsUntilAction;


    

    public virtual void PerformAction(AI ai, Player player)
    {
        Debug.Log("AI not given action");
    }
    
    
}
