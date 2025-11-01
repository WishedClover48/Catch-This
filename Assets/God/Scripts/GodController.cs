using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GodController : MonoBehaviourPunCallbacks
{
    [Header("Camera settings")] 
    [SerializeField] private int fieldOfView;
    [SerializeField] private Quaternion rotation;

    [Header("UI settings")] 
    [SerializeField] private GameObject uiPrefab;
    [SerializeField] private List<GodAttack> attacksList;

    private void Awake()
    {
        if (!photonView.IsMine) return;
        SetUpCamera();
        SetUpUi();
    }

    private void SetUpCamera()
    {
        PlayerCamera.CreateCamera(transform, fieldOfView, rotation); 
    }

    private void SetUpUi()
    {
        var ui = Instantiate(uiPrefab, transform);
        ui.GetComponent<GodUI>().SetUp(attacksList);
    }
}
