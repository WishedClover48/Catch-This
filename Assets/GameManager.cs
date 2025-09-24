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
    [field: SerializeField] public Transform[] SpawnPoints { get; private set; }

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
            Transform spawnPoint = SpawnPoints[Random.Range(0, SpawnPoints.Length)];
            PhotonNetwork.Instantiate(PlayerPrefab.name, spawnPoint.position, Quaternion.identity);
        }
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