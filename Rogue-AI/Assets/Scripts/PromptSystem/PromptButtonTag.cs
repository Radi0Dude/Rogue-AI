using System;
using UnityEngine;

public class PromptButtonTag : MonoBehaviour
{
    public string[] promptTags = { "Proffesor", "AI", "Game Developer", "Learning", "Programmer" };

	Tags tags;

	private void Awake()
	{
		for(int i = 0; i < System.Enum.GetValues(typeof(Tags)).Length; i++)
		{
			promptTags[i] = Enum.GetName(typeof(Tags), i);
		}
	}
}

public enum Tags
{
	Proffesor,
	AI,
	GameDeveloper,
	Learning,
	Programmer,
}

[Serializable]
public class PormptAndTag
{
	public MultiplePrompts[] prompt;
	public Tags[] tags;
}
[Serializable]
public class MultiplePrompts
{
	public string[] prompts;
}