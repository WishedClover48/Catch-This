using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BulletMovement : MonoBehaviourPun
{
    [SerializeField] private string source;
    [SerializeField] private float speed;
    void Update()
    {
        transform.position += Vector3.forward * (speed * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider collision)
    {
        if(collision.CompareTag("Player"))
        {
            //collision.gameObject.SetActive(false);
            PlayerMovement playerHitted = collision.GetComponent<PlayerMovement>();
            playerHitted.Pv.RPC("KillPlayer",playerHitted.Pv.Owner);
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
