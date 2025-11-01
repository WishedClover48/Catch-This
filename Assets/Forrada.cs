using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Forrada : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject spawnPrefab;
    [SerializeField] Vector3 spawnPoint;
    private void Start()
    {
        SpawnPlayer();
    }
    private void SpawnPlayer()
    {
        PhotonNetwork.Instantiate(spawnPrefab.name, new Vector3(0, 50, -50), Quaternion.identity);
    }
}
