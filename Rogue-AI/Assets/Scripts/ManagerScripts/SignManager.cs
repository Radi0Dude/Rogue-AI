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
        // Get total number of rooms needed (e.g., 3 signs * 3 options per sign)
        int totalRoomCount = _numberOfSigns * 3;

        // Get unique random rooms
        List<RoomData> allRandomRooms = _crossRoadData.GetRandomRooms(totalRoomCount);

        // Assign rooms to each sign
        for (int i = 0; i < _numberOfSigns; i++)
        {
            signs[i].gameObject.SetActive(true);

            // Take 3 rooms per sign
            var roomOptions = allRandomRooms.GetRange(i * 3, 3);
            signs[i].Init(roomOptions, _signRoom);
        }
    }

}
