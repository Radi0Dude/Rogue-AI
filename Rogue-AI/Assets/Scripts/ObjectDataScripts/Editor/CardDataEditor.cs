using UnityEditor;

namespace ObjectDataScripts.Editor
{
    [CustomEditor(typeof(CardData))]
    public class CardDataEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            CardData data = (CardData)target;
            
            EditorGUILayout.LabelField(data.CardName.ToUpper(), EditorStyles.boldLabel);
            EditorGUILayout.Space(10);
            // ↑↑ add before ↑↑
            base.OnInspectorGUI();
            // ↓↓ add after ↓↓

            if (!data.CardSymbol)
            {
                EditorGUILayout.HelpBox("Cauition missing symbol", MessageType.Warning);
            }
        }
    }
}

