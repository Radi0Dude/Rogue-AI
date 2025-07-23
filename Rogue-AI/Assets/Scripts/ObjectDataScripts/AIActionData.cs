using UnityEngine;

public abstract class AIActionData : ScriptableObject
{
    [Header("Base Data")]
    [SerializeField] private string actionName;
    [SerializeField] private Sprite actionIcon;
    [SerializeField] private int roundsUntilAction;
    [SerializeField] private Color barColor = Color.black;

    public string ActionName => actionName;
    public Sprite ActionIcon => actionIcon;
    public int RoundsUntilAction => roundsUntilAction;
    public Color BarColor => barColor;

    public virtual void PerformAction(AI ai, Player player)
    {
        Debug.Log("AI not given action");
    }
}
