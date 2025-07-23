using UnityEngine;

[CreateAssetMenu(fileName = "StatusAction", menuName = "AI/Actions/AI Status")]

public class AIStatusAction : AIActionData
{
    [Header("Heal Action Specifics")]
    [SerializeField] private CardData statusCard;
    
    
    public override void PerformAction(AI ai, Player player)
    {
        Debug.Log("AI Perform Heal action");

        player.AddCardToCombat(statusCard);
    }
}
