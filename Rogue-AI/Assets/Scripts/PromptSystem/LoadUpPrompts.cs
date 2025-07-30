using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using static SaveToJson;

public class LoadUpPrompts : MonoBehaviour
{
	[SerializeField, Path]
	List<string> paths = new List<string>();

	int typeOfAi = 0;

	[SerializeField] string jsonContent;
	[SerializeField] public string currentPrompt;
	[SerializeField] string[] currentOptionNames;
	CardData currentCardPlayed;

	Dictionary<string, PromptNodeData> nodeDict = new Dictionary<string, PromptNodeData>();
	PromptNodeData currentNode;

	private void Awake()
	{
		GetAllPossiblePrompts();
	}

	void GetAllPossiblePrompts()
	{
		if (typeOfAi < 0 || typeOfAi >= paths.Count)
		{
			Debug.LogError("typeOfAi index out of range.");
			return;
		}

		string selectedPath = paths[typeOfAi];

		if (string.IsNullOrEmpty(selectedPath) || !Directory.Exists(selectedPath))
		{
			Debug.LogError("Path is empty or invalid at index: " + typeOfAi);
			return;
		}

		string[] files = Directory.GetFiles(selectedPath, "*.json", SearchOption.TopDirectoryOnly);
		Debug.Log($"Found {files.Length} JSON files in: {selectedPath}");

		if (files.Length == 0)
		{
			Debug.LogWarning("No JSON files found in: " + selectedPath);
			return;
		}

		string file = files[0]; // Load first file
		Debug.Log($"Loading JSON file: {file}");
		jsonContent = File.ReadAllText(file);

		PromptNodeDataList nodeDataList = JsonUtility.FromJson<PromptNodeDataList>(jsonContent);
		BuildDict(nodeDataList);
		LoadUpStartPrompt(nodeDataList);
	}

	public void BuildDict(PromptNodeDataList nodeDataList)
	{
		nodeDict.Clear();
		foreach (var n in nodeDataList.nodes)
		{
			nodeDict[n.nodeId] = n;
			Debug.Log($"Node added: {n.nodeId} with text: {n.text}");
		}
	}

	void LoadUpStartPrompt(PromptNodeDataList nodeDataList)
	{
		var allNodeIds = new HashSet<string>(nodeDataList.nodes.Select(n => n.nodeId));
		var referencedIds = new HashSet<string>(
			nodeDataList.nodes.SelectMany(n => n.options).Select(o => o.nextNodeId)
		);

		var entryCandidates = allNodeIds.Except(referencedIds).ToList();

		if (entryCandidates.Count > 0 && nodeDict.TryGetValue(entryCandidates[0], out PromptNodeData entryNode))
		{
			currentNode = entryNode;
		}
		else
		{
			currentNode = nodeDataList.nodes[0]; // fallback
		}

		SetPromptFromNode(currentNode);
		Debug.Log($"Start node loaded: {currentNode.nodeId}");
	}

	public void GetCurrentCard(CardData cardData)
	{
		currentCardPlayed = cardData;

		if (currentNode == null)
		{
			Debug.LogWarning("Current node is not set.");
			return;
		}

		// Look for a matching cardName in current options
		var matchingOption = currentNode.options.FirstOrDefault(opt => opt.cardName == cardData.cardName);

		if (matchingOption != null && nodeDict.TryGetValue(matchingOption.nextNodeId, out var nextNode))
		{
			currentNode = nextNode;
			SetPromptFromNode(nextNode);
			Debug.Log($"Moved to node '{nextNode.nodeId}' using card '{cardData.cardName}'");
		}
		else
		{
			Debug.LogWarning($"No valid option found for card '{cardData.cardName}' in node '{currentNode.nodeId}'");
		}
	}

	private void SetPromptFromNode(PromptNodeData node)
	{
		currentPrompt = node.text;
		currentOptionNames = node.options.Select(opt => opt.cardName).ToArray();
	}
}