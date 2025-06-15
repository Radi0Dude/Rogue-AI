using System;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    private InputField _inputField;

    private Player _player;
    private AI _ai;
    
    
    public void BeginCombat()
    {
        _inputField.EndingTurnEvent += EndTurn;
    }
    
   
    private void StartTurn()
    {
        // If there are no prompt present, give a new prompt
        // Player draws card
    }
    
    public void EndTurn()
    {
        EnemyAdvancement();
    }


    private void EnemyAdvancement()
    {
        _ai.ReduceCountDown();
    }

    
    public void EndCombat()
    {
        // Sends the player to road select screen
        _inputField.EndingTurnEvent -= EndTurn;

    }
}
