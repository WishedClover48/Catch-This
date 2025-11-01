using System;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEditor;
using UnityEngine;

public class Bullet : MonoBehaviourPun
{
    [SerializeField] private string source;
    [SerializeField] private float speed;
    [SerializeField] private LayerMask Mask;
    [SerializeField] private LayerMask PlayerMask;
    [SerializeField] private GameObject owner;
    [SerializeField] private float lifeTime;
    [SerializeField] private Player ownerID;

    private void Start()
    {
        if (photonView.IsMine)
            StartCoroutine(Life());
    }

    void Update()
    {
        transform.position += transform.forward * (speed * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider collision)
    {
        if(!photonView.IsMine) return;
        if (InMask(Mask, collision.gameObject.layer)&&collision.gameObject!=owner)
        {
            if (InMask(PlayerMask, collision.gameObject.layer))
            {
                PhotonView pv = collision.GetComponent<PhotonView>();
                if (pv != null)
                {
                    pv.RPC("KillPlayer", pv.Owner);
                    PhotonNetwork.LocalPlayer.AddScore(1);
                }
            }            
            PhotonNetwork.Destroy(gameObject);
        }
    }

    IEnumerator Life()
    {
        yield return new WaitForSeconds(lifeTime);
        PhotonNetwork.Destroy(gameObject);
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
