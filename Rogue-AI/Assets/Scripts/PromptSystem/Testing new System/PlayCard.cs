using UnityEngine;

public class PlayCard : MonoBehaviour
{
	[SerializeField]
	CardData[] cardData;

	PromptManager promptManager;

	private void Awake()
	{
		promptManager = FindFirstObjectByType<PromptManager>();
	}

	public void PlayCards()
	{
		int randomIndex = Random.Range(0, cardData.Length);
		promptManager.CreatePrompt(cardData[randomIndex]);
	}
}
