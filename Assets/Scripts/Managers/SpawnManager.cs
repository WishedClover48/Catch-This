using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnManager : MonoBehaviourPunCallbacks
{
    public static SpawnManager Instance { get; private set; }
    [Header("Area Source")]
    [SerializeField] private GameObject floor;
    
    private int amountOfPlayers;
    
    public int _height;
    private Vector3Int _areaSize;
    private Vector3 _value;
    private List<Vector3> _spawnList = new List<Vector3>();
    private bool _ready;
    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        _areaSize.x = (int)(5 * floor.transform.localScale.x);
        _areaSize.y = 0;
        _areaSize.z = (int)(5 * floor.transform.localScale.z);
    }

    private void Start()
    {
        int n = (amountOfPlayers > 0) ? amountOfPlayers : PhotonNetwork.CurrentRoom?.PlayerCount ?? 1; //We need n - 1 positions as 1 of the players will be god
        if (PhotonNetwork.IsMasterClient)
        {
            var points = CreateSpawnPoints(n);
            _spawnList.Clear();
            _spawnList.AddRange(points);
            _ready = true;
            
            float[] flat = new float[_spawnList.Count * 3];
            for (int i = 0; i < _spawnList.Count; i++)
            {
                flat[i * 3 + 0] = _spawnList[i].x;
                flat[i * 3 + 1] = _spawnList[i].y;
                flat[i * 3 + 2] = _spawnList[i].z;
            }
            
            photonView.RPC(nameof(RPC_SetSpawnList), RpcTarget.AllBuffered, flat);
        }
    }
    
    public Vector3 GetMySpawnPosition()
    {
        if (!_ready || _spawnList.Count == 0)
        {
            Debug.LogWarning("[SpawnManager] Not ready yet; defaulting to center.");
            return new Vector3(0, _height, 0);
        }
        
        int idx = ((PhotonNetwork.LocalPlayer?.ActorNumber ?? 1) - 1) % _spawnList.Count;
        return _spawnList[idx];
    }
    
    public void ClearPositions()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC(nameof(RPC_ClearPositions), RpcTarget.AllBuffered);
        }
        else
        {
            photonView.RPC(nameof(RPC_RequestClear), RpcTarget.MasterClient);
        }
    }
    
    private List<Vector3> CreateSpawnPoints(int n)
    {
        var list = new List<Vector3>(n);

        // safe radius inside rectangle (leave margin so nobody spawns outside)
        float rx = Mathf.Max(1f, _areaSize.x * 0.8f);
        float rz = Mathf.Max(1f, _areaSize.z * 0.8f);

        // If n == 1, just center
        if (n == 1)
        {
            list.Add(new Vector3(0f, _height, 0f));
            return list;
        }

        // Place on an ellipse perimeter to spread evenly
        for (int i = 0; i < n; i++)
        {
            float t = (i / (float)n) * Mathf.PI * 2f;
            float x = Mathf.Cos(t) * rx;
            float z = Mathf.Sin(t) * rz;

            // Clamp inside rectangle in case rx/rz exceed
            x = Mathf.Clamp(x, -_areaSize.x, _areaSize.x);
            z = Mathf.Clamp(z, -_areaSize.z, _areaSize.z);

            list.Add(new Vector3(x, _height, z));
        }

        // Optional: small jitter so two very close angles don’t perfectly align on rectangular edges
        System.Random rnd = new System.Random(12345); // deterministic
        for (int i = 0; i < list.Count; i++)
        {
            float jx = (float)(rnd.NextDouble() * 1.0 - 0.5); // ±0.5 units
            float jz = (float)(rnd.NextDouble() * 1.0 - 0.5);
            Vector3 p = list[i];
            p.x = Mathf.Clamp(p.x + jx, -_areaSize.x, _areaSize.x);
            p.z = Mathf.Clamp(p.z + jz, -_areaSize.z, _areaSize.z);
            list[i] = p;
        }

        return list;
    }
    
    [PunRPC]
    private void RPC_SetSpawnList(float[] flat)
    {
        _spawnList.Clear();
        for (int i = 0; i < flat.Length; i += 3)
        {
            _spawnList.Add(new Vector3(flat[i + 0], flat[i + 1], flat[i + 2]));
        }
        _ready = true;
    }

    [PunRPC]
    private void RPC_ClearPositions()
    {
        _spawnList.Clear();
        _ready = false;
    }

    [PunRPC]
    private void RPC_RequestClear()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        photonView.RPC(nameof(RPC_ClearPositions), RpcTarget.AllBuffered);
    }
}
