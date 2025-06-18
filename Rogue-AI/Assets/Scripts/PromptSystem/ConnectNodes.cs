using UnityEngine;
using System.Collections.Generic;
using System;

public class ConnectNodes : MonoBehaviour
{
	[Header("Connections")]
	public List<GameObject> connectedTo = new List<GameObject>();
	public List<GameObject> connectedFrom = new List<GameObject>();

	[Header("Visual Lines")]
	public List<LineRenderer> connectorLines = new List<LineRenderer>();
	public List<LineRenderer> connectedLines = new List<LineRenderer>();

	[Header("Node Properties")]
	public bool isPrompt = true;
	[TextArea] public string promptText;
	public string nodeName;

	[Header("Option Labels (match connections)")]
	public List<string> optionTexts = new List<string>();

	[SerializeField] CardData thisCardData;

	private void OnValidate()
	{
		while (optionTexts.Count < connectedTo.Count)
			optionTexts.Add("Option");

		while (optionTexts.Count > connectedTo.Count)
			optionTexts.RemoveAt(optionTexts.Count - 1);
	}

	private void Update()
	{
		UpdateLines();
	}

	public void AssignCardData(CardData cardData)
	{
		thisCardData = cardData;
	}

	private void UpdateLines()
	{
		foreach (LineRenderer line in connectorLines)
			if (line) line.SetPosition(0, transform.position);

		foreach (LineRenderer line in connectedLines)
			if (line) line.SetPosition(1, transform.position);
	}

	public void UpdatePromptText(string text)
	{
		promptText = isPrompt ? text : "This is a Card Node";
	}
}
