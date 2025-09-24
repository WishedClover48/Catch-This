using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEditor;
using UnityEngine;

public class BulletMovement : MonoBehaviourPun
{
    [SerializeField] private string source;
    [SerializeField] private float speed;
    [SerializeField] private LayerMask Mask;
    [SerializeField] private LayerMask PlayerMask;
    [SerializeField] private GameObject owner;
    [SerializeField] private Player ownerID;
    void Update()
    {
        transform.position += transform.forward * (speed * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider collision)
    {
        if(!photonView.IsMine) return;
        Debug.Log("pre pre pre kill");
        if (InMask(Mask, collision.gameObject.layer)&&collision.gameObject!=owner)
        {
            Debug.Log("pre pre kill");
            if (InMask(PlayerMask, collision.gameObject.layer))
            {
                //PlayerMovement playerHitted = collision.GetComponent<PlayerMovement>();
                //playerHitted.Pv.RPC("KillPlayer", playerHitted.Pv.Owner);
                Debug.Log("pre kill");
                PhotonView pv = collision.GetComponent<PhotonView>();
                if (pv != null)
                {
                    pv.RPC("KillPlayer", RpcTarget.All);
                    Debug.Log("kill");
                }
            }            
            PhotonNetwork.Destroy(gameObject);
        }
    }

    public void SetUpOwner(GameObject ownerGameObject, Player player)
    {
        owner=ownerGameObject;
        ownerID=player;
    }
    private static bool InMask( LayerMask mask,int layer)
    {
        return (mask.value & (1 << layer)) != 0;
    }
}
