using UnityEngine;
using UnityEditor;


[CustomPropertyDrawer(typeof(AIActionData), true)]
public class AIActionDrawer : PropertyDrawer
{
    // How to draw to the Inspector window
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Draw the object reference field (i.e., the picker for the ScriptableObject)
        Rect referenceRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.BeginProperty(referenceRect, label, property);
        property.objectReferenceValue = EditorGUI.ObjectField(referenceRect, label, property.objectReferenceValue, typeof(AIActionData), false);
        EditorGUI.EndProperty();

        // If no object is assigned, stop here
        if (property.objectReferenceValue == null)
            return;

        // Toggle foldout to show/hide the SO fields
        property.isExpanded = EditorGUI.Foldout(
            new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight),
            property.isExpanded,
            GUIContent.none,
            true);

        if (!property.isExpanded)
            return;

        // Draw inline fields
        SerializedObject so = new SerializedObject(property.objectReferenceValue);
        so.Update();
        

        EditorGUI.indentLevel++;

        SerializedProperty prop = so.GetIterator();
        prop.NextVisible(true); // Skip "m_Script"

        float y = position.y + EditorGUIUtility.singleLineHeight;

        while (prop.NextVisible(false))
        {
            float propHeight = EditorGUI.GetPropertyHeight(prop, true);
            Rect propRect = new Rect(position.x, y, position.width, propHeight);
            EditorGUI.PropertyField(propRect, prop, true);
            y += propHeight + EditorGUIUtility.standardVerticalSpacing;
        }

        EditorGUI.indentLevel--;

        so.ApplyModifiedProperties();
    }
    

    // request vertical spacing, return it
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float height = EditorGUIUtility.singleLineHeight; // For the object reference field

        if (property.objectReferenceValue != null && property.isExpanded)
        {
            SerializedObject so = new SerializedObject(property.objectReferenceValue);
            SerializedProperty prop = so.GetIterator();

            prop.NextVisible(true); // Skip "m_Script"
            while (prop.NextVisible(false))
            {
                height += EditorGUI.GetPropertyHeight(prop, true) + EditorGUIUtility.standardVerticalSpacing;
            }
        }

        return height;
    }
}
