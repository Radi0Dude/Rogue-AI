using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SignManager : MonoBehaviour
{
    [SerializeField] private List<Sign> signs;
    [SerializeField] private SignData signData;
    
    private int _numberOfSigns = 3;

    private void Start()
    {
        InitiateSigns();
    }

    public void InitiateSigns()
    {
        for (int i = 0; i < _numberOfSigns; i++)
        {
            signs[i].enabled = true;
            
            // Create and Send list of rooms
            signs[i].Init(signData.GetRandomRooms(_numberOfSigns));
        }
    }
}
