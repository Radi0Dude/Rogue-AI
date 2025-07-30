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
			Debug.LogError("No starting node found. Please ensure there is a node with no incoming connections.");
			return;
		}

		var nodeList = new List<PromptNodeData>();
		var visited = new HashSet<ConnectNodes>();
		var nodeIdMap = new Dictionary<ConnectNodes, string>();
		int idCounter = 0;

		Traverse(findFirstNode, nodeList, visited, nodeIdMap, ref idCounter);

		string json = JsonUtility.ToJson(new PromptNodeDataList { nodes = nodeList }, true);

#if UNITY_EDITOR
		string path = EditorUtility.SaveFilePanel("Save Story JSON", "", "story.json", "json");
		if (!string.IsNullOrEmpty(path))
			File.WriteAllText(path, json);
#endif

		Debug.Log("JSON saved:\n" + json);
	}

	private void FindFirstNode()
	{
		var nodes = FindObjectsByType<ConnectNodes>(FindObjectsSortMode.None);
		foreach (var node in nodes)
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
		HashSet<ConnectNodes> visited,
		Dictionary<ConnectNodes, string> nodeIdMap,
		ref int idCounter
	)
	{
		if (visited.Contains(node)) return;
		visited.Add(node);

		if (!nodeIdMap.ContainsKey(node))
		{
			nodeIdMap[node] = idCounter.ToString();
			idCounter++;
		}

		string nodeId = nodeIdMap[node];

		var promptData = new PromptNodeData
		{
			nodeId = nodeId,
			text = node.promptText,
			options = new List<PromptOptionData>()
		};

		for (int i = 0; i < node.connectedTo.Count; i++)
		{
			var cardObj = node.connectedTo[i];
			if (cardObj == null) continue;

			var cardNode = cardObj.GetComponent<ConnectNodes>();
			if (cardNode == null || cardNode.connectedTo.Count == 0) continue;

			var nextPrompt = cardNode.connectedTo[0]?.GetComponent<ConnectNodes>();
			if (nextPrompt == null) continue;

			if (!nodeIdMap.ContainsKey(nextPrompt))
			{
				nodeIdMap[nextPrompt] = idCounter.ToString();
				idCounter++;
			}

			promptData.options.Add(new PromptOptionData
			{
				cardName = cardNode.nodeName,
				nextNodeId = nodeIdMap[nextPrompt]
			});

			Traverse(nextPrompt, nodeList, visited, nodeIdMap, ref idCounter);
		}

		nodeList.Add(promptData);
	}

	[System.Serializable]
	public class PromptOptionData
	{
		public string cardName;
		public string nextNodeId;
	}

	[System.Serializable]
	public class PromptNodeData
	{
		public string nodeId;
		public string text;
		public List<PromptOptionData> options;
	}

	[System.Serializable]
	public class PromptNodeDataList
	{
		public List<PromptNodeData> nodes;
	}
}