using System.Collections.Generic;
using UnityEngine;

public class PromptManager : MonoBehaviour
{

	List<string> currentPromt = new List<string>();
	string fullPrompt = "";
	List<Prompts> promptData = new List<Prompts>();

	CardList cardList = new CardList();

	

	public void SetPrompt(string prompt)
	{
		for (int i = 0; i < prompt.Length; i++) 
		{
			for(int j = 0; j < cardList.cardTypes.Length; j++)
			{
				if (prompt[i].ToString() == cardList.cardTypes[j])
				{
					continue;
				}
			}
			fullPrompt += prompt[i];
		}

	
		Debug.Log("Current Prompt Set: " + currentPromt);
	}

	public void GetStartPrompt()
	{
		int RandomIndex = Random.Range(0, promptData.Count);

		SetPrompt(promptData[RandomIndex].promptText);
	}

	public void UpdatePromptWithCard(string promptAddition, string placement, string[] newtags)
	{

	}
}