using System;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField] private InputField inputField;


    private void OnEnable()
    {
        inputField.EndingTurnEvent += EndTurn;
    }

    public void EndTurn()
    {
        
    }

    private void StartTurn()
    {
        // If there are no prompt present, give a new prompt
        // Player draws card
    }

    private void EnemyAdvancement()
    {
        
    }

    private void BeginCombat()
    {
        
    }

    private void EndCombat()
    {
        
    }
}
