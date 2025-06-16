using UnityEngine;
using System.Collections.Generic;
using System;

public class ConnectNodes : MonoBehaviour
{
	public List<GameObject> connectorNodes = new List<GameObject>();
	public List<LineRenderer> connectorLines = new List<LineRenderer>();

	public List<GameObject> connectedNodes = new List<GameObject>();
	public List<LineRenderer> connectedLines = new List<LineRenderer>();

	public bool isPrompt;
	public string promptText;

	private void Update()
	{
		UpdateLines();
	}

	private void UpdateLines()
	{
		foreach(LineRenderer line in connectorLines)
		{
			line.SetPosition(0, transform.position);
		}
		foreach(LineRenderer line in connectedLines)
		{
			line.SetPosition(1, transform.position);
		}
	}

	public void UpdatePromptText(string text)
	{
		if (isPrompt)
		{
			promptText = text;
		}
		else
		{
			promptText = "This is a connector node. It does not have a prompt.";
			
		}
	}
}
