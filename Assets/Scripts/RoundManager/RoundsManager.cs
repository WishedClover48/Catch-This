
using Photon.Pun;
using Photon.Realtime;
using System.Diagnostics;
using UnityEngine;
using TMPro;
using System.Collections;

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
        // Notify everyone that the round ended
        photonView.RPC("OnRoundEnd", RpcTarget.All);
    }

    [PunRPC]
    void OnRoundEnd()
    {
        PhotonNetwork.LoadLevel("Lobby");
        // Add local reaction to round end
    }
}