using UnityEngine;
using Photon.Pun;

public class ObstacleSpawner : MonoBehaviourPun
{
    public GameObject obstaclePrefab;
    public Vector2 areaSize = new Vector2(100f, 100f); // tamaño del mapa idk
    public int obstacleCount = 10;

    void Start()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        for (int i = 0; i < obstacleCount; i++)
        {
            Vector3 pos = new Vector3(
                Random.Range(-areaSize.x / 2f, areaSize.x / 2f),
                0f,
                Random.Range(-areaSize.y / 2f, areaSize.y / 2f)
            );

                PhotonNetwork.InstantiateRoomObject("Obstacle", pos, Quaternion.identity);
        }
    }
}