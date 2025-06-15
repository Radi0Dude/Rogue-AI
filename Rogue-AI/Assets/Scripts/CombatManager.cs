using System;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    private InputField _inputField;

    private Player _player;
    private AI _ai;

    private void Start()
    {
        BeginCombat();
    }

    private void BeginCombat()
    {
        _inputField = FindAnyObjectByType<InputField>();
        _player = FindAnyObjectByType<Player>();
        _ai = FindAnyObjectByType<AI>();

        if (_inputField != null)
        {
            _inputField.OnEndingTurnEvent += EndTurn;
            _inputField.OnSendPromptEvent += SendPrompt;
        }
        else
        {
            Debug.LogWarning("InputField not found.");
        }

        if (_player != null)
        {
            _player.OnPlayerDeath += EndCombat;
        }
        else
        {
            Debug.LogWarning("Player not found.");
        }

        if (_ai != null)
        {
            _ai.OnAISane += LoseCombat;
            _ai.Initialize(_player);
        }
        else
        {
            Debug.LogWarning("AI not found.");
        }
        
        StartTurn();
        
    }

 

    private void StartTurn()
    {
        // If there are no prompt present, give a new prompt
        // Player draws card
        _player.DrawHand();
    }
    
    private void SendPrompt(List<PromptType> prompts)
    {
        //TODO: Send prompt to the prompt manager, and gain the amount of sanity that should be given to the AI

        Debug.Log("Sending Prompt.");
        EndTurn();
    }
    
    private void EndTurn()
    {
        Debug.Log("Ending Turn");
        _player.DiscardAllCards();
        EnemyAdvancement();
    }



    private void EnemyAdvancement()
    {
        _ai.ReduceCountDown();
        StartTurn();
    }

    
    public void EndCombat()
    {
        // Sends the player to road select screen
        
    }
    
    private void LoseCombat()
    {
        // Game Over the player
    }

    private void OnDisable()
    {
        _inputField.OnEndingTurnEvent -= EndTurn;
        _inputField.OnSendPromptEvent -= SendPrompt;
        _player.OnPlayerDeath -= EndCombat;
        _ai.OnAISane -= LoseCombat;
    }

}
