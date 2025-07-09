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
    [SerializeField] private string aiName = "UnNamed";
    [SerializeField] private float maxSanity = 100.0f;
    [SerializeField] private float startSanity = 10.0f;
    [SerializeField] private AITypes aiType;
    [SerializeField] private List<AIActionData> aiActions = new();
    
    [Header("Option for stronger Enemies")]
    [SerializeField] private AIActionData endOfTurnAction;

    public string AIName => aiName;
    public float MaxSanity => maxSanity;
    public float StartSanity => startSanity;
    public AITypes AIType => aiType;
    public List<AIActionData> AIActions => aiActions;
    public AIActionData EndOfTurnAction => endOfTurnAction;

}
