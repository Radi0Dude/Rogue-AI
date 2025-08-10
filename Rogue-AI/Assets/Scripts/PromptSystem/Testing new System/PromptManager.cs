using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PromptPlacement
{
	Front,
	StartPromptStart,
	Middle,
	StartPromptEnd,
	End
}

[RequireComponent(typeof(PromptButtonTag))]

public class PromptManager : MonoBehaviour
{
	[SerializeField] List<StartPromptList> startingPrompts = new();
	[SerializeField] string fullPrompt = "";

	[SerializeField] Tags currentPromptTag;
	bool hasbeenSet = false;

	List<CardData> playedCards = new();

	public GameObject alreadyPlayedCardText;

	private Dictionary<PromptPlacement, string> segments = new()
	{
		{ PromptPlacement.Front, "" },
		{ PromptPlacement.StartPromptStart, "" },
		{ PromptPlacement.Middle, "" },
		{ PromptPlacement.StartPromptEnd, "" },
		{ PromptPlacement.End, "" }
	};

	private void Start()
	{
		GetStartPrompt();
	}

	public void GetStartPrompt()
	{
		int randomIndex = Random.Range(0, startingPrompts.Count);
		var prompt = startingPrompts[randomIndex];

		segments[PromptPlacement.StartPromptStart] = prompt.startPrompt;
		segments[PromptPlacement.StartPromptEnd] = prompt.endPrompt;

		SetPrompt();
	}

	public void CreatePrompt(CardData cardData)
	{
		if (!segments.ContainsKey(cardData.promptPlacement))
		{
			Debug.LogWarning("Invalid prompt placement.");
			return;
		}


		segments[cardData.promptPlacement] = GetRandomPrompt(cardData); 

		SetPrompt();
	}

	string GetRandomPrompt(CardData cardata)
	{
		CardData cardData = cardata;
		int randomIndex = Random.Range(0, cardData.cardPromptUpdate.prompt.Length);
		if(hasbeenSet == false)
		{
			currentPromptTag = cardData.cardPromptUpdate.tags[randomIndex];
			hasbeenSet = true;
			return cardata.cardPromptUpdate.prompt[randomIndex];
		}
		else
		{
			if(playedCards.Contains(cardData))
			{
				StartCoroutine(AlreadyPlayedCard());
				return "";
			}
			while (currentPromptTag != cardData.cardPromptUpdate.tags[randomIndex])
			{
				randomIndex = Random.Range(0, cardData.cardPromptUpdate.prompt.Length);
			}
			playedCards.Add(cardData);
			return cardata.cardPromptUpdate.prompt[randomIndex] ;
		}
		
	}

	public void SetPrompt()
	{
		fullPrompt = $"{segments[PromptPlacement.Front]}" +
					 $"{segments[PromptPlacement.StartPromptStart]}" +
					 $"{segments[PromptPlacement.Middle]}" +
					 $"{segments[PromptPlacement.StartPromptEnd]}" +
					 $"{segments[PromptPlacement.End]}".Trim();

		Debug.Log("Full Prompt: " + fullPrompt);
		segments[PromptPlacement.StartPromptStart] = segments[PromptPlacement.Front] + " " +
													 segments[PromptPlacement.StartPromptStart];
		string original = segments[PromptPlacement.StartPromptStart];
		string result = "";

		for (int i = 0; i < original.Length; i++)
		{
			char c = original[i];
			if (i != 0)
				c = char.ToLower(c);
			result += c;
		}

		
		segments[PromptPlacement.StartPromptStart] = result;
		Debug.Log("Start Prompt Start: " + segments[PromptPlacement.StartPromptStart]);
		
	}

	IEnumerator AlreadyPlayedCard()
	{
		alreadyPlayedCardText.SetActive(true);
		yield return new WaitForSeconds(2f);
		alreadyPlayedCardText.SetActive(false);
	}
}
[System.Serializable]
public class StartPromptList
{
	public string startPrompt;
	public string endPrompt;
}