
using Photon.Pun;
using Photon.Realtime;
using System.Diagnostics;
using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun.UtilityScripts;

public class RoundsManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private float _roundDuration = 60f;
    private float _roundTimer = 0f;
    private bool _roundActive = false;
    public float RoundDuration => _roundDuration;
    
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            StartRound();
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

    void EndRound()
    {
        _roundActive = false;
        UnityEngine.Debug.Log("Round finished");
        RecalculateAlivePlayers();
        // Notify everyone that the round ended
        photonView.RPC("OnRoundEnd", RpcTarget.All);
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

    [PunRPC]
    void OnRoundEnd()
    {
        // PhotonNetwork.LoadLevel("Lobby");
        // Add local reaction to round end
    }
}