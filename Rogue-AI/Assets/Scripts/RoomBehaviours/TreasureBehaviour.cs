using System;
using UnityEngine;

public enum RewardType
{
    Card,
    Delete,
    PowerUp,
}
public class TreasureBehaviour : MonoBehaviour
{
    private TreasureRoom _data;
    private RewardType _rewardType;
    // Display Reward from Room Data
    private void Start()
    {
        if (GameManager.GetRoom() is TreasureRoom room)
        {
            _data = room;
            _rewardType = room.RewardType;
        }
        else
        {
            Debug.LogError("The current room in GameManager is not a TreasureRoom");
            return;
        }
        

    }
    
    // Receive Reward When Pressed
    private void OnButtonPressedReceiveReward()
    {
        // Figure out what type of action are necessary based on reward type
        switch (_rewardType)
        {
            case RewardType.Card:
                break;
            case RewardType.Delete:
                break;
            case RewardType.PowerUp:
                break;
        }
        
    }
}
