using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System;

public class RoomListUI : MonoBehaviourPunCallbacks
{
    public GameObject roomButtonPrefab;
    public Transform roomListParent;
    [SerializeField] private RoomJoin connectButton;

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
         foreach (RoomInfo room in roomList)
         {
             if (room.RemovedFromList || room.PlayerCount >= room.MaxPlayers)
                 continue;

             //Emprolijar
             GameObject button = Instantiate(roomButtonPrefab, roomListParent);
             button.GetComponentInChildren<TextMeshProUGUI>().text = $"{room.Name} ({room.PlayerCount}/{room.MaxPlayers})";
             connectButton.roomName.text = room.Name;
             button.GetComponent<Button>().onClick.AddListener(() => connectButton.ConnectToRoom());
         } 
    }
}
