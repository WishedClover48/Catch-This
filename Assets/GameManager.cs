using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance { get; private set; }

    private int _godSelector = 0;

    private bool _amIGod = false;

    [field: SerializeField] public GameObject PlayerPrefab { get; private set; }
    [field: SerializeField] public GameObject GodPrefab { get; private set; }
    [field: SerializeField] public List<Vector3> SpawnPoints { get; private set; }

    private void Awake()
    {
        // Singleton guard
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient) 
        {  
            //ChangeGod();
        }

        SpawnPoints = CreateSpawnPoints();
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        if (PlayerPrefab == null)
        {
            Debug.LogError("Player Prefab is not assigned in GameManager!");
            return;
        }

        photonView.RPC("AmIGod", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer);
        
        if (_amIGod)
        {
            PhotonNetwork.Instantiate(GodPrefab.name, new Vector3(0,50,-50), Quaternion.identity);
        }
        else
        {
            var idx = PhotonNetwork.LocalPlayer.ActorNumber - 2; //One for the god and one for the zero start of arrays
            Vector3 spawnPoint = SpawnPoints[idx];
            PhotonNetwork.Instantiate(PlayerPrefab.name, spawnPoint, Quaternion.identity);
        }
    }

    private List<Vector3> CreateSpawnPoints()
    {
        int n = PhotonNetwork.CurrentRoom.PlayerCount - 1;
        
        var list = new List<Vector3>(n);
        int areaSize = 40;
        int height = 0;
        
        float rx = Mathf.Max(1f, areaSize * 0.8f);
        float rz = Mathf.Max(1f, areaSize * 0.8f);
        
        if (n == 1)
        {
            list.Add(new Vector3(0f, height, 0f));
            return list;
        }
        
        for (int i = 0; i < n; i++)
        {
            float t = (i / (float)n) * Mathf.PI * 2f;
            float x = Mathf.Cos(t) * rx;
            float z = Mathf.Sin(t) * rz;

            // Clamp inside rectangle in case rx/rz exceed
            x = Mathf.Clamp(x, -areaSize, areaSize);
            z = Mathf.Clamp(z, -areaSize, areaSize);

            // Round to integers, then store as Vector3 with .0
            int xi = Mathf.RoundToInt(x);
            int zi = Mathf.RoundToInt(z);

            list.Add(new Vector3(xi, height, zi));
        }

        return list;
    }
    
    [PunRPC]
    public void ChangeGod()
    {
        _godSelector = Random.Range(0, PhotonNetwork.PlayerList.Length);
        Debug.Log("Beep boop changing god...");
    }

    [PunRPC]
    public void AmIGod(Player player)
    {
        bool isGod = false;
        
        if (player == PhotonNetwork.PlayerList.GetValue(_godSelector))
        {
            isGod = true;
            PlayersManager.Instance.godActorNumber = PhotonNetwork.PlayerList[_godSelector].ActorNumber;
        }
        else
        {
            isGod = false;
        }
        
        PhotonView.Get(this).RPC("ReceiveGodAnswer", PhotonNetwork.CurrentRoom.GetPlayer(player.ActorNumber), isGod);
    }

    [PunRPC]
    void ReceiveGodAnswer(bool result)
    {
        _amIGod = result;
    }
}