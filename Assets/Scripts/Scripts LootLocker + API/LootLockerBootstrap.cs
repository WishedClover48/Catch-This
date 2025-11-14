using LootLocker.Requests;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootLockerBootstrap : MonoBehaviour
{
    public static bool SessionStarted {  get; private set; }

    string playerIdentifier = DateTime.Now.ToString();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        StartGuest();
    }

    void StartGuest()
    {
        LootLockerSDKManager.StartGuestSession(playerIdentifier, response =>
        {
            if (!response.success)
            {
                Debug.LogError("Fallo");
                return;
            }
            SessionStarted = true;
            Debug.Log("Conectado a LootLocker");
            PlayerPrefs.SetString("PlayerID", playerIdentifier);
        });
    }
}
