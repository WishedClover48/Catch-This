
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class RoundsManager : MonoBehaviourPunCallbacks
{
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

        if (roundTimer <= 0f || PlayersManager.Instance.CountAlivePlayers() == 0)
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
        Debug.Log("Round finished");

        // Notify everyone that the round ended
        photonView.RPC("OnRoundEnd", RpcTarget.All);
    }

    [PunRPC]
    void OnRoundEnd()
    {
        // Add local reaction to round end
    }
}