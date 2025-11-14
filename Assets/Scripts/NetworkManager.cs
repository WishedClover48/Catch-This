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
    [SerializeField] private PlayerNameHelper nameHelper;

    void Start()
    {
        Debug.Log("Connecting to Photon...");
        PhotonNetwork.ConnectUsingSettings(); // Connect to Photon server
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
            Debug.LogWarning("The player does not have a nickname.");
            return;
        }
        var roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 16;
        roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable { { "gameStarted", false } };
        roomOptions.CustomRoomPropertiesForLobby = new string[] { "gameStarted" };
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
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
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("gameStarted", out object started) && (bool)started)
        {
            Debug.Log("Game already started...");
            if (gameStartedPanel != null)
            {
                gameStartedPanel.SetActive(true);
            }
            else
            {
                SceneManager.LoadScene("MainMenu");
            }
            return;
        }
        nameHelper.StartSettingName(PhotonNetwork.NickName);

        Debug.Log("Player '" + PhotonNetwork.NickName + "' joined the room!");
        PhotonNetwork.LoadLevel("Lobby");
    }
}