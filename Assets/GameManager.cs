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

    private int _godSelector = 0;

    private bool _amIGod = false;

    [field: SerializeField] public GameObject PlayerPrefab { get; private set; }
    [field: SerializeField] public GameObject GodPrefab { get; private set; }
    [field: SerializeField] public Transform[] SpawnPoints { get; private set; }
    
    [field: SerializeField] private UIManager UIManager;
    [field: SerializeField] private RoundsManager roundsManager;
    [field: SerializeField] private LeaderBoard leaderBoard;
    [field: SerializeField] public float endOfRoundTime;
    public TextMeshProUGUI test;
    public event Action RoundStart; 
    public event Action ToNextRound; 
    public event Action RoundEnd; 


    private void Awake()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            ChangeGod();
            photonView.RPC("AmIGod", RpcTarget.AllBuffered,_godSelector);
        }

        Instance = this;
    }

    private void Start()
    {
        UIManager.StartSequence();
        UIManager.SequenceFinished += StartRound;
        roundsManager.EndRoundEvent += EndRound;
        //SpawnPlayer();
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

    public bool IsGodPlayer()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("GodPlayer", out object value))
        {
            return (bool)value;
        }
        return false;
    }


    [PunRPC]
    public void SetActive(int viewID, bool isActive)
    {
        PhotonView pv = PhotonView.Find(viewID);
        if (pv != null && pv.gameObject != null)
        {
            pv.gameObject.SetActive(isActive);
        }
    }


    [PunRPC]
    public void ChangeGod()
    {
        _godSelector = Random.Range(1, PhotonNetwork.PlayerList.Length+1);
        test.text = _godSelector.ToString();
        Debug.Log("Beep boop changing god...");
    }

    [PunRPC]
    public void AmIGod(int god)
    {
        var player = PhotonNetwork.LocalPlayer;
        
        if (player == PhotonNetwork.PlayerList[god])
        {
            _amIGod = true;
            photonView.RPC("SetGodNumber", RpcTarget.AllBuffered, player.ActorNumber);
            PlayersManager.Instance.godActorNumber = player.ActorNumber;
        }
        else
        {
            _amIGod = false;
        }
        Debug.Log(god);
    }
    [PunRPC]
    public void SetGodNumber(int number) {
        PlayersManager.Instance.godActorNumber = number;
    }
}