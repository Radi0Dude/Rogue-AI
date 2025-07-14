using System;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;

namespace ObjectDataScripts.Editor
{
    [CustomEditor(typeof(CardData))]
    public class CardDataEditor : UnityEditor.Editor
    {
        
        private SerializedProperty _cardType;
        private SerializedProperty _promptType;
        private SerializedProperty _effectiveAgainst;
        private SerializedProperty _cardName;
        private SerializedProperty _cardDescription;
        private SerializedProperty _cardSymbol;
        private SerializedProperty _cardsToDraw;
        private SerializedProperty _cardsToDelete;
        
        
        private bool _selectedPromptype, _selectedEffective, _canPrompt, _canDraw, _canDelete;
        


        private void OnEnable()
        {
            _cardType = serializedObject.FindProperty("cardType");
            _promptType = serializedObject.FindProperty("promptType");
            _effectiveAgainst = serializedObject.FindProperty("effectiveAgainst");
            _cardName = serializedObject.FindProperty("cardName");
            _cardDescription = serializedObject.FindProperty("cardDescription");
            _cardSymbol = serializedObject.FindProperty("cardSymbol");
            _cardsToDraw = serializedObject.FindProperty("cardsToDraw");
            _cardsToDelete = serializedObject.FindProperty("cardsToDelete");
        }

        public override void OnInspectorGUI()
        {
            HandleEnumValueChanged((CardType)_cardType.intValue);

            serializedObject.UpdateIfRequiredOrScript();
            
            EditorGUILayout.LabelField(_cardName.stringValue.ToUpper(), EditorStyles.boldLabel);
            
            EditorGUILayout.Space(10);
            
            // ↑↑ add above base inspector ↑↑
            //base.OnInspectorGUI();
            // Custom GUI
            EditorGUILayout.LabelField("General Stats", EditorStyles.boldLabel);
            
            EditorGUILayout.PropertyField(_cardName, new GUIContent("Card Name"));
            if (_cardName.stringValue.Length <= 0)
            {
                EditorGUILayout.HelpBox("Cauition, Should be given a name", MessageType.Warning);
            }
            EditorGUILayout.PropertyField(_cardType, new GUIContent("Card Type"));
           
            if (!_canPrompt && !_canDraw && !_canDelete)
            {
                EditorGUILayout.HelpBox("No Card Type is selected and card won't work", MessageType.Error);
            }
            // Options to be visible depending on card types
            EditorGUI.indentLevel++;
            EditorGUILayout.LabelField("Specifications", EditorStyles.boldLabel);
            EditorGUIUtility.labelWidth = 200;
            if (_canPrompt)
            {
                EditorGUILayout.PropertyField(_promptType, new GUIContent("Prompt Type"));
                if (_selectedPromptype)
                {
                    EditorGUILayout.HelpBox("Cauition, no type selected", MessageType.Warning);
                }
                EditorGUILayout.PropertyField(_effectiveAgainst, new GUIContent("Effective Against"));
                if (_selectedEffective)
                {
                    EditorGUILayout.HelpBox("Cauition, no effectiveness selected", MessageType.Warning);
                }
            }
            
            if (_canDraw)
            {
                EditorGUILayout.PropertyField(_cardsToDraw, new GUIContent("Cards to Draw"));
                if (_cardsToDraw.intValue <= 0)
                {
                    EditorGUILayout.HelpBox("Cauition, value should be higher than 0", MessageType.Warning);
                }
            }

            if (_canDelete)
            {
                EditorGUILayout.PropertyField(_cardsToDelete, new GUIContent("Cards to Delete"));
                if (_cardsToDelete.intValue <= 0)
                {
                    EditorGUILayout.HelpBox("Cauition, value should be higher than 0", MessageType.Warning);
                }
            }

            EditorGUI.indentLevel--;
            
            EditorGUILayout.Space(20);
            
            EditorGUILayout.PropertyField(_cardDescription, new GUIContent("Tooltip Description"));
            if (_cardDescription.stringValue.Length == 0)
            {
                EditorGUILayout.HelpBox("Cauition, should be a description", MessageType.Warning);
            }
            
            EditorGUILayout.PropertyField(_cardSymbol, new GUIContent("Card Symbol"));
            if (!_cardSymbol.objectReferenceValue)
            {
                EditorGUILayout.HelpBox("Cauition, missing symbol", MessageType.Warning);
            }
            
            
            // ↓↓ add bellow base inspector ↓↓

            
            
            serializedObject.ApplyModifiedProperties();
        }

        private void HandleEnumValueChanged(CardType cardTypeValue)
        {
            _canPrompt = (cardTypeValue & CardType.Prompt) != 0;
            
            _canDraw = (cardTypeValue & CardType.Draw) != 0;

            _canDelete = (cardTypeValue & CardType.Delete) != 0;

            _selectedPromptype = _promptType.intValue == 0;

            _selectedEffective = _effectiveAgainst.intValue == 0;

        }
    }
}

