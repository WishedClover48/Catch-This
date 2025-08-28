using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomJoin : MonoBehaviour
{
    [SerializeField] TMP_InputField roomName;
    [SerializeField] TMP_InputField playerName;
    [SerializeField] NetworkManager networkManager;
    
    public void ConnectToRoom()
    {
        PhotonNetwork.NickName = playerName.text;
        networkManager.JoinARoom(roomName.text);
    }
}
