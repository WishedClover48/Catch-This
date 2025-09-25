using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;

public class ObstacleSpawner : MonoBehaviourPun
{
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private Vector2 areaSize = new Vector2(80f, 80f);
    [SerializeField] private int obstacleCount;
    [SerializeField] private float minDistance; // Distancia mínima entre obstáculos

    private List<Vector3> spawnedPositions = new List<Vector3>();

    void Start()
    {
        GameManager.Instance.RoundStart += SpawnObstacle;
    }

    private void SpawnObstacle()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        int spawned = 0;
        int attempts = 0;
        int maxAttempts = obstacleCount * 10;

        while (spawned < obstacleCount && attempts < maxAttempts)
        {
            Vector3 candidatePos = new Vector3(
                Random.Range(-areaSize.x / 2f, areaSize.x / 2f),
                0f,
                Random.Range(-areaSize.y / 2f, areaSize.y / 2f)
            );

            bool tooClose = false;
            foreach (var pos in spawnedPositions)
            {
                if (Vector3.Distance(candidatePos, pos) < minDistance)
                {
                    tooClose = true;
                    break;
                }
            }

            if (tooClose)
            {
                attempts++;
                continue;
            }

            PhotonNetwork.InstantiateRoomObject("Obstacle", candidatePos, Quaternion.identity);
            spawnedPositions.Add(candidatePos);
            spawned++;
        }

        Debug.Log($"[ObstacleSpawner] Spawned {spawned} obstacles after {attempts} attempts.");
    }
}