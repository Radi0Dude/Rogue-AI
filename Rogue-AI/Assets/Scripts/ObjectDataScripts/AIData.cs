using System;
using System.Collections.Generic;
using UnityEngine;




[CreateAssetMenu(fileName = "AI", menuName = "AI/AI Data")]
public class AIData : ScriptableObject
{
    [SerializeField] private string aiName = "UnNamed";
    [SerializeField] private float maxSanity = 100.0f;
    [SerializeField] private float startSanity = 10.0f;
    [SerializeField] private AIModelType aiModelType;
    [SerializeField] private List<AIActionData> aiActions = new();
    
    [Header("Option for stronger Enemies")]
    [SerializeField] private AIActionData endOfTurnAction;


    public string AIName => aiName;
    public float MaxSanity => maxSanity;
    public float StartSanity => startSanity;
    public AIModelType AIModelType => aiModelType;
    public List<AIActionData> AIActions => aiActions;
    public AIActionData EndOfTurnAction => endOfTurnAction;

}
