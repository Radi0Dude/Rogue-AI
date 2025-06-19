using UnityEngine;

public abstract class AIActionData : ScriptableObject
{
    public string actionName;
    public int roundsUntilAction;

    

    public virtual void PerformAction(AI ai, Player player)
    {
        Debug.Log("AI not given action");
    }
    
    
}
