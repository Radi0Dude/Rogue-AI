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
            
            serializedObject.UpdateIfRequiredOrScript();

            // Name Display
            EditorGUILayout.LabelField(_aiName.stringValue.ToUpper(), EditorStyles.boldLabel);
            EditorGUILayout.Space(10);
            
            // AI Health Bar
            float barValue = _startSanity.floatValue/_maxSanity.floatValue;
            string barName = "Sanity Bar : " + _startSanity.floatValue + "/" + _maxSanity.floatValue;
            MyEditorFunctions.MyProgressBarGUI(barValue, barName);
            // ↑↑ add before ↑↑
            //base.OnInspectorGUI();
            

            EditorGUILayout.PropertyField(_aiName, new GUIContent("Name"));
            EditorGUILayout.PropertyField(_maxSanity, new GUIContent("Max Sanity"));
            EditorGUILayout.PropertyField(_startSanity, new GUIContent("Start Sanity"));
            EditorGUILayout.PropertyField(_aiType, new GUIContent("AI Type"));
            EditorGUILayout.PropertyField(_aiActions, new GUIContent("AI Actions"));
            if (_aiActions.arraySize == 0)
            {
                EditorGUILayout.HelpBox("Cauition AI does not have any actions", MessageType.Error);
            }
            EditorGUILayout.PropertyField(_endOfTurnAction, new GUIContent("End Of Turn Action"));
            
            
            
            // ↓↓ add after ↓↓

            EditorGUILayout.Space(20);
            EditorGUILayout.LabelField("Script References", EditorStyles.boldLabel);
            GUI.enabled = false;
            EditorGUILayout.ObjectField("Script", MonoScript.FromScriptableObject((AIData)target), typeof(AIData), false);
            EditorGUILayout.ObjectField("Scriptable Object", ((AIData)target), typeof(AIData), false);
            GUI.enabled = true;
            
            serializedObject.ApplyModifiedProperties();

        }

        
    }
}
