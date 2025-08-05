using UnityEngine;

public class PlayCard : MonoBehaviour
{
	[SerializeField]
	CardData cardData;

	PromptManager promptManager;

	private void Awake()
	{
		promptManager = FindFirstObjectByType<PromptManager>();
	}

	public void PlayCards()
	{
		promptManager.CreatePrompt(cardData);
	}
}
