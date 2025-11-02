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

        if (UnityEngine.Input.GetKeyDown(input) && !OnCooldown)
        {
            if(GetClickPosition(out var mPos))
            {
                _laserManager.Activate(mPos);
            }
        }

        if (UnityEngine.Input.GetKey(input) && !OnCooldown)
        {
            if (GetClickPosition(out var mPos))
            {
                _laserManager.UpdatePosition(mPos);
            }
        }

        if (UnityEngine.Input.GetKeyUp(input))
        {
            _laserManager.Stop();
            StartCoroutine(CooldownRoutine());
        }
    }

}
