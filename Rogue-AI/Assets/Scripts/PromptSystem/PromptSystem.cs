using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class PromptSystem : MonoBehaviour
{
	[SerializeField] List<CardData> cards = new List<CardData>();

	VisualPromptSystem visualPromptSystem;

	private void Awake()
	{
		#region Load Card Data Start
#if UNITY_EDITOR
		LoadAllCardDataEditor();
#else
		LoadAllCardData();
#endif
		#endregion
		visualPromptSystem = FindFirstObjectByType<VisualPromptSystem>();
	}

	




	#region Load Card Data
	private void LoadAllCardData()
	{
		cards = Resources.LoadAll<CardData>("Cards").ToList();
	}

#if UNITY_EDITOR
	private void LoadAllCardDataEditor()
	{
		cards.Clear();
		string[] guids = AssetDatabase.FindAssets("t:CardData");
		foreach (string guid in guids)
		{
			string path = AssetDatabase.GUIDToAssetPath(guid);
			CardData cardData = AssetDatabase.LoadAssetAtPath<CardData>(path);
			if (cardData != null)
			{
				cards.Add(cardData);
			}
		}
	}
#endif
	#endregion
	
}
