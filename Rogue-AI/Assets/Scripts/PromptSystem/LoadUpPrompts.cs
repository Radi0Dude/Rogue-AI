using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using static SaveToJson;

public class LoadUpPrompts : MonoBehaviour
{
	[SerializeField, Path]
	private List<string> paths = new List<string>();

	[SerializeField]
	private int typeOfAi = 0;

	[SerializeField]
	private string jsonContent;

	[SerializeField]
	public string currentPrompt;

	[SerializeField]
	public string[] currentOptionNames;

	private PromptNodeData currentNode;
	private PromptNodeDataList loadedNodeDataList;
	private CardData currentCardPlayed;

	private Dictionary<string, PromptNodeData> nodeDict = new Dictionary<string, PromptNodeData>();

	// The list of card types (1 per node per group/layer)
	public List<string> cardTypes = new List<string> {
		"Basic Delete", "Basic Draw", "Basic Format",
		"Basic Iterate", "Basic Persona", "Basic Specify",
		"Basic Virus", "Specify and Expand", "DELETE"
	};

	private void Awake()
	{
		LoadFirstPromptFile();
	}

	void LoadFirstPromptFile()
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

		if (files.Length == 0)
		{
			Debug.LogWarning("No JSON files found in: " + selectedPath);
			return;
		}

		string file = files[0]; // Load the first file
		Debug.Log($"Loading JSON file: {file}");

		jsonContent = File.ReadAllText(file);
		loadedNodeDataList = JsonUtility.FromJson<PromptNodeDataList>(jsonContent);

		if (loadedNodeDataList == null || loadedNodeDataList.nodes.Count == 0)
		{
			Debug.LogError("Failed to parse or empty JSON.");
			return;
		}

		BuildDict(loadedNodeDataList);

		// Use nodeId "0" if it exists, otherwise use the last
		currentNode = loadedNodeDataList.nodes.FirstOrDefault(n => n.nodeId == "0")
					  ?? loadedNodeDataList.nodes.Last();

		UpdateCurrentPrompt(currentNode);
	}

	public void BuildDict(PromptNodeDataList nodeDataList)
	{
		nodeDict.Clear();

		foreach (var n in nodeDataList.nodes)
		{
			nodeDict[n.nodeId] = n;
		}

		Debug.Log($"Built node dictionary with {nodeDict.Count} entries.");
	}

	void UpdateCurrentPrompt(PromptNodeData node)
	{
		currentPrompt = node.text;
		currentOptionNames = new string[node.options.Count];

		for (int i = 0; i < node.options.Count; i++)
		{
			currentOptionNames[i] = node.options[i].optionText;
		}

		Debug.Log($"Prompt updated: {currentPrompt} ({node.nodeId})");
	}

	public void GetCurrentCard(CardData cardData)
	{
		currentCardPlayed = cardData;

		if (currentNode == null || currentNode.options == null)
		{
			Debug.LogWarning("No current node to progress from.");
			return;
		}

		string baseName = cardData.cardName;
		int groupSize = cardTypes.Count;

		int currentNumber = ExtractNumberSuffix(currentNode.nodeId);
		int nextLayerStart = ((currentNumber / groupSize) + 1) * groupSize;

		var candidates = currentNode.options
			.Select(opt => opt.nextNodeId)
			.Where(id => id.StartsWith(baseName + "_"))
			.Select(id => new
			{
				nodeId = id,
				number = ExtractNumberSuffix(id)
			})
			.Where(x => x.number >= nextLayerStart)
			.OrderBy(x => x.number)
			.ToList();

		if (candidates.Count > 0)
		{
			string nextId = candidates.First().nodeId;
			if (nodeDict.TryGetValue(nextId, out PromptNodeData nextNode))
			{
				currentNode = nextNode;
				UpdateCurrentPrompt(currentNode);
				Debug.Log($"Progressed to next layer node: {nextId} via card: {baseName}");
				return;
			}
		}

		Debug.LogWarning($"No valid next-layer node found for '{baseName}' after '{currentNode.nodeId}'");
	}

	private int ExtractNumberSuffix(string nodeId)
	{
		int underscoreIndex = nodeId.LastIndexOf('_');
		if (underscoreIndex >= 0 && int.TryParse(nodeId.Substring(underscoreIndex + 1), out int number))
		{
			return number;
		}
		return -1;
	}
}