using UnityEngine;

public abstract class AIActionData : ScriptableObject
{
    [Header("Base Data")]
    [SerializeField] private string actionName;
    [SerializeField] private Sprite actionIcon;
    [TextArea][SerializeField] private string actionDescription;
    [SerializeField] private int roundsUntilAction;
    [SerializeField] private Color barColor = Color.black;

    public string ActionName => actionName;
    public Sprite ActionIcon => actionIcon;
    public string ActionDescription => actionDescription;
    public int RoundsUntilAction => roundsUntilAction;
    public Color BarColor => barColor;

    public virtual void PerformAction(AI ai, CombatManager cm)
    {
        Debug.Log("AI not given action");
    }
}
