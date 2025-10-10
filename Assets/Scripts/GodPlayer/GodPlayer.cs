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
    [SerializeField] private int cooldown;
    private Meteor _meteorScript;

    private LaserManager laser;

    [Header("Config")]
    [SerializeField] private LayerMask clickableMask;
    
    private event Action<Vector3> OnPrimaryAction;
    private bool onCooldown;

    private void Start()
    {
        if (!photonView.IsMine) return;
        
        CreateCamera();
        
        OnPrimaryAction += Attack;

        laser = LaserManager.Instance;
    }

    void Update()
    {
        if (!photonView.IsMine) return;
        
        if (!onCooldown && Input.GetMouseButtonDown(0) && GetClickPosition(out var clickPos))
        {
            OnPrimaryAction?.Invoke(clickPos);
        }

        if (Input.GetMouseButtonDown(1))
        {
            if(GetClickPosition(out var mPos))
            {
                laser.Activate(mPos);
            }
        }

        if (Input.GetMouseButton(1))
        {
            if (GetClickPosition(out var mPos))
            {
                laser.UpdatePosition(mPos);
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            laser.Stop();
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
    
    private void Attack(Vector3 clickPos)
    {
        var meteor = PhotonNetwork.Instantiate(mainAttackPrefab.name, new Vector3(0, 50, 0), Quaternion.identity);
        
        _meteorScript = meteor.GetComponent<Meteor>();
        
        _meteorScript.Shoot(clickPos);
        
        onCooldown = true;
        StartCoroutine(StartCooldown());
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

    private IEnumerator StartCooldown()
    {
        yield return new WaitForSeconds(cooldown);
        onCooldown = false;
    }

}
