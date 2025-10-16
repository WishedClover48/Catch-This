using Photon.Pun;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    int fill;

    void Start()
    {
        SpawnPlayer();
    }
    void SpawnPlayer()
    {
        Vector2 pos = new Vector2();
        switch (PhotonNetwork.LocalPlayer.ActorNumber)
        {
            case 1:
                pos = new Vector2(10, 0.5f);
                break;
            case 2:
                pos = new Vector2(-10, 0.5f);
                break;
            case 3:
                pos = new Vector2(6, 0.5f);
                break;
            case 4:
                pos = new Vector2(-6, 0.5f);
                break;
            default:
                break;
        }

        PhotonNetwork.Instantiate("PongPlayer", pos, Quaternion.identity);

    }
}
