using UnityEngine;

public abstract class AIAction : ScriptableObject
{
    public string actionName;
    public int roundsUntilAction;

    

    public virtual void PerformAction(AI ai)
    {
        Debug.Log("AI not given action");
    }
    
    
}
