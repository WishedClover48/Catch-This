using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BulletMovement : MonoBehaviourPun
{
    [SerializeField] private string source;
    [SerializeField] private float speed;
    [SerializeField] private LayerMask Mask;
    void Update()
    {
        transform.position += transform.forward * (speed * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (LayerMask.Equals(Mask, collision.gameObject.layer))
        {
            if (collision.gameObject.layer == LayerMask.GetMask("Player"))
            {
                PlayerMovement playerHitted = collision.GetComponent<PlayerMovement>();
                playerHitted.Pv.RPC("KillPlayer", playerHitted.Pv.Owner);
            }            
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
