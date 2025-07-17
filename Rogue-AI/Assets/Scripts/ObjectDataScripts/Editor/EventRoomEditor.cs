using System;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EventRoom))]
public class EventRoomEditor : UnityEditor.Editor
{
    // From RoomData
    private SerializedProperty _roomName;
    private SerializedProperty _roomIcon;
    // From EventRoom
    private SerializedProperty _eventRoomName;
    private SerializedProperty _eventDescription;
    private SerializedProperty _optionDescriptions;
    private SerializedProperty _currentHealthChange;
    private SerializedProperty _maxHealthChange;
    private SerializedProperty _drawAmountChange;
    private SerializedProperty _cardToAdd;
    private SerializedProperty _numberOfCardsToDelete;


    private void OnEnable()
    {
        _roomName = serializedObject.FindProperty("roomName");
        _roomIcon = serializedObject.FindProperty("roomIcon");
        _eventRoomName = serializedObject.FindProperty("eventRoomName");
        _eventDescription = serializedObject.FindProperty("eventDescription");
        _optionDescriptions = serializedObject.FindProperty("optionDescriptions");
        _currentHealthChange = serializedObject.FindProperty("currentHealthChange");
        _maxHealthChange = serializedObject.FindProperty("maxHealthChange");
        _drawAmountChange = serializedObject.FindProperty("drawAmountChange");
        _cardToAdd = serializedObject.FindProperty("cardToAdd");
        _numberOfCardsToDelete = serializedObject.FindProperty("numberOfCardsToDelete");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.UpdateIfRequiredOrScript();
        
        base.OnInspectorGUI();
        
        serializedObject.ApplyModifiedProperties();
    }
    

}
