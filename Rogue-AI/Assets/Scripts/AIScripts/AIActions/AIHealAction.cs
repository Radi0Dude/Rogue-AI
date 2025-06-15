using UnityEngine;

[CreateAssetMenu(fileName = "AIHealAction", menuName = "AI/Actions/AI Heal")]

public class AIHealAction : AIAction
{
    [Header("Amount of sanity lost")]
    public int healAmount;
    public override void PerformAction(AI ai)
    {
        Debug.Log("AI Perform Heal action");
        
        ai.ChangeSanity(-healAmount);
    }
}
