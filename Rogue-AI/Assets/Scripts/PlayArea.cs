using UnityEngine;

public class PlayArea : MonoBehaviour
{
    [SerializeField] private InputField inputField;
    
    public void PlayCard(Card card)
    {
        var cardData = card.GetData();
        
        inputField.UpdatePrompt(cardData.promptType);
    }
}
