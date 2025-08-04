using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Prompts", menuName = "Scriptable Objects/Prompts")]
public class Prompts : ScriptableObject
{
	[Tooltip("All valid card types / categories.")]
	public List<string> cardTypes = new List<string>()
	{
		"Define Limits",
		"Give Background",
		"Structure Answer",
		"Clarify Outcome",
		"Iterate",
		"Assign a Role",
		"Give an Example",
		"Break it Down",
		"Constraints with Context",
		"Laser-Focused Briefing",
		"Goal-Oriented Iteration",
		"Expert Response Template",
		"Persona with Purpose",
		"Step-by-Step Format"
	};
}
