using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersManager : MonoBehaviour 
{
    public static PlayersManager Instance;
    [SerializeField] private Vector3 DeadCamaraPosition;
    [SerializeField] private Vector3 DeadCameraRotation;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    public void MarkAsDead()
    {
        var props = new ExitGames.Client.Photon.Hashtable
        {
            { "IsDead", true }
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }

    public void MarkAsAlive()
    {
        var props = new ExitGames.Client.Photon.Hashtable
        {
            { "IsDead", false }
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }

    public int CountAlivePlayers()
    {
        int alive = 0;
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            if (p.CustomProperties.TryGetValue("IsDead", out object IsDead))
            {
                if (!(bool)IsDead)
                    alive++;
            }
            else
            {
                // if a player doesn't happen to have the property, it means it is alive (shouldn't happen).
                alive++; 
            }
        }
        return alive;
    }
    public void SetCamaraOnDeath(Camera camara)
    {
        camara.transform.position = DeadCamaraPosition;
        camara.transform.rotation = Quaternion.Euler(DeadCameraRotation);
    }
}
