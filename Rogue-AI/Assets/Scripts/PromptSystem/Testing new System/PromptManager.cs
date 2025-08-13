using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using System.Linq;

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

	bool hasStartedWriting = false;
	[SerializeField] TMP_Text promptText;

	private Dictionary<PromptPlacement, string> segments = new()
	{
		{ PromptPlacement.Front, "" },
		{ PromptPlacement.StartPromptStart, "" },
		{ PromptPlacement.Middle, "" },
		{ PromptPlacement.StartPromptEnd, "" },
		{ PromptPlacement.End, "" }
	};

	private Dictionary<PromptPlacement, string> segmentsCopy = new()
	{
		{ PromptPlacement.Front, "" },
		{ PromptPlacement.StartPromptStart, "" },
		{ PromptPlacement.Middle, "" },
		{ PromptPlacement.StartPromptEnd, "" },
		{ PromptPlacement.End, "" }
	};

	// fixed order used everywhere
	static readonly PromptPlacement[] Order = {
		PromptPlacement.Front,
		PromptPlacement.StartPromptStart,
		PromptPlacement.Middle,
		PromptPlacement.StartPromptEnd,
		PromptPlacement.End
	};

	private void Start()
	{
		if (startingPrompts == null || startingPrompts.Count == 0)
		{
			Debug.LogWarning("No starting prompts configured.");
		}
		else
		{
			GetStartPrompt();
		}
		StartCoroutine(ConstantUpdateTyping());
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
		
		if (!segments.ContainsKey(cardData.promptPlacement[0]))
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
		segments[cardData.promptPlacement[0]] = picked; // this wont work yet, i need to change it to work with multiple systems

		SetPrompt();
	}

	string GetRandomPrompt(CardData cardData)
	{
		var tags = cardData.cardPromptUpdate.tags;
		List<string> prompts = new List<string>();
		foreach (var prompt in cardData.cardPromptUpdate.prompt)
		{
			prompts.AddRange(prompt.prompts);
		}
		//var prompts = cardData.cardPromptUpdate.prompt[0].prompts;

		if (tags == null || prompts == null || tags.Length == 0 || prompts.Count == 0 || tags.Length != prompts.Count)
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
		// Build normalized fullPrompt (lowercase after first char), BEFORE typing
		fullPrompt = (
			$"{segments[PromptPlacement.Front]} " +
			$"{segments[PromptPlacement.StartPromptStart]} " +
			$"{segments[PromptPlacement.Middle]} " +
			$"{segments[PromptPlacement.StartPromptEnd]} " +
			$"{segments[PromptPlacement.End]}"
		).Trim();

		if (!string.IsNullOrEmpty(fullPrompt))
		{
			char first = fullPrompt[0];
			string rest = fullPrompt.Substring(1).ToLowerInvariant();
			fullPrompt = first + rest;
		}
		fullPrompt = Regex.Replace(fullPrompt, @"\s{2,}", " ");

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

		
		hasStartedWriting = false;
		StopCoroutine(ConstantUpdateTyping());

		StartCoroutine(WritePromptBySegments());

		foreach (var p in Order) segmentsCopy[p] = segments[p];
	}

	IEnumerator WritePromptBySegments()
	{
		if (promptText == null) yield break;

		var oldSegs = SnapshotLower(segmentsCopy);
		var newSegs = SnapshotLower(segments);

		string oldRenderedLower = JoinSegments(oldSegs); 
		string oldRendered = CapitalizeFirst(oldRenderedLower);

		promptText.text = oldRendered;

		var layout = BuildLayout(oldSegs, out _);

		foreach (var place in Order)
		{
			string oldT = oldSegs[place];
			string newT = newSegs[place];
			if (oldT == newT) continue;

			// Figure out append vs prepend, else fall back to full insert
			bool isAppend = newT.StartsWith(oldT);
			bool isPrepend = newT.EndsWith(oldT);

			string delta = isAppend ? newT.Substring(oldT.Length)
						 : isPrepend ? newT.Substring(0, newT.Length - oldT.Length)
						 : newT;

			// Auto-space at boundary if needed
			if (!string.IsNullOrEmpty(oldT) && !string.IsNullOrEmpty(delta))
			{
				if (isPrepend && delta[^1] != ' ' && oldT[0] != ' ') delta += " ";
				if (isAppend && oldT[^1] != ' ' && delta[0] != ' ') delta = " " + delta;
			}
			delta = Regex.Replace(delta, @"\s{2,}", " ");

			// Where do we insert in the CURRENT text?
			if (!layout.TryGetValue(place, out var info))
			{
				info = (promptText.text.Length, 0);
			}
			int insertIndex = isAppend ? info.start + info.len : info.start;
			insertIndex = ClampIndex(promptText.text, insertIndex);

			// If we’re inserting at very start and nothing is on screen yet, capitalize first char
			bool capFirst = insertIndex == 0 && string.IsNullOrEmpty(promptText.text);

			// Insert characters one by one (no replacement)
			for (int i = 0; i < delta.Length; i++)
			{
				char ch = (capFirst && i == 0) ? char.ToUpperInvariant(delta[i]) : delta[i];
				int clamped = ClampIndex(promptText.text, insertIndex + i);
				promptText.text = promptText.text.Insert(clamped, ch.ToString());

				// caret pulse
				promptText.text += "|";
				yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
				promptText.text = promptText.text.TrimEnd('|');
			}

			// Update our "old" model and layout for next segment
			oldSegs[place] = (isPrepend ? (delta + oldT) : (oldT + delta)).Trim();
			layout = BuildLayout(oldSegs, out _);
		}

		// Resume caret
		hasStartedWriting = true;
		StartCoroutine(ConstantUpdateTyping());
	}

	// ---------- helpers ----------

	Dictionary<PromptPlacement, string> SnapshotLower(Dictionary<PromptPlacement, string> src)
	{
		var snap = new Dictionary<PromptPlacement, string>();
		foreach (var p in Order)
		{
			var s = (src.TryGetValue(p, out var v) ? v : "") ?? "";
			s = Regex.Replace(s, @"\s{2,}", " ").Trim().ToLowerInvariant();
			snap[p] = s;
		}
		return snap;
	}

	string JoinSegments(Dictionary<PromptPlacement, string> segs)
	{
		var parts = new List<string>();
		foreach (var p in Order)
		{
			var s = segs[p];
			if (!string.IsNullOrWhiteSpace(s)) parts.Add(s.Trim());
		}
		var joined = string.Join(" ", parts);
		joined = Regex.Replace(joined, @"\s{2,}", " ").Trim();
		return joined; // lowercased
	}

	Dictionary<PromptPlacement, (int start, int len)> BuildLayout(Dictionary<PromptPlacement, string> segs, out string builtLower)
	{
		var map = new Dictionary<PromptPlacement, (int, int)>();
		var sb = new StringBuilder();
		bool first = true;

		foreach (var p in Order)
		{
			string t = (segs[p] ?? "").Trim();
			int start = sb.Length;

			if (!string.IsNullOrEmpty(t))
			{
				if (!first) sb.Append(' ');
				start = sb.Length;
				sb.Append(t);
				map[p] = (start, t.Length);
				first = false;
			}
			else
			{
				map[p] = (start, 0); // where it would start
			}
		}

		builtLower = sb.ToString();
		builtLower = Regex.Replace(builtLower, @"\s{2,}", " ").Trim();
		return map;
	}

	int ClampIndex(string s, int i) => Mathf.Clamp(i, 0, s?.Length ?? 0);

	string CapitalizeFirst(string s)
	{
		if (string.IsNullOrEmpty(s)) return s;
		return char.ToUpperInvariant(s[0]) + (s.Length > 1 ? s.Substring(1) : "");
	}

	IEnumerator ConstantUpdateTyping()
	{
		while (hasStartedWriting)
		{
			if (!promptText.text.EndsWith("|"))
			{
				promptText.text += "|";
			}
			else
			{
				promptText.text = promptText.text.TrimEnd('|');
			}
			yield return new WaitForSeconds(0.33f);
		}
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