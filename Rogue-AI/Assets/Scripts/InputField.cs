using System;
using TMPro;
using UnityEngine;
using UnityEngine.Android;

public class InputField : MonoBehaviour
{
    [SerializeField] private TMP_Text promptText;
    
    public event Action EndingTurnEvent;
    
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
        //TODO: Send prompt to the prompt manager, and gain the amount of sanity that should be given to the AI
        
        
        EndTurn();
    }

    public void EndTurn()
    {
        // Use event to call Combat manager
        EndingTurnEvent?.Invoke();
    }
}
