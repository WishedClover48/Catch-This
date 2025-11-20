
using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.Collections.Generic;
using Photon.Pun.UtilityScripts;
using System.Collections;

public class RoundsManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private float _roundDuration = 60f;
    private float _roundTimer = 0f;
    private bool _roundActive = false;
    public float RoundDuration => _roundDuration;
    public event Action EndRoundEvent; 
    
    void Start()
    {
        GameManager.Instance.RoundStart += StartRound;
        GameManager.Instance.ToNextRound += RoundEnd;
        if (PhotonNetwork.IsMasterClient)
        {
            
            //StartRound();
        }
    }

    void Update()
    {
        if (!PhotonNetwork.IsMasterClient || !_roundActive)
            return;

        _roundTimer -= Time.deltaTime;

        if (_roundTimer <= 0f || PlayersManager.Instance.CountAlivePlayers() == 0)
        {
            EndRound();
        }
    }
    
    void StartRound()
    {
        _roundTimer = _roundDuration;
        _roundActive = true;
    }

    public void EndRound()
    {
        _roundActive = false;
        Debug.Log("Round finished");
        RecalculateAlivePlayers();
        EndRoundEvent?.Invoke();
        // Notify everyone that the round ended
        //photonView.RPC("OnRoundEnd", RpcTarget.All);
    }
    private void RecalculateAlivePlayers()
    {
        List<Player> alivePlayers  = new List<Player>();
        int alive = 0;
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            if (p.CustomProperties.TryGetValue("IsDead", out object isDeadObj))
                if (!(bool)isDeadObj)
                    alivePlayers .Add(p);
                else
                    alive++;
        }
        int totalPlayers = PhotonNetwork.CountOfPlayersInRooms;
        int pointsToDistribute = totalPlayers * 2 - (totalPlayers - alivePlayers.Count);

        foreach (Player player in alivePlayers)
        {
            int points = pointsToDistribute / alivePlayers.Count;
            player.AddScore(points);
        }
    }

    private void RoundEnd()
    {
        if (RoundData.Instance.roundsPassed >= Mathf.Clamp(PhotonNetwork.PlayerList.Length * 3, 4, 20) || 
            PhotonNetwork.PlayerList.Length == 1)
        { 
            photonView.RPC("OnMatchFinished", RpcTarget.All);
        }
        else
        {
            photonView.RPC("OnRoundEnd", RpcTarget.All);
        }
    }

    [PunRPC]
    void OnMatchFinished()
    {
        LeaderboardService.SubmitScore(PhotonNetwork.LocalPlayer.GetScore(),"gloabalmaxpoints", success =>
        {
            Debug.Log("Submit done: " + success);
        });
        RoundData.Instance.ResetRounds();
        PlayersManager.Instance.MarkAsAlive(PhotonNetwork.LocalPlayer);
        PhotonNetwork.LoadLevel("Lobby");
    }
    
    [PunRPC]
    void OnRoundEnd()
    {
        PlayersManager.Instance.MarkAsAlive(PhotonNetwork.LocalPlayer);
        PhotonNetwork.LoadLevel("SampleScene");
    }
}