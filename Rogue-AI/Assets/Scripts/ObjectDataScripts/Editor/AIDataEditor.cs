using System;
using UnityEditor;
using UnityEngine;

namespace ObjectDataScripts.Editor
{
    [CustomEditor(typeof(AIData))]
    public class AIDataEditor : UnityEditor.Editor
    {
        private SerializedProperty _aiName;
        private SerializedProperty _maxSanity;
        private SerializedProperty _startSanity;
        private SerializedProperty _aiType;
        private SerializedProperty _aiActions;
        private SerializedProperty _endOfTurnAction;

        private void OnEnable()
        {

            _aiName = serializedObject.FindProperty("aiName");
            _maxSanity = serializedObject.FindProperty("maxSanity");
            _startSanity = serializedObject.FindProperty("startSanity");
            _aiType = serializedObject.FindProperty("aiType");
            _aiActions = serializedObject.FindProperty("aiActions");
            _endOfTurnAction = serializedObject.FindProperty("endOfTurnAction");
        }


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
            //            EditorGUILayout.PropertyField(_cardName, new GUIContent("Card Name"));


            EditorGUILayout.PropertyField(_aiName, new GUIContent("Name"));
            
            
            
            // ↓↓ add after ↓↓
            if (data.AIActions.Count == 0)
            {
                EditorGUILayout.HelpBox("Cauition AI does not have any actions", MessageType.Error);
            }
        }

        
    }
}
