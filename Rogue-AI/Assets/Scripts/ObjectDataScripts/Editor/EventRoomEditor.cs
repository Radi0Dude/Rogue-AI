using System;
using System.Security.Cryptography;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EventRoom))]
public class EventRoomEditor : UnityEditor.Editor
{
    // From RoomData
    private SerializedProperty _roomName;
    private SerializedProperty _roomIcon;
    // From EventRoom
    private SerializedProperty _eventDescription;
    private SerializedProperty _optionDescriptions;


    private void OnEnable()
    {
        _roomName = serializedObject.FindProperty("roomName");
        _roomIcon = serializedObject.FindProperty("roomIcon");
        
        _eventDescription = serializedObject.FindProperty("eventDescription");
        _optionDescriptions = serializedObject.FindProperty("optionDescriptions");
        
        
        
    }

    public override void OnInspectorGUI()
    {
        
        
        serializedObject.UpdateIfRequiredOrScript();
        
        EditorGUILayout.LabelField(_roomName.stringValue.ToUpper(), EditorStyles.boldLabel);
        EditorGUILayout.Space(10);
        
        EditorGUILayout.PropertyField(_roomName, new GUIContent("Room Name"));
        EditorGUILayout.PropertyField(_roomIcon, new GUIContent("Room Icon"));
        
        EditorGUILayout.PropertyField(_eventDescription, new GUIContent("Event Description"));
        EditorGUILayout.PropertyField(_optionDescriptions, new GUIContent("Option Descriptions"));
        
        
        serializedObject.ApplyModifiedProperties();
    }
    /*
        _currentHealthChange = serializedObject.FindProperty("currentHealthChange");
        _maxHealthChange = serializedObject.FindProperty("maxHealthChange");
        _drawAmountChange = serializedObject.FindProperty("drawAmountChange");
        _cardToAdd = serializedObject.FindProperty("cardToAdd");
        _numberOfCardsToDelete = serializedObject.FindProperty("numberOfCardsToDelete");
        EditorGUILayout.PropertyField(_currentHealthChange, new GUIContent("Current Health Change"));
        EditorGUILayout.PropertyField(_maxHealthChange, new GUIContent("Max Health Change"));
        EditorGUILayout.PropertyField(_drawAmountChange, new GUIContent("Draw Amount Change"));
        EditorGUILayout.PropertyField(_cardToAdd, new GUIContent("Card To Add"));
        EditorGUILayout.PropertyField(_numberOfCardsToDelete, new GUIContent("Number of Cards to Delete"));
     */

}
