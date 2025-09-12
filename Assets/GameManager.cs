using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance { get; private set; }

    [field: SerializeField] public GameObject PlayerPrefab { get; private set; }
    [field: SerializeField] public Transform[] SpawnPoints { get; private set; }
    
    public Dictionary< Player, GameObject> AllPlayers = new Dictionary<Player, GameObject>();

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
        SpawnPlayers();
    }

    public void SpawnPlayers()
    {
        if (PlayerPrefab == null)
        {
            Debug.LogError("Player Prefab is not assigned in GameManager!");
            return;
        }

        Transform spawnPoint = SpawnPoints[Random.Range(0, SpawnPoints.Length)];
        var go = PhotonNetwork.Instantiate(PlayerPrefab.name, spawnPoint.position, Quaternion.identity);
    }

    private void ClearDictionary(Player player)
    {
        //Hay que llamarla al final de cada ronda.
        AllPlayers.Clear();
    }

    [PunRPC]
    public void AddPlayer(Player player, GameObject gameObject)
    {
        //Al principio de cada Ronda.
        AllPlayers.Add(player, gameObject);
    }



}