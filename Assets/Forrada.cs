using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Forrada : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject PlayerPrefab;
    [SerializeField] Vector3 spawnPoint;
    private void Start()
    {
        SpawnPlayer();
    }
    private void SpawnPlayer()
    {
        if (PlayerPrefab == null)
        {
            Debug.LogError("Player Prefab is not assigned in GameManager!");
            return;
        }
        else
        {
            var pasant = PhotonNetwork.Instantiate(PlayerPrefab.name, spawnPoint, Quaternion.identity);
        }
    }
}
