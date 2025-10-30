using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance { get; private set; }

    private Player _godSelector;

    private bool _amIGod = false;

    [field: SerializeField] public GameObject PlayerPrefab { get; private set; }
    [field: SerializeField] public GameObject GodPrefab { get; private set; }
    [field: SerializeField] public Transform[] SpawnPoints { get; private set; }
    
    [field: SerializeField] private UIManager UIManager;
    [field: SerializeField] private RoundsManager roundsManager;
    [field: SerializeField] private LeaderBoard leaderBoard;
    [field: SerializeField] public float endOfRoundTime;

    private bool RoundStarted=false;//Test

    public event Action RoundStart; 
    public event Action ToNextRound; 
    public event Action RoundEnd; 


    private void Awake()
    {
        SetReadyProperty(false);

        if (PhotonNetwork.IsMasterClient)
        {
            ChangeGod();
            photonView.RPC("AmIGod", RpcTarget.AllBuffered, _godSelector);
        }

        Instance = this;
    }

    private void Start()
    {
        UIManager.SequenceFinished += StartRound;
        roundsManager.EndRoundEvent += EndRound;
        //SpawnPlayer();
    }
    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if(!RoundStarted)
            {
                var allReady = true;
                foreach (Player p in PhotonNetwork.PlayerList)
                {
                    if (p.CustomProperties.TryGetValue("ReadyToGo", out object value))
                    {
                        if (!(bool)value)
                            allReady = false;
                        break;
                    }
                }
                if (allReady)
                {
                    Debug.Log("yes");
                    RoundStarted = true;
                    photonView.RPC("EveryoneReady", RpcTarget.AllBuffered);
                }
        }
        }

    }
    [PunRPC]
    public void EveryoneReady() 
    {
        UIManager.StartSequence();
    }

    private void StartRound()
    {
        SpawnPlayer();
        RoundStart?.Invoke();
    }

    private void EndRound()
    {
        RoundEnd?.Invoke();
        StartCoroutine(Delay());
    }

    public IEnumerator Delay()
    {
        yield return new WaitForSeconds(endOfRoundTime);
        ToNextRound?.Invoke();
    }

    private void SpawnPlayer()
    {
        if (PlayerPrefab == null)
        {
            Debug.LogError("Player Prefab is not assigned in GameManager!");
            return;
        }
        if (_amIGod)
        {
            var god = PhotonNetwork.Instantiate(GodPrefab.name, new Vector3(0, 50, -50), Quaternion.identity);
            
            SetGodPlayer(true);
        }
        else
        {
            var idx = PhotonNetwork.LocalPlayer.ActorNumber - 2; //One for the god and one for the zero start of arrays
            Vector3 spawnPoint = SpawnManager.Instance.GetSpawnPoint(idx);
            var pasant = PhotonNetwork.Instantiate(PlayerPrefab.name, spawnPoint, Quaternion.identity);
            
            SetGodPlayer(false);
        }
    }

    private void SetGodPlayer(bool isGod)
    {
        ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
        playerProperties["GodPlayer"] = isGod;
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
    }

    private void SetReadyProperty(bool value) 
    {
        ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
        playerProperties["ReadyToGo"] = value;
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
    }


   // public bool IsGodPlayer()
   // {
   //     if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("GodPlayer", out object value))
   //     {
   //         return (bool)value;
   //     }
   //     return false;
   // }


    [PunRPC]
    public void SetActive(int viewID, bool isActive)
    {
        PhotonView pv = PhotonView.Find(viewID);
        if (pv != null && pv.gameObject != null)
        {
            pv.gameObject.SetActive(isActive);
        }
    }


    public void ChangeGod()
    {
        
        _godSelector = PhotonNetwork.PlayerList[Random.Range(0, PhotonNetwork.PlayerList.Length)];
        
        Debug.Log("Beep boop changing god...");
    }

    [PunRPC]
    public void AmIGod(Player god)
    {
        var player = PhotonNetwork.LocalPlayer;
        
        if (player == god)
        {
            _amIGod = true;
            photonView.RPC("SetGodNumber", RpcTarget.AllBuffered, player.ActorNumber);
            //PlayersManager.Instance.godActorNumber = player.ActorNumber;
        }
        else
        {
            _amIGod = false;
        }

    }
    [PunRPC]
    public void SetGodNumber(int number) {
        PlayersManager.Instance.godActorNumber = number;
        SetReadyProperty(true);
    }
}