using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    void Start()
    {
        Debug.Log("Connecting to Photon...");
        PhotonNetwork.ConnectUsingSettings(); // Connect to Photon server
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master Server!");
        PhotonNetwork.JoinOrCreateRoom("Room1", new RoomOptions { MaxPlayers = 4 }, TypedLobby.Default);
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby.");
        
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.NickName = "The Beast";

        Debug.Log("Player '" + PhotonNetwork.NickName + "' joined the room!");
        Debug.Log("The ID of the player is: " + PhotonNetwork.LocalPlayer.UserId);
        // Now we can spawn the player
        GameManager.Instance.SpawnPlayer();
    }
}