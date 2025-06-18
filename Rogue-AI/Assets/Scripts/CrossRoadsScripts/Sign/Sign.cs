using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sign : MonoBehaviour
{
    [SerializeField] private List<Image> iconPos;
    
    private List<RoomData> _rooms = new();
    
    
    public void Init(List<RoomData> rooms, SignRoom signRoom)
    {
        _rooms = rooms;
        SetIcons();
        _rooms.Add(signRoom);
    }

    private void SetIcons()
    {
        for (int i = 0; i < _rooms.Count; i++)
        {
            iconPos[i].sprite = _rooms[i].roomIcon;
        }
    }

    // Called by OnPressed() button from unity
    public void ChoosePathWhenPressed()
    {
        // Should save selected rooms down path
        GameManager.AddRoomsToList(_rooms);
    }
}
