using System;
using UnityEditor;
using UnityEngine;

namespace ObjectDataScripts.Editor
{
    [CustomEditor(typeof(TreasureRoom))]
    public class TreasureRoomEditor : UnityEditor.Editor
    {
        private SerializedProperty _roomName;
        private SerializedProperty _roomIcon;
        private SerializedProperty _rewardType;
        private SerializedProperty _rewardSprite;
        private SerializedProperty _rewardName;
        private SerializedProperty _rewardDescription;
        
        private SerializedProperty _rewardCard;
        private SerializedProperty _numberToDelete;

        private bool _isCard, _isDelete, _isPowerUp;

        private void OnEnable()
        {
            _roomName = serializedObject.FindProperty("roomName");
            _roomIcon = serializedObject.FindProperty("roomIcon");
            _rewardType = serializedObject.FindProperty("rewardType");
            _rewardSprite = serializedObject.FindProperty("rewardSprite");
            _rewardName = serializedObject.FindProperty("rewardName");
            _rewardDescription = serializedObject.FindProperty("rewardDescription");
            
            _rewardCard = serializedObject.FindProperty("rewardCard");
            _numberToDelete = serializedObject.FindProperty("numberToDelete");
        }

        public override void OnInspectorGUI()
        {
            HandleEnumValueChanged((RewardType)_rewardType.enumValueIndex);
            
            serializedObject.UpdateIfRequiredOrScript();
            
            
            EditorGUILayout.LabelField(_roomName.stringValue.ToUpper(), EditorStyles.boldLabel);
            EditorGUILayout.Space(10);
            
            EditorGUILayout.PropertyField(_roomName, new GUIContent("Room Name"));
            EditorGUILayout.PropertyField(_roomIcon, new GUIContent("Room Icon"));
            
            EditorGUILayout.LabelField("Treasure Room Settings", EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(_rewardType, new GUIContent("Reward Type"));

            EditorGUI.indentLevel++;

            if (_isCard)
            {
                EditorGUILayout.ObjectField(_rewardCard, typeof(CardData), new GUIContent("Reward Card"));
            }

            if (_isDelete)
            {
                EditorGUILayout.PropertyField(_numberToDelete, new GUIContent("Number To Delete"));
            }
            
            EditorGUI.indentLevel--;
            EditorGUILayout.Space(20);
            
            EditorGUILayout.PropertyField(_rewardName, new GUIContent("Reward Name"));
            EditorGUILayout.PropertyField(_rewardSprite, new GUIContent("Reward Sprite"));



            EditorGUILayout.PropertyField(
                _rewardDescription,
                new GUIContent("Reward Description"),
                GUILayout.Height(EditorGUI.GetPropertyHeight(_rewardDescription, true) + 40) // Increase height
            );
            

            
            serializedObject.ApplyModifiedProperties();
        }

        private void HandleEnumValueChanged(RewardType typeValue)
        {
            _isCard = typeValue == RewardType.Card;
            
            _isDelete = typeValue == RewardType.Delete;
            
            _isPowerUp = typeValue == RewardType.PowerUp;
        }

    }
}
