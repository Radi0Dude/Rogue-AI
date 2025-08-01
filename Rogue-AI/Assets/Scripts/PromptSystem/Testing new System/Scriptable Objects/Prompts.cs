using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Prompts", menuName = "Scriptable Objects/Prompts")]
public class Prompts : ScriptableObject
{
    public string promptID;
    public string promptText;
    public List<string> options;

    [TextArea] public string template;
    [SerializeField] bool useCustomTemplate = false;

	public PromptTemplate promptTemplate;



}
