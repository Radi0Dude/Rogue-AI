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

	[SerializeField] string currentPromptTag;

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

		
		segments[cardData.promptPlacement] = GetRandomPrompt(cardData); // or .GetRandomAddition()

		SetPrompt();
	}

	string GetRandomPrompt(CardData cardata)
	{
		return cardata.cardPromptUpdate[Random.Range(0, cardata.cardPromptUpdate.Length)];
	}

	public void SetPrompt()
	{
		fullPrompt = $"{segments[PromptPlacement.Front]} " +
					 $"{segments[PromptPlacement.StartPromptStart]} " +
					 $"{segments[PromptPlacement.Middle]} " +
					 $"{segments[PromptPlacement.StartPromptEnd]} " +
					 $"{segments[PromptPlacement.End]}".Trim();

		Debug.Log("Full Prompt: " + fullPrompt);
	}
}
[System.Serializable]
public class StartPromptList
{
	public string startPrompt;
	public string endPrompt;
}