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
		if (cardData[randomIndex].cardata != null)
		{
			for(int i = 0; cardData[randomIndex].cardata.Length > i; i++)
			{
				promptManager.CreatePrompt(cardData[randomIndex].cardata[i]);
				Debug.Log($"Played card: {cardData[randomIndex].cardata[i].cardName}");
			}
		}
		else
		{
			promptManager.CreatePrompt(cardData[randomIndex]);
			Debug.Log($"Played card: {cardData[randomIndex].cardName}");
		}
		
	}

	public void PlayCardData(CardData data)
	{
		Debug.Log($"Playing card: {data.cardName}");
		if (data.cardata.Length > 0)
		{
			Debug.Log("Card has multiple sub-cards, playing each one:");
			for (int i = 0; data.cardata.Length > i; i++)
			{
				promptManager.CreatePrompt(data.cardata[i]);
				Debug.Log($"Played card: {data.cardata[i].cardName}");
			}
		}
		else
		{
			Debug.Log("Card has a single effect, playing it:");
			promptManager.CreatePrompt(data);
			Debug.Log($"Played card: {data.cardName}");
		}
	}
}
