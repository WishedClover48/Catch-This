using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class LavaLakeManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject[] lavaLakes;
    [Space]
    [SerializeField] private float spawnTimer;
    
    private int _notSpawnIndex = -1;
    private int _currentIndex = 0;
    
    private Coroutine _lavaRoutine;

    private void Awake()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        GameManager.Instance.RoundStart += StartTimer;
        
        photonView.RPC(nameof(RPC_ResetLavaLakes), RpcTarget.AllBuffered);
        
        int randomIndex = Random.Range(0, lavaLakes.Length);
        photonView.RPC(nameof(RPC_SetNotSpawnIndex), RpcTarget.AllBuffered, randomIndex);
    }

    void StartTimer()
    {
        if (_lavaRoutine != null) StopCoroutine(_lavaRoutine);

        _lavaRoutine = StartCoroutine(LavaCycleRoutine());
    }
    
    private IEnumerator LavaCycleRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnTimer);
            
            TriggerNextLava();
        }
    }

    private void TriggerNextLava()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        photonView.RPC(nameof(RPC_ActivateLavaLake), RpcTarget.All);
    }
    [PunRPC] public void RPC_ResetLavaLakes()
    {
        _currentIndex = 0;

        foreach (var lake in lavaLakes)
        {
            if (lake == null) continue;
            lake.transform.localScale = Vector3.one;
            lake.SetActive(false);
        }
    }
    [PunRPC] public void RPC_SetNotSpawnIndex(int index)
    {
        _notSpawnIndex = index;
    }
    [PunRPC] public void RPC_ActivateLavaLake()
    {
        if (_currentIndex == _notSpawnIndex) _currentIndex++;
        if (_currentIndex >= lavaLakes.Length) return;

        var lake = lavaLakes[_currentIndex];
        if (lake != null) lake.SetActive(true);

        _currentIndex++;
    }
}
