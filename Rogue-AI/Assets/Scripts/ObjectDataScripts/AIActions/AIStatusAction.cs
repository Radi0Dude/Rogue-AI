using UnityEngine;

[CreateAssetMenu(fileName = "StatusAction", menuName = "AI/Actions/AI Status")]

public class AIStatusAction : AIActionData
{
    [Header("Heal Action Specifics")]
    [SerializeField] private CardData[] statusCards;
    
    
    public override void PerformAction(AI ai, CombatManager cm)
    {
        foreach (var statusCard in statusCards)
        {
            cm.AddCardToCombat(statusCard);
        }
    }
}
