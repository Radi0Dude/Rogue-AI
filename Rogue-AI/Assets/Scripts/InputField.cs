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

    
    public event Action<PromptType> OnPlayCard;
    
    
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
    }

    public void UpdatePrompt(PromptType promptType)
    {
        // TODO: Lines of code that connects Tobias' prompt generator
        OnPlayCard?.Invoke(promptType);
        
        _currentPrompt = promptType + " " + promptText.text;
        _promptTypes.Add(promptType);
        
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
        
        _currentPrompt = "Prompt";
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
