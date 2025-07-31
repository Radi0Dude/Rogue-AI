using System;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "DamageAction", menuName = "AI/Actions/AI Damage")]
public class AIDamageAction : AIActionData
{
    [Header("Damage Action Specifics")]
    [SerializeField] private int minDamage;
    [SerializeField] private int maxDamage;
    
    
    public override void PerformAction(AI ai, CombatManager cm)
    {
        Debug.Log("AI Perform Damage action");

        var damage = Random.Range(minDamage, maxDamage+1);
        cm.ChangeHealth(-damage);
    }
}
