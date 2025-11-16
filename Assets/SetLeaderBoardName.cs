using System.Collections;
using System.Collections.Generic;
using LootLocker.Requests;
using TMPro;
using UnityEngine;

public class SetLeaderBoardName : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Name;
    public void SetName()
    {
        if(Name.text == "") return;
        SetPlayerName(Name.text);
    }
    public void SetPlayerName(string name)
    {
        
        LootLockerSDKManager.SetPlayerName(name, resp =>
        {
            if (!resp.success) Debug.LogError("Fallo nombre");
            else {
                Debug.Log("Se puso el nombre");
            }
        });
    }
}
