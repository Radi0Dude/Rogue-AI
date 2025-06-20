using System.Collections.Generic;
using UnityEngine;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SaveToJson : MonoBehaviour
{
	ConnectNodes findFirstNode;

	public void ToJson()
	{
		FindFirstNode();

		if (findFirstNode == null)
		{
			Debug.LogError("No starting node found. Please ensure there is a node with no connections.");
			return;
		}

		var nodeList = new List<PromptNodeData>();
		var visitedNodes = new HashSet<ConnectNodes>();

		Traverse(findFirstNode, nodeList, visitedNodes, 0);

		string json = JsonUtility.ToJson(new PromptNodeDataList { nodes = nodeList }, true);

		Debug.Log(json);

		// Optionally, save the JSON to a file in the Editor
#if UNITY_EDITOR
		string path = EditorUtility.SaveFilePanel("Save Story JSON", "", "story.json", "json");
		if (!string.IsNullOrEmpty(path))
			File.WriteAllText(path, json);
#endif
	}

	private void FindFirstNode()
	{
		ConnectNodes[] nodes = FindObjectsByType<ConnectNodes>(FindObjectsSortMode.None);
		foreach (ConnectNodes node in nodes)
		{
			if (node.connectedFrom == null || node.connectedFrom.Count == 0)
			{
				findFirstNode = node;
				break;
			}
		}
	}

	void Traverse(ConnectNodes node, List<PromptNodeData> nodeList, HashSet<ConnectNodes> visited, int id)
	{
		if (visited.Contains(node))
		{
			return;
		}
		visited.Add(node);

		var nodeData = new PromptNodeData
		{
			nodeId = node.nodeName + id.ToString(),
			cardName = node.nodeName,
			text = node.promptText,
			options = new List<PromptOptionData>()
		};

		// Loop through each option and its connected node
		for (int i = 0; i < node.connectedTo.Count; i++)
		{
			var targetObj = node.connectedTo[i];
			var targetNode = targetObj != null ? targetObj.GetComponent<ConnectNodes>() : null;
			if (targetNode != null)
			{
				string optionText = (node.optionTexts != null && i < node.optionTexts.Count)
					? node.optionTexts[i]
					: "Option";

				nodeData.options.Add(new PromptOptionData
				{
					optionText = optionText,
					nextNodeId = targetNode.nodeName,
					
				});

				Traverse(targetNode, nodeList, visited, i +1);
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
		public string cardName;
		public string text;
		public List<PromptOptionData> options = new List<PromptOptionData>();
	}

	[System.Serializable]
	public class PromptNodeDataList
	{
		public List<PromptNodeData> nodes;
	}
}