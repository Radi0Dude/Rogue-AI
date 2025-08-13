using System;
using UnityEngine;

public class PromptButtonTag : MonoBehaviour
{
    public string[] promptTags = { "Proffesor", "AI", "Game Developer", "Learning", "Programmer" };

	Tags tags;

	private void Awake()
	{
		promptTags = System.Enum.GetNames(typeof(Tags));
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