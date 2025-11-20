using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using WebSocketSharp;

public class RoomJoin : MonoBehaviour
{
    [SerializeField] public TMP_InputField roomName;
    [SerializeField] TMP_InputField playerName;
    [SerializeField] NetworkManager networkManager;
    
    public void ConnectToRoom()
    {
        PhotonNetwork.NickName = playerName.text;
        if (roomName.text.IsNullOrEmpty())
            return;
        networkManager.JoinARoom(roomName.text);
    }

    public void ConnectToRoom(string roomToJoin)
    {
        PhotonNetwork.NickName = playerName.text;
        networkManager.JoinARoom(roomToJoin);
    }
}
