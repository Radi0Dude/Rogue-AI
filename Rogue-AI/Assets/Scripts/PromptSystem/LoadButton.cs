using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadButton : MonoBehaviour
{
	private LoadUpPrompts loadUpPrompts;
	private PromptSystem promptSystem;

	[SerializeField]
	public List<CardData> cards = new List<CardData>();

	[SerializeField]
	TMP_Text promptText;

	private void Awake()
	{
		loadUpPrompts = FindFirstObjectByType<LoadUpPrompts>();
		promptSystem = FindFirstObjectByType<PromptSystem>();

		if (promptSystem != null)
			cards = promptSystem.cards;
	}

	private void Start()
	{
		UpdatePromptText();
	}

	public void LoadPrompt()
	{
		if (cards == null || cards.Count == 0)
		{
			Debug.LogWarning("No cards available to load prompts.");
			return;
		}

		int randomNum = Random.Range(0, cards.Count);
		var selectedCard = cards[randomNum];

		Debug.Log($"Selected card: {selectedCard.cardName}");

		loadUpPrompts.GetCurrentCard(selectedCard);

		// Safeguard: ensure the current prompt was actually updated
		if (!string.IsNullOrEmpty(loadUpPrompts.currentPrompt))
		{
			UpdatePromptText();
		}
		else
		{
			Debug.LogWarning("No prompt text found after card selection.");
		}
	}

	void UpdatePromptText()
	{
		if (promptText != null)
		{
			promptText.text = loadUpPrompts.currentPrompt;
		}
		else
		{
			Debug.LogWarning("Prompt text UI reference is missing.");
		}
	}
}
