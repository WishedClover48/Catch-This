using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [field: SerializeField] public GameObject PlayerPrefab { get; private set; }
    [field: SerializeField] public Transform[] SpawnPoints { get; private set; }
    
    public List<GameObject> AllPlayers = new();

    private void Awake()
    {
        // Singleton guard
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        if (PlayerPrefab == null)
        {
            Debug.LogError("Player Prefab is not assigned in GameManager!");
            return;
        }

        Transform spawnPoint = SpawnPoints[Random.Range(0, SpawnPoints.Length)];
        var go = PhotonNetwork.Instantiate(PlayerPrefab.name, spawnPoint.position, Quaternion.identity);
        //AllPlayers.Add(go);
    }

    public void AddPlayer(GameObject player)
    {
        AllPlayers.Add(player);
    }
}