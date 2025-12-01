
using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.Collections.Generic;
using Photon.Pun.UtilityScripts;
using System.Collections;
using Unity.Services.Analytics;

public class RoundsManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private float _roundDuration = 60f;
    private float _roundTimer = 0f;
    private bool _roundActive = false;
    private int OldPoint;
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
        ID.IncrementRound();
    }

    public void EndRound()
    {
        _roundActive = false;
        Debug.Log("Round finished");
        GodInfo(); //Metricas
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

    private void GodInfo()
    {
        GodAbilityUsedEvent evtUsed = new GodAbilityUsedEvent{ MatchID = ID.GetMatchID(), LaserCount = GodCounter.GetLaserCastCount(), MeteorCount = GodCounter.GetMeteorCastsCount()};
        GodAbilityKillsEvent evtKills = new GodAbilityKillsEvent{ MatchID = ID.GetMatchID(), LaserKillCount = GodCounter.GetLaserKillsCount(),  MeteorKillCount = GodCounter.GetMeteorKillsCount()};

        AnalyticsService.Instance.RecordEvent(evtUsed);
        AnalyticsService.Instance.RecordEvent(evtKills);
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
        var role=PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("GodPlayer", out object value)?"God":"Survivor";
        var points=PhotonNetwork.LocalPlayer.GetScore()-OldPoint;
        OldPoint=PhotonNetwork.LocalPlayer.GetScore();
        PlayerScoreRecorded(ID.GetMatchID(), ID.GetPlayerID(), role, points);

        PlayersManager.Instance.MarkAsAlive(PhotonNetwork.LocalPlayer);
        PhotonNetwork.LoadLevel("SampleScene");
    }
    public void PlayerScoreRecorded( int matchID,int playerID, string role,int score)
    {
        PlayerScoreRecordedEvent evt = new PlayerScoreRecordedEvent{ PlayerID = playerID, MatchID = matchID,Role = role,Score = score};
        
        AnalyticsService.Instance.RecordEvent(evt);
        
    }
}