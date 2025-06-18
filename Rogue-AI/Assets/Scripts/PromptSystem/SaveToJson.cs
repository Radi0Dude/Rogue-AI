using System.Collections.Generic;
using System;
using UnityEngine;
using static SaveToJson;

public class SaveToJson : MonoBehaviour
{
    ConnectNodes findFirstNode;

    


    public void ToJson()
    {
        FindFirstNode();
		
		if(findFirstNode == null)
		{
			Debug.LogError("No starting node found. Please ensure there is a node with no connections.");
			return;
		}
		var nodeList = new List<PromptNodeData>();
		var visitedNodes = new HashSet<ConnectNodes>();

		Traverse(findFirstNode, nodeList, visitedNodes);

		string json = JsonUtility.ToJson(new PromptNodeDataList { nodes = nodeList }, true);

	}

	private void FindFirstNode()
	{
		ConnectNodes[] nodes = FindObjectsByType<ConnectNodes>(FindObjectsSortMode.None);
		foreach (ConnectNodes node in nodes)
		{
			if (node.connectedFrom.Count == 0)
			{
				findFirstNode = node;
				break;
			}
		}
	}

	void Traverse(ConnectNodes node, List<PromptNodeData> nodeList, HashSet<ConnectNodes> visited)
	{
		if(visited.Contains(node))
		{
			return;
		}
		visited.Add(node);
		var nodeData = new PromptNodeData
		{
			nodeId = node.gameObject.name,
			text = node.promptText,
			options = new List<PromptOptionData>()
		};

		foreach (GameObject obj in node.connectedTo)
		{
			var targetNode = obj.GetComponent<ConnectNodes>();
			if (targetNode != null)
			{
				// Optionally grab option text from your node structure
				string optionText = "Option"; // Replace with actual option text if you have it
				nodeData.options.Add(new PromptOptionData
				{
					optionText = optionText,
					nextNodeId = targetNode.cardName
				});

				Traverse(targetNode, nodeList, visited);
			}
		}
		nodeList.Add(nodeData);
	}

	[System.Serializable]
	public class PromptOptionData
	{
		public string optionText;
		public string nextNodeId;
	}
	[System.Serializable]
	public class PromptNodeData
	{
		public string nodeId;
		public string text;
		public List<PromptOptionData> options = new List<PromptOptionData>();

	}
	[System.Serializable]
	public class PromptNodeDataList
	{
		public List<PromptNodeData> nodes;
	}
}
