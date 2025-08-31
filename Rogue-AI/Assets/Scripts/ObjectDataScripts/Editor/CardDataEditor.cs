using System;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;

using UnityEditor;
using UnityEngine;

namespace ObjectDataScripts.Editor
{
	[CustomEditor(typeof(CardData))]
	public class CardDataEditor : UnityEditor.Editor
	{
		private SerializedProperty _cardType;
		private SerializedProperty _cardRarity;
		private SerializedProperty _promptType;
		private SerializedProperty _cardName;
		private SerializedProperty _cardDescription;
		private SerializedProperty _cardSymbol;
		private SerializedProperty _cardsToDraw;
		private SerializedProperty _cardsToDelete;
		private SerializedProperty _cardsToDiscard;
		private SerializedProperty _isPlayedEndOfTurn;
		private SerializedProperty _virusEffect;
		private SerializedProperty _promptPlacement;
		private SerializedProperty _cardPromptUpdate;
		private SerializedProperty _cardata;

		private bool _selectedPromptType, _selectedEffective, _canPrompt, _canDraw, _canDelete, _canDiscard, _canStatus;

		private void OnEnable()
		{
			_cardType = serializedObject.FindProperty("cardType");
			_promptType = serializedObject.FindProperty("promptType");
			_cardRarity = serializedObject.FindProperty("cardRarity");
			_cardName = serializedObject.FindProperty("cardName");
			_cardDescription = serializedObject.FindProperty("cardDescription");
			_cardSymbol = serializedObject.FindProperty("cardSymbol");
			_cardsToDraw = serializedObject.FindProperty("cardsToDraw");
			_cardsToDelete = serializedObject.FindProperty("cardsToDelete");
			_cardsToDiscard = serializedObject.FindProperty("cardsToDiscard");
			_isPlayedEndOfTurn = serializedObject.FindProperty("isPlayedEndOfTurn");
			_virusEffect = serializedObject.FindProperty("virusEffect");
			_promptPlacement = serializedObject.FindProperty("promptPlacement");
			_cardPromptUpdate = serializedObject.FindProperty("cardPromptUpdate");
			_cardata = serializedObject.FindProperty("cardata");
		}

		public override void OnInspectorGUI()
		{
			HandleEnumValueChanged((CardType)_cardType.intValue);

			serializedObject.UpdateIfRequiredOrScript();

			EditorGUILayout.LabelField(_cardName.stringValue.ToUpper(), EditorStyles.boldLabel);

			EditorGUILayout.Space(10);

			EditorGUILayout.LabelField("General Stats", EditorStyles.boldLabel);

			EditorGUILayout.PropertyField(_cardName, new GUIContent("Card Name"));
			if (_cardName.stringValue.Length <= 0)
			{
				EditorGUILayout.HelpBox("Cauition, Should be given a name", MessageType.Warning);
			}
			EditorGUILayout.PropertyField(_cardType, new GUIContent("Card Type"));
			EditorGUILayout.PropertyField(_cardRarity, new GUIContent("Card Rarity"));

			if ((!_canPrompt && !_canDraw && !_canDelete && !_canStatus && !_canDiscard))
			{
				EditorGUILayout.HelpBox("No Card Type is selected and card won't work", MessageType.Error);
			}

			EditorGUI.indentLevel++;
			EditorGUILayout.LabelField("Specifications", EditorStyles.boldLabel);
			EditorGUIUtility.labelWidth = 200;
			if (_canPrompt)
			{
				EditorGUILayout.PropertyField(_promptType, new GUIContent("Prompt Type"));
			}

			if (_canDraw)
			{
				EditorGUILayout.PropertyField(_cardsToDraw, new GUIContent("Cards to Draw"));
			}

			if (_canDelete)
			{
				EditorGUILayout.PropertyField(_cardsToDelete, new GUIContent("Cards to Delete"));
			}

			if (_canDiscard)
			{
				EditorGUILayout.PropertyField(_cardsToDiscard, new GUIContent("Cards to Discard"));
			}

			if (_canStatus)
			{
				EditorGUILayout.PropertyField(_isPlayedEndOfTurn, new GUIContent("Will be Played End of Turn"));
				EditorGUILayout.PropertyField(_virusEffect, new GUIContent("Virus Effect"));
			}

			EditorGUI.indentLevel--;

			EditorGUILayout.Space(20);

			EditorGUILayout.PropertyField(_cardDescription, new GUIContent("Tooltip Description"));

			EditorGUILayout.PropertyField(_cardSymbol, new GUIContent("Card Symbol"));

			EditorGUILayout.LabelField("General Stats", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(_cardName, new GUIContent("Card Name"));

			EditorGUILayout.PropertyField(_promptPlacement, new GUIContent("Prompt Placement"));
			EditorGUILayout.PropertyField(_cardPromptUpdate, new GUIContent("Card Prompt Update"));
			EditorGUILayout.PropertyField(_cardata, new GUIContent("Card Data"));

			EditorGUILayout.Space(20);
			EditorGUILayout.LabelField("Script References", EditorStyles.boldLabel);

			GUI.enabled = false;
			EditorGUILayout.ObjectField("Script", MonoScript.FromScriptableObject((CardData)target), typeof(CardData), false);
			EditorGUILayout.ObjectField("Scriptable Object", ((CardData)target), typeof(CardData), false);
			GUI.enabled = true;

			serializedObject.ApplyModifiedProperties();
		}

		private void HandleEnumValueChanged(CardType cardTypeValue)
		{
			_canPrompt = (cardTypeValue & CardType.Prompt) != 0;
			_canDraw = (cardTypeValue & CardType.Draw) != 0;
			_canDelete = (cardTypeValue & CardType.Delete) != 0;
			_canStatus = (cardTypeValue & CardType.Status) != 0;
			_canDiscard = (cardTypeValue & CardType.Discard) != 0;
			_selectedPromptType = _promptType.intValue == 0;
		}
	}
}
