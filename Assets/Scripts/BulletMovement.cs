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
    [SerializeField] private GameObject owner;
    [SerializeField] private Player ownerID;
    void Update()
    {
        transform.position += transform.forward * (speed * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (InMask(Mask, collision.gameObject.layer)&&collision.gameObject!=owner)
        {
            if (collision.gameObject.layer == LayerMask.GetMask("Player"))
            {
                //PlayerMovement playerHitted = collision.GetComponent<PlayerMovement>();
                //playerHitted.Pv.RPC("KillPlayer", playerHitted.Pv.Owner);
                PhotonView pv = collision.GetComponent<PhotonView>();
                if (pv != null)
                {
                    pv.RPC("KillPlayer", RpcTarget.All);
                }
            }            
            //PhotonNetwork.Destroy(gameObject);
        }
    }

    public void SetUpOwner(GameObject ownerGameObject, Player player)
    {
        owner=ownerGameObject;
        ownerID=player;
    }
    private static bool InMask(int layer, LayerMask mask)
    {
        return (mask.value & (1 << layer)) != 0;
    }
}
