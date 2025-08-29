using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField] Button LobbyButton;
    void Start()
    {
        Debug.Log("Connecting to Photon...");
        PhotonNetwork.ConnectUsingSettings(); // Connect to Photon server
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master Server!");
        if (LobbyButton != null)
        {
            LobbyButton.interactable = true;
        }
    }

    public void JoinARoom(string roomName)
    {
        PhotonNetwork.JoinOrCreateRoom( roomName, new RoomOptions { MaxPlayers = 16 }, TypedLobby.Default);
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby.");
        
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Player '" + PhotonNetwork.NickName + "' joined the room!");
        Debug.Log("The ID of the player is: " + PhotonNetwork.LocalPlayer.UserId);
        PhotonNetwork.LoadLevel("Lobby");
    }
}