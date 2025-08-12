using System.Collections.Generic;
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
        private SerializedProperty _aiModel;
        private SerializedProperty _aiActions;
        private SerializedProperty _endOfTurnAction;
        private void OnEnable()
        {

            _aiName = serializedObject.FindProperty("aiName");
            _maxSanity = serializedObject.FindProperty("maxSanity");
            _startSanity = serializedObject.FindProperty("startSanity");
            _aiModel = serializedObject.FindProperty("aiModelType");
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
            EditorGUILayout.PropertyField(_aiModel, new GUIContent("AI Model"));
            
            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Action Part",EditorStyles.boldLabel);
            EditorGUILayout.Space(10);
            
            ActionProbabilityBar();
            
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

        private void ActionProbabilityBar()
        {
            // Dict: nameOfAction, NumberOfTimesUsed
            Dictionary<AIActionData, int> dict = new ();
            int amountOfValues = 0;
            
            // Fill Dict with Actions from AI
            for (int i = 0; i < _aiActions.arraySize; i++)
            {
                AIActionData action = (AIActionData)_aiActions.GetArrayElementAtIndex(i).objectReferenceValue;

                if (!dict.TryAdd(action, 1))
                {
                    dict[action]++;
                }

                amountOfValues++;
            }
            

            // Get full width of inspector
            Rect fullRect = GUILayoutUtility.GetRect(EditorGUIUtility.currentViewWidth, 40f);
            

            float xPos = fullRect.x;
            float yPos = fullRect.y;
            float height = fullRect.height;
            float width = fullRect.width;

            float xUsed = 0.0f;

            // Create Rects based on dict size
            foreach (var dictData in dict)
            {
                // Create rect with length and corner pos based on dictData's Value
                float part = ((float)dictData.Value/amountOfValues);
                
                Rect rect = new Rect(xPos + (xUsed * width), yPos, width * part, height);
                Color color = dictData.Key.BarColor;
                string rectText = dictData.Key.ActionName + " " + Mathf.Round(part * 100) + "%";
                
                EditorGUI.DrawRect(rect, color);
                EditorGUI.LabelField(rect, rectText, GetCenteredStyle());
                
                xUsed += part;
            }
        }
        
        private GUIStyle GetCenteredStyle()
        {
            var centeredStyle = new GUIStyle(EditorStyles.label);
            centeredStyle.alignment = TextAnchor.MiddleCenter;
            centeredStyle.normal.textColor = Color.white;
            centeredStyle.wordWrap = true;
            centeredStyle.fontStyle = FontStyle.Bold;
            
            return centeredStyle;
        }
    }
}
