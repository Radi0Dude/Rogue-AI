using UnityEngine;

[CreateAssetMenu(fileName = "HealAction", menuName = "AI/Actions/AI Heal")]

public class AIHealAction : AIActionData
{
    [Header("Heal Action Specifics")]
    [SerializeField] private int healAmount;
    
    
    public override void PerformAction(AI ai, Player player)
    {
        Debug.Log("AI Perform Heal action");
        
        ai.ChangeSanity(-healAmount);
    }
}
