using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    void Update()
    {
        transform.position += Vector3.forward * speed * Time.deltaTime;
    }
    private void OnTriggerEnter(Collider collision)
    {
        if(collision.tag=="Player")
        {
            //collision.gameObject.SetActive(false);
            collision.gameObject.GetComponent<PlayerMovement>().Killed();
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
