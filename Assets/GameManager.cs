using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    //public static GameManager Instance;

    public GameObject playerPrefab;
    public Transform[] spawnPoints;

    void Awake()
    {
        // Singleton pattern
        //if (Instance == null)
        //{
        //    Instance = this;
       // }
       // else
        //{
        //    Destroy(gameObject);
        //}
    }

    private void Start()
    {
        // Now we can spawn the player
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        if (playerPrefab == null)
        {
            Debug.LogError("Player Prefab is not assigned in GameManager!");
            return;
        }

        // Pick a random spawn point (optional)
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Instantiate the player over the network
        PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, Quaternion.identity);
    }
}