using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StoryNode", menuName = "Scriptable Objects/StoryNode")]
public class StoryNode : ScriptableObject
{
	public string nodeId;
	[TextArea] public string text;
	public List<Choice> choices = new List<Choice>();
}
[System.Serializable]
public class Choice
{
	public string choiceText;
	public StoryNode nextNode; // Link to another node
}
