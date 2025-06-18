using System;
using UnityEngine;

public class SaveToJson : MonoBehaviour
{
    ConnectNodes findFirstNode;

    


    public void ToJson()
    {
        FindFirstNode();
		
		if(findFirstNode == null)
		{
			Debug.LogError("No starting node found. Please ensure there is a node with no connections.");
		}

		foreach (GameObject obj in findFirstNode.connectedTo)
		{

		}

	}

	private void FindFirstNode()
	{
		ConnectNodes[] nodes = FindObjectsByType<ConnectNodes>(FindObjectsSortMode.None);
		foreach (ConnectNodes node in nodes)
		{
			if (node.connectedFrom.Count == 0)
			{
				findFirstNode = node;
				break;
			}
		}
	}
}
