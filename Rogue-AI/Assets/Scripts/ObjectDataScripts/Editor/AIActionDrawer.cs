using UnityEngine;
using UnityEditor;
/* // This Scripts works for a class, but not a scriptable object
[CustomPropertyDrawer(typeof(AIActionData))]
public class AIActionDrawer : PropertyDrawer
{
    private SerializedProperty _actionName;
    private SerializedProperty _roundsUntilAction;

    // How to draw to the Inspector window
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        
        // Find our properties
        _actionName = property.FindPropertyRelative("actionName");
        _roundsUntilAction = property.FindPropertyRelative("roundsUntilAction");
        
        // Drawing instructions
        Rect foldOutBox = new Rect(position.min.x, position.min.y,
            position.size.x, EditorGUIUtility.singleLineHeight);
        property.isExpanded = EditorGUI.Foldout(foldOutBox, property.isExpanded, label);
        
        if (property.isExpanded)
        {
            // Draw our properties
            DrawActionNameProperty(position);
            //DrawRoundsUntilActionProperty(position);
        }
        
        
        EditorGUI.EndProperty();
    }
    
    private void DrawActionNameProperty(Rect position)
    {
        float xPos = position.min.x;
        float yPos = position.min.y + EditorGUIUtility.singleLineHeight;
        float width = position.size.x;
        float height = EditorGUIUtility.singleLineHeight;
        
        Rect drawArea = new Rect(xPos, yPos, width, height);
        EditorGUI.PropertyField(drawArea, _actionName, new GUIContent("Action"));
    }
    
    private void DrawRoundsUntilActionProperty(Rect position)
    {
        throw new System.NotImplementedException();
    }

 

    // request more vertical spacing, return it
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        int totalLines = 1;
        
        // increase our height if we expand arrow
        if (property.isExpanded)
        {
            totalLines += 3;
        }
        
        return EditorGUIUtility.singleLineHeight * totalLines;
    }
}
*/