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
    [SerializeField] private NetworkManager networkManager;
    bool startedCleaning = false;
    bool finishedCleaning = false;

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
         foreach (Transform child in roomListParent)
         {
             //Destroy(child.gameObject);
         }
         Debug.Log("Amount of rooms: " + roomList.Count);
         foreach (RoomInfo room in roomList)
         {
             if (room.RemovedFromList || room.PlayerCount >= room.MaxPlayers)
                 continue;

             //Emprolijar
             GameObject button = Instantiate(roomButtonPrefab, roomListParent);
             button.GetComponentInChildren<TextMeshProUGUI>().text = $"{room.Name} ({room.PlayerCount}/{room.MaxPlayers})";
             button.GetComponent<Button>().onClick.AddListener(() => networkManager.JoinARoom(room.Name));
         } 
    }
}
