using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "CrossRoadData", menuName = "CrossRoad/CrossRoad")]
public class CrossRoadData : ScriptableObject
{
    public List<RoomData> rooms;

    public List<RoomData> GetRandomRooms(int amount)
    {
        var randomRooms = new List<RoomData>();
        
        for (int i = 0; i < amount; i++)
        {
            randomRooms.Add(rooms[Random.Range(0, rooms.Count)]);
        }
        
        return randomRooms;
    }
} 
