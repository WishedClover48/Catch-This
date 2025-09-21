using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GodPlayer : MonoBehaviourPunCallbacks
{
    private Camera _mainCamera;
    
    [Header("Prefabs")]
    [SerializeField] private GameObject mainAttackPrefab;
    private MeteorStrike _meteorScript;
    [SerializeField] private GameObject secondaryAttackPrefab;
    
    [Header("Mask")]
    [SerializeField] private LayerMask clickableMask;
    
    
    private event Action<Vector3> OnPrimaryAction;
    private event Action<Vector3> OnSecondaryAction;

    private void Start()
    {
        if (!photonView.IsMine) return;
        
        CreateCamera();

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
        var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
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
    
    private void CreateCamera()
    {
        GameObject cameraObject = new GameObject("MyCamera");
        
        Camera cam = cameraObject.AddComponent<Camera>();
        
        cam.orthographic = false;
        cam.fieldOfView = 60;
        
        cameraObject.transform.parent = transform;
        cameraObject.transform.rotation = Quaternion.Euler(50f, 0f, 0f);
        cameraObject.transform.position = transform.position;

        _mainCamera = cam;
    }

}
