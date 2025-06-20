using System.Collections.Generic;
using UnityEngine;
using System.IO;
using static SaveToJson;
using System.Collections;
using System.Linq;

public class LoadUpPrompts : MonoBehaviour
{
    [SerializeField, Path]
	List<string> paths = new List<string>();

	int typeOfAi = 0;

	InputField inputField;
	[SerializeField]
	string jsonContent;
	[SerializeField]
	string currentPrompt;
	[SerializeField]
	string[] currentOptionNames;
	CardData currentCardPlayed;

	Dictionary<string, PromptNodeData> nodeDict = new Dictionary<string, PromptNodeData>();

	private void Awake()
	{
		GetAllPossiblePrompts();
		//inputField = FindFirstObjectByType<InputField>();
	}

	private void Start()
	{
		//inputField.OnPlayCard += CardPlayed;
		//Type of Ai Should be set by the developer before fighting and we should get it here
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

		if (files.Length == 0)
		{
			Debug.LogWarning("No JSON files found in: " + selectedPath);
			return;
		}

		int randomStartingPrompt = Random.Range(0, files.Length);
		string file = files[randomStartingPrompt];
		jsonContent = File.ReadAllText(file);
		StartCoroutine(waitForJson());
		BuildDict(JsonUtility.FromJson<PromptNodeDataList>(jsonContent));
	}

	public void BuildDict(PromptNodeDataList nodeDataList)
	{
		foreach (var n in nodeDataList.nodes)
		{
			nodeDict[n.nodeId] = n;
			Debug.Log($"Node added: {n.nodeId} with text: {n.text}");
		}
	}

	IEnumerator waitForJson()
	{
		while (string.IsNullOrEmpty(jsonContent))
		{
			yield return new WaitForEndOfFrame();
			
		}
		LoadUpStartPrompt();
	}
	
	public void LoadUpStartPrompt()
    {
		PromptNodeDataList nodeDataList = JsonUtility.FromJson<PromptNodeDataList>(jsonContent);
		if (nodeDataList.nodes != null && nodeDataList.nodes.Count > 0)
		{
			PromptNodeData node = nodeDataList.nodes[nodeDataList.nodes.Count - 1];
			currentPrompt = node.text;
			currentOptionNames = new string[node.options.Count];
			for (int i = 0; i < node.options.Count; i++)
			{
				currentOptionNames[i] = node.options[i].nextNodeId;
			}
			Debug.Log(currentPrompt);
		}
	}

	public void GetCurrentCard(CardData cardData)
	{
		currentCardPlayed = cardData;
		if(nodeDict != null && nodeDict.TryGetValue(cardData.cardName, out PromptNodeData nodeData))
		{
			currentPrompt = nodeData.text;
			currentOptionNames = new string[nodeData.options.Count];
			for (int i = 0; i < nodeData.options.Count; i++)
			{
				currentOptionNames[i] = nodeData.options[i].nextNodeId;
			}
			Debug.Log($"Current Card: {cardData.cardName}, Prompt: {currentPrompt}");
		}
		else
		{
			Debug.LogError($"Card data for {cardData.cardName} not found in node dictionary.");
		}
	}
	void UpdatePrompt()
	{

	}
	private void CardPlayed(PromptType promptType)
	{
		
	}

}
