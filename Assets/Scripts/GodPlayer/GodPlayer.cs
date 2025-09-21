using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GodPlayer : MonoBehaviourPunCallbacks
{
    [SerializeField] private Camera mainCamera;
    
    [Header("Prefabs")]
    [SerializeField] private GameObject mainAttackPrefab;
    private MeteorStrike _meteorScript;
    [SerializeField] private GameObject secondaryAttackPrefab;
    
    [Header("Mask")]
    [SerializeField] private LayerMask clickableMask;
    
    
    private event Action<Vector3> OnPrimaryAction;
    private event Action<Vector3> OnSecondaryAction;

    private void Initialize()
    {
        if (!photonView.IsMine) return;
        
        if (mainCamera != null) mainCamera.enabled = true;

        PrepareAttacks();
        
        OnPrimaryAction += pos => _meteorScript.Cast(pos);
    }

    void Update()
    {
        if (!photonView.IsMine) return;
        
        if (Input.GetMouseButtonDown(0) && GetClickPosition(out var clickPos))
        {
            OnPrimaryAction?.Invoke(clickPos);
        }
    }
    
    private bool GetClickPosition(out Vector3 worldPoint)
    {
        worldPoint = default;
        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out var hit, Mathf.Infinity, clickableMask)) return false;
        worldPoint = hit.point;
        return true;
    }

    private void PrepareAttacks()
    {
        var meteor = PhotonNetwork.Instantiate(mainAttackPrefab.name, transform.position, Quaternion.identity);
        meteor.transform.parent = this.gameObject.transform;
        _meteorScript = meteor.GetComponent<MeteorStrike>();
        _meteorScript.Initialize(photonView.ViewID);
    }

}
