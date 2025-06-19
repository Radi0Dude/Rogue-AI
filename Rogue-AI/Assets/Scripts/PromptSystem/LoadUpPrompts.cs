using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LoadUpPrompts : MonoBehaviour
{
    [SerializeField, Path]
	List<string> paths = new List<string>();

	int typeOfAi = 0;

	InputField inputField;
	string jsonContent;

	private void Awake()
	{
		GetAllPossiblePrompts();
		inputField = FindFirstObjectByType<InputField>();
	}

	private void Start()
	{
		inputField.OnPlayCard += CardPlayed;
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
	}

	public void LoadUpStartPrompt()
    {

    }

	private void CardPlayed(PromptType promptType)
	{

	}

}
