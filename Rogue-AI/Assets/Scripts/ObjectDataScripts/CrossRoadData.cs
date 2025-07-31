using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "CrossRoadData", menuName = "CrossRoad/CrossRoad")]
public class CrossRoadData : ScriptableObject
{
    [SerializeField] private List<RoomData> rooms;

    public List<RoomData> GetRandomRooms(int amount)
    {
        // Copy the list locally so we don't modify the original
        var shuffledRooms = new List<RoomData>(rooms);

        // Shuffle the copied list
        for (int i = shuffledRooms.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (shuffledRooms[i], shuffledRooms[j]) = (shuffledRooms[j], shuffledRooms[i]);
        }

        // Return the first `amount` items (clamped to avoid overflow)
        return shuffledRooms.GetRange(0, Mathf.Min(amount, shuffledRooms.Count));
    }

} 
