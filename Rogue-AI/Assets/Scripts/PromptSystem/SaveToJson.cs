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
		int globalIdCounter = 0;

		Traverse(findFirstNode, nodeList, nodeIdMap, visitedNodes, ref globalIdCounter);

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
		ref int globalIdCounter
	)
	{
		if (visited.Contains(node))
			return;

		visited.Add(node);

		// Assign a globally unique node ID
		if (!nodeIdMap.ContainsKey(node))
			nodeIdMap[node] = node.nodeName + "_" + globalIdCounter++;

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
			var targetObj = node.connectedTo[i];
			var targetNode = targetObj != null ? targetObj.GetComponent<ConnectNodes>() : null;

			if (targetNode != null)
			{
				// Assign ID for target node if it hasn't been mapped yet
				if (!nodeIdMap.ContainsKey(targetNode))
					nodeIdMap[targetNode] = targetNode.nodeName + "_" + globalIdCounter++;

				string optionText = (node.optionTexts != null && i < node.optionTexts.Count)
					? node.optionTexts[i]
					: "Option";

				nodeData.options.Add(new PromptOptionData
				{
					optionText = optionText,
					nextNodeId = nodeIdMap[targetNode]
				});

				Traverse(targetNode, nodeList, nodeIdMap, visited, ref globalIdCounter);
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