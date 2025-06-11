using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
public class PromptWizard : EditorWindow
{

	//string nodeId = "Node_1";
	//string nodeText = "This is the start of the story.";
	//int choiceCount = 2;
	//List<string> choiceTexts = new List<string>();
	//List<StoryNode> choiceLinks = new List<StoryNode>();

	//[MenuItem("Tools/Story Wizard")]
	//public static void ShowWindow()
	//{
	//	GetWindow<PromptWizard>("Story Wizard");
	//}

	//void OnGUI()
	//{
	//	GUILayout.Label("Create New Story Node", EditorStyles.boldLabel);
	//	nodeId = EditorGUILayout.TextField("Node ID", nodeId);
	//	nodeText = EditorGUILayout.TextArea(nodeText, GUILayout.Height(60));

	//	choiceCount = EditorGUILayout.IntField("Choices", choiceCount);
	//	while (choiceTexts.Count < choiceCount)
	//	{
	//		choiceTexts.Add("Choice " + (choiceTexts.Count + 1));
	//		choiceLinks.Add(null);
	//	}

	//	for (int i = 0; i < choiceCount; i++)
	//	{
	//		GUILayout.BeginHorizontal();
	//		choiceTexts[i] = EditorGUILayout.TextField("Text", choiceTexts[i]);
	//		choiceLinks[i] = (StoryNode)EditorGUILayout.ObjectField("Next Node", choiceLinks[i], typeof(StoryNode), false);
	//		GUILayout.EndHorizontal();
	//	}

	//	GUILayout.Space(10);
	//	if (GUILayout.Button("Create Story Node"))
	//	{
	//		CreateNode();
	//	}
	//}

	//void CreateNode()
	//{
	//	StoryNode newNode = ScriptableObject.CreateInstance<StoryNode>();
	//	newNode.nodeId = nodeId;
	//	newNode.text = nodeText;
	//	newNode.choices = new List<Choice>();

	//	for (int i = 0; i < choiceCount; i++)
	//	{
	//		newNode.choices.Add(new Choice
	//		{
	//			choiceText = choiceTexts[i],
	//			nextNode = choiceLinks[i]
	//		});
	//	}

	//	string path = "Assets/StoryNodes/" + nodeId + ".asset";
	//	AssetDatabase.CreateAsset(newNode, path);
	//	AssetDatabase.SaveAssets();
	//	AssetDatabase.Refresh();

	//	Debug.Log($"Created story node: {nodeId}");
	//}
}
