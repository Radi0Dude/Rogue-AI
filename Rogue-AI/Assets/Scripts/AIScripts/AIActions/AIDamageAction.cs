using System;
using UnityEngine;

[CreateAssetMenu(fileName = "AIDamageAction", menuName = "AI/Actions/AI Damage")]
public class AIDamageAction : AIActionData
{
    public int damage;
    
    public override void PerformAction(AI ai, Player player)
    {
        Debug.Log("AI Perform Damage action");

        player.ChangeHealth(-damage);
    }
}
