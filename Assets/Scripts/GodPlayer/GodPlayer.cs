using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GodPlayer : MonoBehaviourPunCallbacks
{
    [SerializeField] private Camera mainCamera;
    
    [SerializeField] private MeteorStrike meteorPrefab;
    [SerializeField] private LayerMask clickableMask;
    private Vector3 _clickedPos;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && ClickPosition())
        {
            CastMeteor(_clickedPos, photonView.ViewID);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            
        }
    }

    private bool ClickPosition()
    { 
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        var a = Physics.Raycast(ray,out var hit, Mathf.Infinity, clickableMask) ? true : false;
        _clickedPos = hit.point;
        
        return a;
    }
    
    public void CastMeteor(Vector3 clickWorldPoint, int playerId)
    {
        //var meteor = PhotonNetwork.Instantiate(meteorPrefab.name, transform.position, Quaternion.identity);
        var meteor = Instantiate(meteorPrefab);
        meteor.GetComponent<MeteorStrike>().Initialize(clickWorldPoint, playerId);
    }

}
