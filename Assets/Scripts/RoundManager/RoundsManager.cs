
using Photon.Pun;
using Photon.Realtime;
using System.Diagnostics;
using UnityEngine;
using TMPro;

public class RoundsManager : MonoBehaviourPunCallbacks
{
    [field: SerializeField] public TMP_Text Debugger { get; private set; }

    public float roundDuration = 60f;
    private float roundTimer = 0f;
    private bool roundActive = false;

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            StartRound();
        }
    }

    void Update()
    {
        if (!PhotonNetwork.IsMasterClient || !roundActive)
            return;

        roundTimer -= Time.deltaTime;

        Debugger.text = PlayersManager.Instance.CountAlivePlayers() + " alive players.";


        if (roundTimer <= 0f)
        {
            EndRound();
        }
    }

    void StartRound()
    {
        roundTimer = roundDuration;
        roundActive = true;
    }

    void EndRound()
    {
        roundActive = false;
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