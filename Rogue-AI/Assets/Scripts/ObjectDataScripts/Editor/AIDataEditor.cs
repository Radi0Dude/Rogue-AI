using UnityEditor;

namespace ObjectDataScripts.Editor
{
    [CustomEditor(typeof(AIData))]
    public class AIDataEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            // Reference
            AIData data = (AIData)target;
            
            // Name Display
            EditorGUILayout.LabelField(data.AIName.ToUpper(), EditorStyles.boldLabel);
            EditorGUILayout.Space(10);
            
            // AI Health Bar
            float barValue = data.StartSanity/data.MaxSanity;
            string barName = "Sanity Bar : " + data.StartSanity + "/" + data.MaxSanity;
            MyEditorFunctions.MyProgressBarGUI(barValue, barName);
            // ↑↑ add before ↑↑
            base.OnInspectorGUI();
            // ↓↓ add after ↓↓
            if (data.AIActions.Count == 0)
            {
                EditorGUILayout.HelpBox("Cauition AI does not have any actions", MessageType.Error);
            }
        }

        
    }
}
