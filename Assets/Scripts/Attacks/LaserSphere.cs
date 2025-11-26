using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;

public class LaserSphere : MonoBehaviourPunCallbacks
{
    [SerializeField] private LayerMask playerMask;

    private void OnTriggerEnter(Collider collision)
    {
        if(!photonView.IsMine) return;
        
        if (InMask(playerMask, collision.gameObject.layer))
        {
            PhotonView pv = collision.GetComponent<PhotonView>();
            if (pv != null)
            {
                PhotonNetwork.LocalPlayer.AddScore(1);
                pv.RPC("RPC_MeteorKill", RpcTarget.AllBuffered);
                pv.RPC("KillPlayer", pv.Owner);
            }
        }            
    }
    
    private static bool InMask( LayerMask mask,int layer)
    {
        return (mask.value & (1 << layer)) != 0;
    }

    [PunRPC]
    public void RPC_LaserKill()
    {
        GodCounter.LaserKill();
    }
}
