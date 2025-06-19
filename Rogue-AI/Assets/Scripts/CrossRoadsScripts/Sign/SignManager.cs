using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class SignManager : MonoBehaviour
{
    [SerializeField] private List<Sign> signs;
    [Tooltip("Data about what rooms can be encountered in this sign area")]
    [SerializeField] private CrossRoadData crossRoadData;
    [Tooltip("The Room scriptable object for the next sign post")]
    [SerializeField] private SignRoom signRoom;
    
    private int _numberOfSigns = 3;
    private int _placesToGo = 3;

    private void Start()
    {
        if (GameManager.GetRoom() is SignRoom room)
        {
            _numberOfSigns = room.numberOfSigns;
            _placesToGo = room.placesToGo;
            signRoom = room.nextSignRoom;
        }
        
        InitiateSigns();
    }

    public void InitiateSigns()
    {
        for (int i = 0; i < _numberOfSigns; i++)
        {
            signs[i].gameObject.SetActive(true);
            
            // Create and Send list of rooms
            signs[i].Init(crossRoadData.GetRandomRooms(_placesToGo), signRoom);
        }
    }
}
