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
        
        EditorGUILayout.Space(20);
        EditorGUILayout.LabelField("Script References", EditorStyles.boldLabel);
        GUI.enabled = false;
        EditorGUILayout.ObjectField("Script", MonoScript.FromScriptableObject((EventRoom)target), typeof(EventRoom), false);
        EditorGUILayout.ObjectField("Scriptable Object", ((EventRoom)target), typeof(EventRoom), false);
        GUI.enabled = true;
        
        serializedObject.ApplyModifiedProperties();
    }

}
