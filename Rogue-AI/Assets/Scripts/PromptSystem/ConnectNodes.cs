using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.PlayerLoop;
using TMPro;
using UnityEngine.UI;

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

	[SerializeField]
	GameObject promptUI;
	[SerializeField]
	TMP_Text promptTextObject;
	[SerializeField]
	GameObject cardUI;
	[SerializeField]
	TMP_Text cardTextObject;

	[SerializeField]
	Image cardImage;

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
		UpdateUi();
		UpdateNodeText();
	}

	void UpdateNodeText()
	{
		promptTextObject.text = isPrompt ? promptText : "";
		cardTextObject.text = isPrompt ? "" : thisCardData != null ? thisCardData.cardName : "No Card Data Assigned";
		if(thisCardData != null && thisCardData.cardImage != null)
		cardImage.sprite = thisCardData != null ? thisCardData.cardImage : null;
	}

	void UpdateUi()
	{
		if (isPrompt)
		{
			promptUI.SetActive(true);
			cardUI.SetActive(false);
		}
		else
		{
			promptUI.SetActive(false);
			cardUI.SetActive(true);
		}
	}

	public void AssignCardData(CardData cardData)
	{
		thisCardData = cardData;
		nodeName = cardData.cardName;
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
