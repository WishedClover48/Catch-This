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
    
    [Header("Constants")]
    [SerializeField] private int y = 0;
    private Vector3Int _areaSize;
    private Vector3 _value;
    private List<Vector3> _spawnPoints = new List<Vector3>();
    public List<Vector3> SpawnPoints => _spawnPoints;
    private bool _ready;
    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        _areaSize.x = (int)(5 * floor.transform.localScale.x);
        _areaSize.y = 0;
        _areaSize.z = (int)(5 * floor.transform.localScale.z);
        
        _spawnPoints = CreateSpawnPoints();
    }
    
    private void ClearPoints()
    {
        _spawnPoints.Clear();
    }
    private List<Vector3> CreateSpawnPoints()
    {
        int n = PhotonNetwork.CurrentRoom.PlayerCount - 1;
    
        var list = new List<Vector3>(n);
    
        float rx = Mathf.Max(1f, _areaSize.x * 0.8f);
        float rz = Mathf.Max(1f, _areaSize.z * 0.8f);
    
        if (n == 1)
        {
            list.Add(new Vector3(0f, y, 0f));
            return list;
        }
    
        for (int i = 0; i < n; i++)
        {
            float t = (i / (float)n) * Mathf.PI * 2f;
            float x = Mathf.Cos(t) * rx;
            float z = Mathf.Sin(t) * rz;

            // Clamp inside rectangle in case rx/rz exceed
            x = Mathf.Clamp(x, -_areaSize.x, _areaSize.x);
            z = Mathf.Clamp(z, -_areaSize.z, _areaSize.z);

            // Round to integers, then store as Vector3 with .0
            int xi = Mathf.RoundToInt(x);
            int zi = Mathf.RoundToInt(z);

            list.Add(new Vector3(xi, y, zi));
        }

        return list;
        

    }
    
    public Vector3 GetSpawnPoint(int idx)
    {
        return _spawnPoints[idx];
    }
}
