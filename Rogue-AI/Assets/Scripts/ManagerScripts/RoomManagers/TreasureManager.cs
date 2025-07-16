using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum RewardType
{
    Card,
    Delete,
    PowerUp,
}
public class TreasureManager : MonoBehaviour
{
    [SerializeField] private GameObject treasurePanel;
    [SerializeField] private Image treasureIcon;
    [SerializeField] private TextMeshProUGUI treasureTitle, treasureDescription;
    
    [SerializeField] private CardLibrary cardLibrary;
    
    private TreasureRoom _data;
    private RewardType _rewardType;
    // Display Reward from Room Data
    private void Start()
    {
        if (GameManager.GetRoom() is TreasureRoom room)
        {
            _data = room;
            _rewardType = _data.RewardType;
        }
        else
        {
            Debug.LogError("The current room in GameManager is not a TreasureRoom");
            return;
        }

        treasureIcon.sprite = _data.RewardSprite;
        treasureTitle.text = "----" + _data.RewardName + "----";
        treasureDescription.text = _data.RewardDescription;

    }
    
    // Receive Reward When Pressed
    public void OnButtonPressedReceiveReward()
    {
        // Figure out what type of action are necessary based on reward type
        switch (_rewardType)
        {
            case RewardType.Card:
                GameManager.PlayerCardCollection.AddCardToCollection(_data.RewardCard);
                GameManager.RemoveRoomFromListAndLoadNextScene();
                break;
            case RewardType.Delete:
                cardLibrary.StartDeleteCards(_data.NumberToDelete, false);
                break;
            case RewardType.PowerUp:
                break;
        }
    }

    public void OnOpenLibrary()
    {
        treasurePanel.SetActive(false);
    }

    public void OnCloseLibrary()
    {
        treasurePanel.SetActive(true);
    }

    public void SkipRoom()
    {
        GameManager.RemoveRoomFromListAndLoadNextScene();
    }
}
