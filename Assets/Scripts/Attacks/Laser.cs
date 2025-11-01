using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine;
using TMPro;

public class Laser : MonoBehaviourPunCallbacks
{
    public static Laser Instance { get; private set; }

    [SerializeField] private GameObject sphere;
    private Collider _col;

    [SerializeField] private float speed = 12f;

    private Vector3 _targetPosition;
    private bool _isAiming;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }
    private void Start()
    {
        _col = sphere.GetComponent<Collider>();
    }
    private void Update()
    {
        if (!_isAiming || sphere == null) return;

        sphere.transform.position = Vector3.Lerp(sphere.transform.position, _targetPosition, Time.deltaTime * speed);
    }
    private bool IsLocalPlayerGod()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("GodPlayer", out object value))
        {
            return value is bool isGod && isGod;
        }

        return false;
    }
    
    public void Activate(Vector3 pos)
    {
        if (!IsLocalPlayerGod()) return;

        if (_isAiming) return;

        photonView.RPC(nameof(ActivateRPC), RpcTarget.All, pos.x, pos.z);
    }
    public void UpdatePosition(Vector3 worldPos)
    {
        if (!_isAiming) return;
        _targetPosition = worldPos;
    }
    public void Stop()
    {
        if (!_isAiming) return;

        photonView.RPC(nameof(StopRPC), RpcTarget.All);
        _isAiming = false;
    }
    
    [PunRPC]
    public void ActivateRPC(float x, float z)
    {
        sphere.transform.position = new Vector3(x,0,z);
        sphere.SetActive(true);
        _col.enabled = true;
        _isAiming = true;
    }
    [PunRPC]
    public void StopRPC()
    {
        sphere.SetActive(false);
        _col.enabled = false;
        _isAiming = false;
    }
}
