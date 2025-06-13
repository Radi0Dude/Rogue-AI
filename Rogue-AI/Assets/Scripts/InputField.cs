using System;
using TMPro;
using UnityEngine;

public class InputField : MonoBehaviour
{
    [SerializeField] private TMP_Text promptText;
    private string currentPrompt = "Prompt";


    private void Start()
    {
        UpdateVisual();
    }

    public void UpdatePrompt(PromptType promptType)
    {
        currentPrompt = promptType + " " + promptText.text;
        
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        promptText.text = currentPrompt;

    }
    
    public void SendPrompt()
    {
        
        
        EndTurn();
    }

    public void EndTurn()
    {
        
    }
}
