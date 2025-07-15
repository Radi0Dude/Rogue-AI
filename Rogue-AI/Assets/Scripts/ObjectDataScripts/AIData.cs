using System;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum AITypes
{
    Grammar = 1 << 0,
    Research = 1 << 1,
    Summarization = 1 << 2,
    Programming = 1 << 3,
}

[CreateAssetMenu(fileName = "AI", menuName = "AI/AI Data")]
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
