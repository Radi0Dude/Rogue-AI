using System.Collections.Generic;
using UnityEngine;

public class LoadButton : MonoBehaviour
{
	LoadUpPrompts loadUpPrompts;
	PromptSystem promptSystem;
	[SerializeField] public List<CardData> cards = new List<CardData>();

	private void Awake()
	{
		loadUpPrompts = FindFirstObjectByType<LoadUpPrompts>();
		promptSystem = FindFirstObjectByType<PromptSystem>();
		cards = promptSystem.cards;
	}
	
	public void LoadPrompt()
	{

	}
}
