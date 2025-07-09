using System;
using UnityEditor;

namespace ObjectDataScripts.Editor
{
    [CustomEditor(typeof(CardData))]
    public class CardDataEditor : UnityEditor.Editor
    {
        
        private SerializedProperty cardType;
        private SerializedProperty promptType;
        private SerializedProperty effectiveAgainst;
        private SerializedProperty cardName;
        private SerializedProperty cardDescription;
        private SerializedProperty cardSymbol;
        private SerializedProperty cardsToDraw;
        private SerializedProperty cardsToDelete;
        
        


        private void OnEnable()
        {
            cardType = serializedObject.FindProperty("cardType");
            promptType = serializedObject.FindProperty("promptType");
            effectiveAgainst = serializedObject.FindProperty("effectiveAgainst");
            cardName = serializedObject.FindProperty("cardName");
            cardDescription = serializedObject.FindProperty("cardDescription");
            cardSymbol = serializedObject.FindProperty("cardSymbol");
            cardsToDraw = serializedObject.FindProperty("cardsToDraw");
            cardsToDelete = serializedObject.FindProperty("cardsToDelete");
        }

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

