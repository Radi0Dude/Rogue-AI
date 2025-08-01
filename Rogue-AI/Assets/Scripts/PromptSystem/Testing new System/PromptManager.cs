using System.Collections.Generic;
using UnityEngine;

public class PromptManager : MonoBehaviour
{

	string currentPromt;

	List<Prompts> promptData = new List<Prompts>();

	public void SetPrompt(string prompt)
	{
		currentPromt = prompt;
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