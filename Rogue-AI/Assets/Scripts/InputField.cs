using System;
using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.Android;

public class InputField : MonoBehaviour
{
    [SerializeField] private TMP_Text promptText;
    [SerializeField] TooltipText sendInTooltipText;
    public event Action OnEndingTurnEvent;
    public event Action<List<PromptType>> OnSendPromptEvent;
    
    
    private string _currentPrompt = "Prompt";
    
    private List<PromptType> _promptTypes = new();


    private void Start()
    {
        // Find a prompt starter
        //GetNewPrompt();
        
        UpdateVisual();
        sendInTooltipText.SetTooltipText("Send prompt to AI and end your turn");
    }

    public void GetNewPrompt()
    {
        // Returns start of new prompt for the player to adjust
        // TODO: Connect to Tobias' prompt generator
        _currentPrompt = "Prompt";

    }

    public void UpdatePrompt(CardData data)
    {
        // TODO: Lines of code that connects Tobias' prompt generator
        // Her er data til kortet :D

        var promptType = data.PromptType;
        foreach (PromptType flag in Enum.GetValues(typeof(PromptType)))
        {
            if (flag != 0 && promptType.HasFlag(flag))
            {
                if (!_promptTypes.Contains(flag))
                    _promptTypes.Add(flag);
            }
        }
        
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        promptText.text = _currentPrompt;

    }
    
    public void SendPrompt()
    {
        if (GameManager.RoundState == RoundState.EndCombat) return;
        OnSendPromptEvent?.Invoke(_promptTypes);
        
        GetNewPrompt();
        UpdateVisual();
        
        _promptTypes.Clear();
    }

    public void EndTurn()
    {
        if (GameManager.RoundState == RoundState.EndCombat) return;
        // Use event to call Combat manager
        OnEndingTurnEvent?.Invoke();
    }
}
