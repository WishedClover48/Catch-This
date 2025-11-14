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
    public void StartSettingName(string name)
    {
        StartCoroutine(SetName(name));
    }
    public IEnumerator SetName(string name)
    {
        //while (!FinishedLoading)
        //{
            SetPlayerName(name);
            yield return new WaitForSeconds(1f);
        //}
    }
}
