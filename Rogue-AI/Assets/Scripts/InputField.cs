using System;
using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.Android;

public class InputField : MonoBehaviour
{
    [SerializeField] private TMP_Text promptText;
    
    public event Action OnEndingTurnEvent;
    public event Action<List<PromptType>> OnSendPromptEvent;

    
    private string currentPrompt = "Prompt";
    
    private List<PromptType> _promptTypes = new();


    private void Start()
    {
        // Find a prompt starter
        //GetNewPrompt();
        
        UpdateVisual();
    }

    private void GetNewPrompt()
    {
        // Returns start of new prompt for the player to adjust
    }

    public void UpdatePrompt(PromptType promptType)
    {
        currentPrompt = promptType + " " + promptText.text;
        _promptTypes.Add(promptType);
        
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        promptText.text = currentPrompt;

    }
    
    public void SendPrompt()
    {
        OnSendPromptEvent?.Invoke(_promptTypes);
        
        currentPrompt = "Prompt";
        UpdateVisual();
        
        _promptTypes.Clear();
    }

    public void EndTurn()
    {
        // Use event to call Combat manager
        OnEndingTurnEvent?.Invoke();
    }
}
