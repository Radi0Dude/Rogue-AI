using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadButton : MonoBehaviour
{
	LoadUpPrompts loadUpPrompts;
	PromptSystem promptSystem;
	[SerializeField] public List<CardData> cards = new List<CardData>();
	[SerializeField]
	TMP_Text promptText;

	private void Awake()
	{
		loadUpPrompts = FindFirstObjectByType<LoadUpPrompts>();
		promptSystem = FindFirstObjectByType<PromptSystem>();
		cards = promptSystem.cards;
		
	}
	private void Start()
	{
		promptText.text = loadUpPrompts.currentPrompt;



	}
	public void LoadPrompt()
	{
		int randomNum = Random.Range(0, cards.Count);
		loadUpPrompts.GetCurrentCard(cards[randomNum]);
		promptText.text = loadUpPrompts.currentPrompt;

	}
}
