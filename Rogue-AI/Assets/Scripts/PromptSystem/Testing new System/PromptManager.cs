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
		if (cardData == null || cardData.promptPlacement == null || cardData.promptPlacement.Length == 0)
			return;

		// Ensure at least one placement maps to our segments dictionary
		bool anyValid = false;
		foreach (var p in cardData.promptPlacement)
			if (segments.ContainsKey(p)) { anyValid = true; break; }
		if (!anyValid) return;

		if (playedCards.Contains(cardData))
		{
			if (alreadyPlayedCardText) StartCoroutine(AlreadyPlayedCard());
			return;
		}

		var picked = GetRandomPrompt(cardData);
		if (string.IsNullOrWhiteSpace(picked))
			return;

		// 1) Try first EMPTY allowed placement (by the order defined on the card)
		PromptPlacement? target = null;
		foreach (var p in cardData.promptPlacement)
		{
			if (!segments.ContainsKey(p)) continue;
			if (string.IsNullOrWhiteSpace(segments[p]))
			{
				target = p;
				break;
			}
		}

		if (target.HasValue)
		{
			// Always append (handles empty existing too)
			segments[target.Value] = AppendWithSpace(segments[target.Value], picked.Trim());
		}
		else
		{
			// 2) If all allowed are filled, append to the LAST allowed one
			var last = cardData.promptPlacement[cardData.promptPlacement.Length - 1];
			if (!segments.ContainsKey(last)) return;
			segments[last] = AppendWithSpace(segments[last], picked.Trim());
		}

		playedCards.Add(cardData);
		SetPrompt();
	}

	private string AppendWithSpace(string existing, string addition)
	{
		existing = existing ?? "";
		addition = addition ?? "";
		existing = existing.TrimEnd();
		addition = addition.TrimStart();
		if (existing.Length == 0) return addition;
		if (!existing.EndsWith(" ")) existing += " ";
		return existing + addition;
	}

	string GetRandomPrompt(CardData cardData)
	{
		var tags = cardData.cardPromptUpdate.tags;
		List<string> prompts = new List<string>();
		foreach (var p in cardData.cardPromptUpdate.prompt)
		{
			prompts.AddRange(p.prompts);
		}

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

		// Start from old rendered state (proper spaces), cap first char
		string start = CapitalizeFirst(JoinSegments(oldSegs));
		promptText.text = start;

		// Animate each segment change using insert-only (no mid-typing deletions)
		foreach (var place in Order)
		{
			string oldT = oldSegs[place];
			string newT = newSegs[place];
			if (oldT == newT) continue;

			// Build canonical target for this step
			var step = new Dictionary<PromptPlacement, string>(oldSegs);
			step[place] = newT;
			string targetStep = CapitalizeFirst(JoinSegments(step));

			yield return TypeToTargetGentle(targetStep);

			// Update model; keep what's on screen (no snap here)
			oldSegs[place] = newT;
		}

		// Final snap to the full target: clean spaces/case and remove any leftovers at once
		promptText.text = CapitalizeFirst(JoinSegments(newSegs));

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

	IEnumerator TypeToTargetGentle(string target)
	{
		// Work without caret artifacts
		string cur = promptText.text?.Replace("|", "") ?? "";
		target ??= "";
		if (target == cur) yield break;

		// PREPEND: missing prefix at front
		if (target.EndsWith(cur))
		{
			string prefix = target.Substring(0, target.Length - cur.Length);

			// First-ever write: type forward left→right
			if (cur.Length == 0)
			{
				for (int i = 0; i < prefix.Length; i++)
				{
					char ch = (i == 0) ? char.ToUpperInvariant(prefix[i]) : prefix[i];
					int insertAt = Mathf.Clamp(i, 0, promptText.text.Length);
					promptText.text = promptText.text.Insert(insertAt, ch.ToString());

					promptText.text += "|";
					yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
					promptText.text = promptText.text.TrimEnd('|');
				}
				yield break;
			}

			// Normal prepend: insert prefix in REVERSE at index 0 (so it appears left→right)
			for (int i = prefix.Length - 1; i >= 0; i--)
			{
				char ch = (i == 0) ? char.ToUpperInvariant(prefix[i]) : prefix[i];
				promptText.text = promptText.text.Insert(0, ch.ToString());

				promptText.text += "|";
				yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
				promptText.text = promptText.text.TrimEnd('|');
			}
			yield break;
		}

		// APPEND: missing suffix at end
		if (target.StartsWith(cur))
		{
			for (int j = cur.Length; j < target.Length; j++)
			{
				int insertAt = Mathf.Clamp(j, 0, promptText.text.Length);
				promptText.text = promptText.text.Insert(insertAt, target[j].ToString());

				promptText.text += "|";
				yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
				promptText.text = promptText.text.TrimEnd('|');
			}
			yield break;
		}

		// MIDDLE CHANGE: insert only the middle chunk between LCP and LCS (no deletion)
		int lcp = 0;
		int max = Mathf.Min(cur.Length, target.Length);
		while (lcp < max && cur[lcp] == target[lcp]) lcp++;

		int ci = cur.Length - 1, ti = target.Length - 1;
		int lcs = 0;
		while (ci - lcs >= lcp && ti - lcs >= lcp && cur[ci - lcs] == target[ti - lcs]) lcs++;

		int tgtMidStart = lcp;
		int tgtMidEnd = target.Length - lcs; // exclusive
		if (tgtMidEnd <= tgtMidStart) yield break;

		for (int j = tgtMidStart; j < tgtMidEnd; j++)
		{
			int insertAt = Mathf.Clamp(j, 0, promptText.text.Length);
			promptText.text = promptText.text.Insert(insertAt, target[j].ToString());

			promptText.text += "|";
			yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
			promptText.text = promptText.text.TrimEnd('|');
		}
		// no deletion here; final snap will clean up
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
				map[p] = (start, 0);
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