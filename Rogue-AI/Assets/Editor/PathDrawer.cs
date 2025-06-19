using UnityEditor;
using UnityEngine;
[CustomPropertyDrawer(typeof(PathAttribute))]
public class PathDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.BeginProperty(position, label, property);

		position.width -= 60;
		property.stringValue = EditorGUI.TextField(position, label, property.stringValue);

		Rect buttonRect = position;
		buttonRect.x += position.width + 2;
		buttonRect.width = 56;
		if (GUI.Button(buttonRect, "Browse"))
		{
			string path = EditorUtility.OpenFolderPanel("Select Folder", Application.dataPath, "");
			if (!string.IsNullOrEmpty(path))
			{
				property.stringValue = path;
			}
		}
		EditorGUI.EndProperty();
	}
}
