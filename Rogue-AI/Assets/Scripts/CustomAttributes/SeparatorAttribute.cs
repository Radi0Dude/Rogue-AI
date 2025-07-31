using System.Security.Permissions;
using UnityEngine;

[System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true)]
public class SeparatorAttribute : PropertyAttribute
{
    public readonly float Height;
    public readonly float Spacing;
    public readonly Color Color;

    public SeparatorAttribute(float height = 1, float spacing = 10)
    {
        Height = height;
        Spacing = spacing;
    }
}
