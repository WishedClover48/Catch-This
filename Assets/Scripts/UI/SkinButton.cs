using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinButton : MonoBehaviourPunCallbacks
{
    [SerializeField] private Skins selectedSkin;

    public void SelectSkin()
    {
        ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
        playerProperties["Skin"] = selectedSkin;
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
    }

    //Testing
    [ContextMenu("ShowSelectedSkin")]
    public void GetPlayerSkin()
    {
        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue<object>("Skin", out object value);
        Debug.Log("Player skin is " + (Skins)value);

    }
}

public enum Skins
{
    Bepi = 0,
    Granny = 1,
    Viking = 2,
    Mouse = 3,
}