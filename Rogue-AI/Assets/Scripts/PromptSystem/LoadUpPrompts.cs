using System.Collections;
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

		string file = files[0]; // always use the first file for now
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

		currentPrompt = currentNode.text;
		currentOptionNames = new string[currentNode.options.Count];
		for (int i = 0; i < currentNode.options.Count; i++)
		{
			currentOptionNames[i] = currentNode.options[i].optionText;
		}

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

		string baseName = cardData.cardName;

		var nextOptions = currentNode.options
			.Where(o => o.nextNodeId.StartsWith(baseName + "_"))
			.Select(o => new
			{
				option = o,
				number = ExtractNumberSuffix(o.nextNodeId)
			})
			.OrderBy(x => x.number)
			.ToList();

		if (nextOptions.Count > 0)
		{
			var nextNodeId = nextOptions.First().option.nextNodeId;
			if (nodeDict.TryGetValue(nextNodeId, out PromptNodeData nextNode))
			{
				currentNode = nextNode;
				currentPrompt = nextNode.text;
				currentOptionNames = new string[nextNode.options.Count];
				for (int i = 0; i < nextNode.options.Count; i++)
				{
					currentOptionNames[i] = nextNode.options[i].optionText;
				}

				Debug.Log($"Moved to: {nextNodeId} via card: {cardData.cardName}");
				return;
			}
		}

		Debug.LogWarning($"No valid node found for card '{cardData.cardName}' from '{currentNode.nodeId}'.");
	}

	private int ExtractNumberSuffix(string id)
	{
		int underscoreIndex = id.LastIndexOf('_');
		if (underscoreIndex >= 0 && int.TryParse(id.Substring(underscoreIndex + 1), out int num))
		{
			return num;
		}
		return -1;
	}
}