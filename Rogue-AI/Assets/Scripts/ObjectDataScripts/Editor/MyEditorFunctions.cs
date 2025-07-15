using UnityEditor;
using UnityEngine;

namespace ObjectDataScripts.Editor
{
    public class MyEditorFunctions : UnityEditor.Editor
    {
        public static void MyProgressBarGUI(float value, string label)
        {
            Rect rect = GUILayoutUtility.GetRect(18,30, "TextField");
            EditorGUI.ProgressBar(rect, value, label);
            EditorGUILayout.Space(20);
        }
    }
}
