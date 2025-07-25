using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class SignManager : MonoBehaviour
{
    [SerializeField] private List<Sign> signs;
    
    private SignRoom _signRoom;
    private CrossRoadData _crossRoadData;
    private int _numberOfSigns = 3;

    private void Start()
    {
        if (GameManager.GetRoom() is SignRoom room)
        {
            _numberOfSigns = room.NumberOfSigns;
            _crossRoadData = room.CrossRoadData;
            _signRoom = room.NextSignRoom;
        }
        
        InitiateSigns();
    }

    private void InitiateSigns()
    {
        for (int i = 0; i < _numberOfSigns; i++)
        {
            signs[i].gameObject.SetActive(true);
            
            // Create and Send list of rooms
            signs[i].Init(_crossRoadData.GetRandomRooms(3), _signRoom);
        }
    }
}
