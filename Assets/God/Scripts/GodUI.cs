using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GodUI : MonoBehaviour
{
    [SerializeField] private TMP_Text survivorsText;
    [Header("Wiring")]
    [SerializeField] private GameObject cooldownHolder;
    [SerializeField] private GameObject cooldownPrefab;

    public void SetUp(List<GodAttack> list)
    {
        foreach (var attack in list)
        {
            CreateCooldownObject(attack);
        }

        SetText();
    }
    private void CreateCooldownObject(GodAttack attack)
    {
        var prefab = Instantiate(cooldownPrefab, cooldownHolder.transform);
        prefab.GetComponent<CooldownController>().SetUp(attack);
    }

    private void SetText()
    {
        

        
        return;
        var maxPlayers = 10; //How many players in the round (-1 GodPlayer)
        var alivePlayers = 2; //How many players are left
        
        //A way to update alivePlayers
 
        survivorsText.text = $"Survivors: {alivePlayers}/{maxPlayers}";
    }
}
