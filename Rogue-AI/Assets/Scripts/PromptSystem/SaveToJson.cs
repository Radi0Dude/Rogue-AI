using System.Collections.Generic;
using UnityEngine;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SaveToJson : MonoBehaviour
{
	private ConnectNodes findFirstNode;

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
		var nodeIdMap = new Dictionary<ConnectNodes, string>();

		int textNodeCounter = 0;

		Traverse(findFirstNode, nodeList, nodeIdMap, visitedNodes, ref textNodeCounter);

		string json = JsonUtility.ToJson(new PromptNodeDataList { nodes = nodeList }, true);
		Debug.Log(json);

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

	private void Traverse(
		ConnectNodes node,
		List<PromptNodeData> nodeList,
		Dictionary<ConnectNodes, string> nodeIdMap,
		HashSet<ConnectNodes> visited,
		ref int textNodeCounter
	)
	{
		if (visited.Contains(node))
			return;

		visited.Add(node);

		// Assign a clean sequential ID
		if (!nodeIdMap.ContainsKey(node))
		{
			nodeIdMap[node] = textNodeCounter.ToString();
			textNodeCounter++;
		}

		string currentNodeId = nodeIdMap[node];

		var nodeData = new PromptNodeData
		{
			nodeId = currentNodeId,
			cardName = node.nodeName,
			text = node.promptText,
			options = new List<PromptOptionData>()
		};

		for (int i = 0; i < node.connectedTo.Count; i++)
		{
			var cardObj = node.connectedTo[i];
			var cardNode = cardObj != null ? cardObj.GetComponent<ConnectNodes>() : null;
			if (cardNode == null) continue;

			string optionText = (node.optionTexts != null && i < node.optionTexts.Count)
				? node.optionTexts[i]
				: "Option";

			ConnectNodes nextPromptNode = null;
			if (cardNode.connectedTo.Count > 0)
			{
				nextPromptNode = cardNode.connectedTo[0]?.GetComponent<ConnectNodes>();
			}

			if (nextPromptNode != null)
			{
				if (!nodeIdMap.ContainsKey(nextPromptNode))
				{
					nodeIdMap[nextPromptNode] = textNodeCounter.ToString();
					textNodeCounter++;
				}

				nodeData.options.Add(new PromptOptionData
				{
					optionText = optionText,
					nextNodeId = nodeIdMap[nextPromptNode]
				});

				Traverse(nextPromptNode, nodeList, nodeIdMap, visited, ref textNodeCounter);
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