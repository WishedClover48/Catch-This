using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public abstract class GodAttack : MonoBehaviourPunCallbacks
{
    public float Cooldown => cooldown;
    public bool IsOnCooldown => OnCooldown;

    public Action OnCooldownStart;
    public Action OnCooldownEnd;
    protected bool OnCooldown;
    
    private Camera _mainCamera;

    [Header("Config")] 
    [SerializeField] public Sprite logo;
    [SerializeField] protected float cooldown;
    [SerializeField] private LayerMask clickableMask;

    protected  virtual void Start()
    {
        if (!photonView.IsMine) return;

        _mainCamera = PlayerCamera.Camera;
    }
    protected IEnumerator CooldownRoutine()
    {
        OnCooldown = true;
        OnCooldownStart?.Invoke();
        
        yield return new WaitForSeconds(cooldown);
        
        OnCooldown = false;
        OnCooldownEnd?.Invoke();
    }
    protected bool GetClickPosition(out Vector3 worldPoint)
    {
        worldPoint = default;
        var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out var hit, Mathf.Infinity, clickableMask)) return false;
        worldPoint = hit.point;
        return true;
    }
}
