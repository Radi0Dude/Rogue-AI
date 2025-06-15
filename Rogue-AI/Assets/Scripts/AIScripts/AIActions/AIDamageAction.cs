using System;
using UnityEngine;

[CreateAssetMenu(fileName = "AIDamageAction", menuName = "AI/Actions/AI Damage")]
public class AIDamageAction : AIAction
{
    
    public override void PerformAction()
    {
        Debug.Log("AI Perform Damage action");

    }
}
