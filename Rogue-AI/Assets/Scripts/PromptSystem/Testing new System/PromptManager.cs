using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


public enum PromptPlacement
{
	Front,
	StartPromptStart,
	Middle,
	StartPromptEnd,
	End
}

public class PromptManager : MonoBehaviour
{
	[SerializeField]
	List<StartPromptList> startingPrompts = new();

	List<string> currentPromt = new List<string>();
	string fullPrompt = "";
	List<Prompts> promptData = new List<Prompts>();

	CardList cardList = new CardList();

	List<string> segmenets;

	[SerializeField]
	string front; 
	[SerializeField]
	string start; 
	[SerializeField]
	string middle; 
	[SerializeField]
	string originalEnd; 
	[SerializeField]
	string end;

	PromptPlacement promptPlacement;

	private void Start()
	{
		segmenets = new List<string>()
		{
			front,
			start,
			middle,
			originalEnd,
			end
		};
		GetStartPrompt();
	}

	public void CreatePrompt(CardData cardData)
	{
		for(int i = 0; i < System.Enum.GetValues(typeof(PromptPlacement)).Length; i++) 
		{
			if(cardData.promptPlacement == (PromptPlacement)i)
			{
				segmenets[i] = cardData.cardName;
				break;
			}
		}
		if (segmenets.Count >= 5)
		{
			front = segmenets[0];
			start = segmenets[1];
			middle = segmenets[2];
			originalEnd = segmenets[3];
			end = segmenets[4];
		}
		else
		{
			Debug.LogError("Prompt segments are not properly set up. Please check the PromptManager.");
			return;
		}
			SetPrompt();
	}

	public void SetPrompt()
	{
		 fullPrompt = $"{front} {start} {middle} {originalEnd} {end}".Trim();
	}

	


	public void GetStartPrompt()
	{
		int RandomIndex = Random.Range(0, startingPrompts.Count);

		start = startingPrompts[RandomIndex].startPrompt;
		originalEnd = startingPrompts[RandomIndex].endPrompt;
		
	}

	public void UpdatePromptWithCard(string promptAddition, string placement, string[] newtags)
	{

	}
}
[System.Serializable]
public class StartPromptList
{
	public string startPrompt;
	public string endPrompt;
}