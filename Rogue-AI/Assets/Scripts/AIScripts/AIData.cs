using System.Collections.Generic;
using UnityEngine;

public enum AITypes
{
    Grammar,
    Research,
    Summarization,
    Programming,
}

[CreateAssetMenu(fileName = "New AI Data", menuName = "AI/AI Data")]
public class AIData : ScriptableObject
{
    public float maxSanity = 100.0f;
    public float startSanity = 10.0f;
    
    public AITypes aiType;
    
    public List<AIActionData> aiActions;

    [Header("Option for stronger Enemies")]
    public AIActionData endOfTurnAction;
}
