using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;

public class Meteor : MonoBehaviourPunCallbacks
{
    [Header("Hit ring")]
    [SerializeField] private LineRenderer ring;
    
    private bool _isActive;
    private float _t;
    private Vector3 _crashPoint;
    private Vector3 _spawnPoint;
    private readonly Collider[] _results = new Collider[20];
    
    [Header("Spawn")]
    [SerializeField] private float spawnHeight;
    [SerializeField] private float spawnRadius;
    
    [Header("Stats")]
    [SerializeField] private float fallTime;
    [SerializeField] private float hitRadius;
    
    [Header("Layers")]
    [SerializeField] private LayerMask floorMask;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private LayerMask obstacleMask;

    private void Awake()
    {
        _isActive = false;
    }

    public void Shoot(Vector3 crashPoint)
    {
        _crashPoint = crashPoint;
        Vector2 rand = UnityEngine.Random.insideUnitCircle * spawnRadius;
        _spawnPoint = new Vector3(_crashPoint.x + rand.x, spawnHeight, _crashPoint.z + rand.y);

        SetRing();
        photonView.RPC("RPC_UsedMeteor", RpcTarget.All);
        _isActive = true;
    }

    private void Update()
    {
        if (!_isActive) return;
        _t += Time.deltaTime / Mathf.Max(0.0001f, fallTime);
        _t = Mathf.Clamp01(_t);
        transform.position = Vector3.Lerp(_spawnPoint, _crashPoint, _t);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        int otherLayer = other.gameObject.layer;

        if (InMask(otherLayer, obstacleMask))
        {
            if (!PhotonNetwork.IsMasterClient) return;
            
            PhotonView pv = other.GetComponent<PhotonView>();
            if (pv != null)
            {
                PhotonNetwork.Destroy(pv);
            }
            
            photonView.RPC("RPC_Finished", PhotonNetwork.LocalPlayer);
            return;
        }
        
        if (InMask(otherLayer, floorMask))
        {
            HandleFloorImpact();
            photonView.RPC("RPC_Finished", PhotonNetwork.LocalPlayer);
            return;
        }

        if (InMask(otherLayer, playerMask))
        {
            PhotonView pv = other.GetComponent<PhotonView>();
            if (pv != null)
            {
                PhotonNetwork.LocalPlayer.AddScore(1);
                photonView.RPC("RPC_MeteorKill", RpcTarget.All);
                pv.RPC("KillPlayer", pv.Owner);
            }
        }
    }
    
    private static bool InMask(int layer, LayerMask mask)
    {
        return (mask.value & (1 << layer)) != 0;
    }
    
    private void HandleFloorImpact()
    {
        int hitCount = Physics.OverlapSphereNonAlloc(_crashPoint, hitRadius, _results);

        for (int i = 0; i < hitCount; i++)
        {
            Collider hit = _results[i];
            if (hit == null) continue;

            PhotonView pv = hit.GetComponent<PhotonView>();
            if (pv != null && pv.gameObject.layer == playerMask)
            {
                PhotonNetwork.LocalPlayer.AddScore(1);
                photonView.RPC("RPC_MeteorKill", RpcTarget.All);
                pv.RPC("KillPlayer", pv.Owner);
            }
            
            _results[i] = null;
        }
        
        ring.gameObject.SetActive(false);
    }
    private void SetRing()
    {
        ring.positionCount = 48 + 1;
        ring.widthMultiplier = 0.1f;
        
        ring.transform.position = new Vector3(_crashPoint.x, _crashPoint.y + 0.03f, _crashPoint.z);

        float step = Mathf.PI * 2f / 48;
        for (int i = 0; i <= 48; i++)
        {
            float a = i * step;
            Vector3 p = new Vector3(Mathf.Cos(a) * hitRadius, 0f, Mathf.Sin(a) * hitRadius);
            ring.SetPosition(i, ring.transform.position + p);
        }
    }
    
    [PunRPC]
    private void RPC_Finished()
    {
        _isActive = false;
        PhotonNetwork.Destroy(gameObject);
    }

    [PunRPC] public void RPC_UsedMeteor()
    {
        GodCounter.MeteorUsed();
    }
    [PunRPC] public void RPC_MeteorKill()
    {
        GodCounter.MeteorKill();
    }
}
