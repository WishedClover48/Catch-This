using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
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
                pv.RPC("KillPlayer", pv.Owner);
                //PhotonNetwork.LocalPlayer.AddScore(1);
            }
        }            
    }
    
    private static bool InMask( LayerMask mask,int layer)
    {
        return (mask.value & (1 << layer)) != 0;
    }
}
