using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField] Button LobbyButton;
    [SerializeField] private GameObject gameStartedPanel;
    [SerializeField] private Image connectedImage;
    
    private Dictionary<string, RoomInfo> roomCache = new Dictionary<string, RoomInfo>();

    void Start()
    {
        Debug.Log("Connecting to Photon...");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        connectedImage.color = Color.green;
        Debug.Log("Connected to Master Server!");
        PhotonNetwork.JoinLobby();
    }

    public void JoinARoom(string roomName)
    {
        if (PhotonNetwork.NickName == string.Empty)
        {
            Debug.Log("The player does not have a nickname.");
            return;
        }
        
        if (roomCache.TryGetValue(roomName, out var roomInfo))
        {
            bool gameStarted = false;

            if (roomInfo.CustomProperties != null && roomInfo.CustomProperties.TryGetValue("gameStarted", out var rawValue) && rawValue is bool startedBool)
            {
                gameStarted = startedBool;
            }

            if (gameStarted)
            {
                gameStartedPanel.SetActive(true);
            }
            else
            {
                PhotonNetwork.JoinRoom(roomName);
            }
        }
        else
        {
            var roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 16;
            roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable { { "gameStarted", false } };
            roomOptions.CustomRoomPropertiesForLobby = new[] { "gameStarted" };

            PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
        }
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby.");
        if (LobbyButton != null)
        {
            LobbyButton.interactable = true;
        }
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Player '" + PhotonNetwork.NickName + "' joined the room!");
        PhotonNetwork.LoadLevel("Lobby");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (var room in roomList)
        {
            if (room.RemovedFromList)
            {
                roomCache.Remove(room.Name);
            }
            else
            {
                roomCache[room.Name] = room;
            }
        }
    }
}