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
			return;

		if (playedCards.Contains(cardData))
		{
			if (alreadyPlayedCardText) StartCoroutine(AlreadyPlayedCard());
			return;
		}

		var picked = GetRandomPrompt(cardData);
		if (string.IsNullOrEmpty(picked))
			return;

		playedCards.Add(cardData);
		segments[cardData.promptPlacement] = picked;
		SetPrompt();
	}

	string GetRandomPrompt(CardData cardData)
	{
		var tags = cardData.cardPromptUpdate.tags;
		var prompts = cardData.cardPromptUpdate.prompt;

		if (tags == null || prompts == null || tags.Length == 0 || prompts.Length == 0 || tags.Length != prompts.Length)
			return "";

		if (!hasbeenSet)
		{
			int idx = Random.Range(0, tags.Length);
			currentPromptTag = tags[idx];
			hasbeenSet = true;
			return prompts[idx];
		}

		const int maxTries = 1000;
		int tries = 0;

		int randomIndex = Random.Range(0, tags.Length);
		while (currentPromptTag != tags[randomIndex] && tries++ < maxTries)
			randomIndex = Random.Range(0, tags.Length);

		if (tries >= maxTries)
			return "";

		return prompts[randomIndex];
	}

	public void SetPrompt()
	{
		fullPrompt = (
			$"{segments[PromptPlacement.Front]}" +
			$"{segments[PromptPlacement.StartPromptStart]}" +
			$"{segments[PromptPlacement.Middle]}" +
			$"{segments[PromptPlacement.StartPromptEnd]}" +
			$"{segments[PromptPlacement.End]}"
		).Trim();

		Debug.Log("Full Prompt: " + fullPrompt);

		string startStartDisplay = (segments[PromptPlacement.Front] + " " +
									segments[PromptPlacement.StartPromptStart]).TrimStart();

		if (!string.IsNullOrEmpty(startStartDisplay))
		{
			char first = startStartDisplay[0];
			string rest = startStartDisplay.Substring(1).ToLowerInvariant();
			startStartDisplay = first + rest;
		}

		Debug.Log("Start Prompt Start: " + startStartDisplay);
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