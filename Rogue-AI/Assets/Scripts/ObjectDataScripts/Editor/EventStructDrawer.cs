using UnityEditor;
using UnityEngine;

namespace ObjectDataScripts.Editor
{
    [CustomPropertyDrawer(typeof(EventStruct))]
    public class EventStructDrawer : PropertyDrawer
    {
        private SerializedProperty _eventDescription;
        private SerializedProperty _eventEffectTooltip;
        private SerializedProperty _eventResults;
        
        private SerializedProperty _currentHealthChange;
        private SerializedProperty _maxHealthChange;
        private SerializedProperty _drawAmountChange;
        private SerializedProperty _cardToAdd;
        private SerializedProperty _numberOfCardsToDelete;

        private bool _isCurrentHealthChange,
        _isMaxHealthChange,
        _isDrawAmountChange,
        _isCardToAdd,
        _isNumberOfCardsToDelete;
        
        private float _currentLineHeight;
        
        // Draw to inspector window
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            
            // find and set properties
            _eventDescription = property.FindPropertyRelative("eventDescription");
            _eventEffectTooltip = property.FindPropertyRelative("eventEffectTooltip");
            _eventResults = property.FindPropertyRelative("eventResults");
            
            _currentHealthChange = property.FindPropertyRelative("currentHealthChange");
            _maxHealthChange = property.FindPropertyRelative("maxHealthChange");
            _drawAmountChange = property.FindPropertyRelative("drawAmountChange");
            _cardToAdd = property.FindPropertyRelative("cardToAdd");
            _numberOfCardsToDelete = property.FindPropertyRelative("numberOfCardsToDelete");

            _currentLineHeight = EditorGUIUtility.singleLineHeight;
            
            
            HandleEnumValueChanged((EventResults)_eventResults.intValue);

            
            // Drawing instructions
            Rect foldOutBox = new Rect(position.min.x, position.min.y, position.size.x, EditorGUIUtility.singleLineHeight);
            property.isExpanded = EditorGUI.Foldout(foldOutBox, property.isExpanded, label);
            
            // Draw properties only when expanded
            if (property.isExpanded)
            {
                DrawTextFieldVariable(position, _eventDescription, "Choice Description");
                DrawTextFieldVariable(position, _eventEffectTooltip, "Choice Tooltip");
                DrawNormalVariable(position, _eventResults, "Choice Results");
                
                VisibleVariables(position);
                
                if (_isCardToAdd)
                {
                    Rect drawArea = new Rect(position.min.x, position.min.y, position.size.x, EditorGUIUtility.singleLineHeight);

                    drawArea = ChangeHeight(drawArea);
                    _cardToAdd.objectReferenceValue = EditorGUI.ObjectField(drawArea, "Card to Add", _cardToAdd.objectReferenceValue, typeof(CardData), false);
                }
            }
            
            
            EditorGUI.EndProperty();
        }
        

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            int totalLines = 1;

            if (property.isExpanded)
            {
                totalLines += 3;
                
                _eventResults = property.FindPropertyRelative("eventResults");

                int enumValue = _eventResults.intValue;
                int numbOfEnums = CountSetBits(enumValue);


                totalLines += numbOfEnums;
            }
            
            float singleLine = EditorGUIUtility.singleLineHeight;
            
            return singleLine * totalLines;
        }

        
        
        private void DrawNormalVariable(Rect position, SerializedProperty property, string name)
        {
            Rect drawArea = new Rect(position.min.x, position.min.y, position.size.x, EditorGUIUtility.singleLineHeight);

            drawArea = ChangeHeight(drawArea);
            EditorGUI.PropertyField(drawArea, property, new GUIContent(name));
        }
        private void DrawTextFieldVariable(Rect position, SerializedProperty property, string name)
        {
            Rect drawArea = new Rect(position.min.x, position.min.y, position.size.x, EditorGUIUtility.singleLineHeight);
            
            drawArea = ChangeHeight(drawArea);
            EditorGUI.PropertyField(drawArea, property, new GUIContent(name));
        }

        private void DrawEmptySpace(Rect position)
        {
            Rect emptyArea = new Rect(position.min.x, position.min.y, position.size.x, EditorGUIUtility.singleLineHeight);
            emptyArea = ChangeHeight(emptyArea);
            EditorGUI.DrawRect(emptyArea, Color.clear);
        }

        private Rect ChangeHeight(Rect drawArea)
        {
            //Increase line height to be placed correct
            drawArea.y += _currentLineHeight;

            _currentLineHeight += drawArea.height;

            return drawArea;
        }

        private void HandleEnumValueChanged(EventResults eventValue)
        {
            _isCurrentHealthChange = (eventValue & EventResults.ChangeHealth) != 0;
            
            _isMaxHealthChange = (eventValue & EventResults.ChangeMaxHealth) != 0;

            _isDrawAmountChange = (eventValue & EventResults.ChangeDrawAmount) != 0;

            _isCardToAdd = (eventValue & EventResults.AddCard) != 0;

            _isNumberOfCardsToDelete = (eventValue & EventResults.RemoveCard) != 0;

        }
        private void VisibleVariables(Rect position)
        {
            if (_isCurrentHealthChange)
            {
                DrawNormalVariable(position, _currentHealthChange, "Health Change");
            }

            if (_isMaxHealthChange)
            {
                DrawNormalVariable(position, _maxHealthChange, "Max Health Change");
            }

            if (_isDrawAmountChange)
            {
                DrawNormalVariable(position, _drawAmountChange, "Draw Amount Change");
            }
            if (_isNumberOfCardsToDelete)
            {
                DrawNormalVariable(position, _numberOfCardsToDelete, "Number To Delete");
            }
            

            
        }

        int CountSetBits(int value)
        {
            int count = 0;
            while (value != 0)
            {
                count += value & 1;
                value >>= 1;
            }
            return count;
        }
    }
}
