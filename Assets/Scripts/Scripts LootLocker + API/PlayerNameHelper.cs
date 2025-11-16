using LootLocker.Requests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNameHelper : MonoBehaviour
{
    bool FinishedLoading = false;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    public void SetPlayerName(string name)
    {
        
        LootLockerSDKManager.SetPlayerName(name, resp =>
        {
            if (!resp.success) Debug.LogError("Fallo nombre");
            else {
                Debug.Log("Se puso el nombre");
                FinishedLoading = true;
            }
        });
    }
}
