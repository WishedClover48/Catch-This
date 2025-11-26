using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.XR;

public class Laser : MonoBehaviourPunCallbacks
{
    public static Laser Instance { get; private set; }

    [SerializeField] private GameObject sphere;
    private Collider _col;

    [SerializeField] private float timeToActivate;
    [SerializeField] private float speed = 12f;

    private Material meshR;
    private Vector3 _targetPosition;
    private bool _isAiming;
    private Vector3 endScale;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        _col = sphere.GetComponent<Collider>();
        meshR = sphere.GetComponent<MeshRenderer>().sharedMaterial;
        endScale = sphere.transform.localScale;
    }
    private void Start()
    {
        photonView.RPC(nameof(StopRPC), RpcTarget.All);
    }
    private void Update()
    {
        if (!_isAiming) return;

        sphere.transform.position += (_targetPosition - sphere.transform.position).normalized * speed * Time.deltaTime;
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
        
        photonView.RPC(nameof(RPC_LaserUsed), RpcTarget.AllBuffered);
        photonView.RPC(nameof(ActivateRPC), RpcTarget.All, pos.x, pos.z);
    }
    public void UpdatePosition(Vector3 pos)
    {
        if (!_isAiming) return;

        photonView.RPC(nameof(SetTargetPos), RpcTarget.All, pos.x, pos.z);
    }
    public void Stop()
    {
        if (!_isAiming) return;

        photonView.RPC(nameof(StopRPC), RpcTarget.All);
    }

    private void SetAlpha(float value)
    {
        Color color = meshR.color;
        color.a = value;
        meshR.color = color;
    }

    private IEnumerator ScaleSphere()
    {
        Vector3 startScale = new Vector3(0, sphere.transform.localScale.y, 0);
        float elapsed = 0f;

        sphere.transform.localScale = startScale;

        while (elapsed < timeToActivate)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / timeToActivate);

            t = Mathf.SmoothStep(0, 1, t);

            sphere.transform.localScale = Vector3.Lerp(startScale, endScale, t);
            yield return null;
        }

        sphere.transform.localScale = endScale;

        photonView.RPC(nameof(ActivateCollider), RpcTarget.All);
    }
   
    
    [PunRPC]
    public void ActivateRPC(float x, float z)
    {
        sphere.transform.position = new Vector3(x,0,z);
        sphere.SetActive(true);
        _isAiming = true;
        StartCoroutine(ScaleSphere());
    }

    [PunRPC]
    public void ActivateCollider()
    {
        _col.enabled = true;
    }

    [PunRPC]
    public void SetTargetPos(float x, float z)
    {
        _targetPosition = new Vector3(x, 0, z);
    }

    [PunRPC]
    public void StopRPC()
    {
        sphere.transform.position = new Vector3(0,50,0);
        _col.enabled = false;
        sphere.SetActive(false);
        _isAiming = false;
    }

    [PunRPC]
    public void RPC_LaserUsed()
    {
        GodCounter.LaserUsed();
    }
}
