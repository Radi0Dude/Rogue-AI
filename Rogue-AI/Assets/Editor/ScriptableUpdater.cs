using UnityEditor;
using UnityEngine;

public class ScriptableUpdater
{
	[MenuItem("Assets/Force Reserialize All CardData")]
	public static void ReserializeAllCardData()
	{
		string[] guids = AssetDatabase.FindAssets("t:CardData");
		foreach (string guid in guids)
		{
			string path = AssetDatabase.GUIDToAssetPath(guid);
			AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
		}
		Debug.Log("Reserialized all CardData assets.");
	}
}
