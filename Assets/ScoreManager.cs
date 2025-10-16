using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private TextMeshProUGUI Score1Text;
    [SerializeField] private TextMeshProUGUI Score2Text;
    private int Team1Score=0;
    private int Team2Score=0;
    void Start()
    {
        Debug.Log("Connecting to Photon...");
        PhotonNetwork.ConnectUsingSettings(); // Connect to Photon server
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master Server!");
        PhotonNetwork.JoinLobby();
    }
    [ContextMenu("joinRoom")]
    public override void OnJoinedLobby()
    {
        PhotonNetwork.JoinRandomOrCreateRoom();
    }
    public void AddScore(int team1 ,int team2)
    {
        Team1Score += team1;
        Team2Score += team2;
        Debug.Log(Team2Score + "  " + Team1Score);
        photonView.RPC("UpdateScoreText", RpcTarget.AllBuffered, Team1Score, Team2Score);
    }
    [PunRPC]
    public void UpdateScoreText(int Score1, int Score2)
    {
        Score1Text.text = Score1.ToString();
        Score2Text.text = Score2.ToString();
    }
}
