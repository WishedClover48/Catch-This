using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class MeteorAttack : GodAttack
{
    private Meteor _meteorScript;
    
    [Header("Prefab")]
    [SerializeField] private GameObject meteorPrefab;
    
    void Update()
    {
        if (!photonView.IsMine) return;
        
        if (!OnCooldown && UnityEngine.Input.GetKeyDown(input) && GetClickPosition(out var clickPos))
        {
            Attack(clickPos);
        }
    }
    private void Attack(Vector3 clickPos)
    {
        var meteor = PhotonNetwork.Instantiate(meteorPrefab.name, new Vector3(0, 50, 0), Quaternion.identity);
        
        _meteorScript = meteor.GetComponent<Meteor>();
        
        _meteorScript.Shoot(clickPos);
        
        StartCoroutine(CooldownRoutine());
    }
}
