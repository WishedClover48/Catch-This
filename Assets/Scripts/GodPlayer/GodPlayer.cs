using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GodPlayer : MonoBehaviourPunCallbacks
{
    [SerializeField] private Camera mainCamera;
    
    [SerializeField] private GameObject mainAttackPrefab;
    [SerializeField] private LayerMask clickableMask;
    
    private event Action<Vector3> OnPrimaryAction;
    private event Action<Vector3> OnSecondaryAction;

    private void Start()
    {
        if (!photonView.IsMine) return;
        
        if (mainCamera != null)
            mainCamera.enabled = true;
        
        OnPrimaryAction += pos =>
        {
            //var go = PhotonNetwork.Instantiate(mainAttackPrefab.name, pos, Quaternion.identity);
            var go = Instantiate(mainAttackPrefab);
            var ms = go.GetComponent<MeteorStrike>();
            ms.Initialize(pos, photonView.ViewID);
        };
    }

    void Update()
    {
        if (!photonView.IsMine) return;
        
        if (Input.GetMouseButtonDown(0) && GetClickPosition(out var clickPos))
        {
            OnPrimaryAction?.Invoke(clickPos);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            //OnSecondaryAction?.Invoke();
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

}
