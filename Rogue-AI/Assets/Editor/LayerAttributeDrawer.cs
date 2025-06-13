using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(LayerAttribute))]
public class LayerAttributeDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		if (property.propertyType != SerializedPropertyType.String)
		{
			EditorGUI.LabelField(position, label.text, "Use [Layer] on a string field.");
			return;
		}

		string[] layers = GetAllLayers();

		int index = Mathf.Max(0, System.Array.IndexOf(layers, property.stringValue));

		int selected = EditorGUI.Popup(position, label.text, index, layers);
		if (selected >= 0 && selected < layers.Length)
		{
			property.stringValue = layers[selected];
		}
	}

	private string[] GetAllLayers()
	{
		var layerList = new System.Collections.Generic.List<string>();
		for (int i = 0; i < 32; i++)
		{
			string name = LayerMask.LayerToName(i);
			if (!string.IsNullOrEmpty(name))
				layerList.Add(name);
		}
		return layerList.ToArray();
	}
}
