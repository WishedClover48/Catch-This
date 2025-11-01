using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class LaserAttack : GodAttack
{
    private Laser _laserManager;
    
    protected override void Start()
    {
        base.Start();
        
        if (!photonView.IsMine) return;
        _laserManager = Laser.Instance;
    }
    private void Update()
    {
        if(!photonView.IsMine) return;

        if (Input.GetMouseButtonDown(1) && !OnCooldown)
        {
            if(GetClickPosition(out var mPos))
            {
                _laserManager.Activate(mPos);
            }
        }

        if (Input.GetMouseButton(1) && !OnCooldown)
        {
            if (GetClickPosition(out var mPos))
            {
                _laserManager.UpdatePosition(mPos);
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            _laserManager.Stop();
            StartCoroutine(CooldownRoutine());
        }
    }

}
