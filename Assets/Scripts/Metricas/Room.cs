using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

public class Room : MonoBehaviour
{
    private void Awake()
    {
        int number = 0;
        string raw = PhotonNetwork.CurrentRoom.Name;

        foreach (char c in raw)
        {
            number = (number * 31 + c) % 10000;
        }
        
        ID.Initialize(number);
    }
}
