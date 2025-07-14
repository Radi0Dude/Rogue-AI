using UnityEditor;
using UnityEngine;

public class WindowDisplay : EditorWindow
{
    [MenuItem("Custom/Window Display")]
    public static void ShowWindow()
    {
        GetWindow<WindowDisplay>("Window");
    }
    private void OnGUI()
    {
        
        
        EditorGUILayout.Space(10);
        if (GUILayout.Button("Button"))
        {
            
        }
    }
}
