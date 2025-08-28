using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NicknameCanvas : MonoBehaviourPun
{
    [SerializeField] TextMeshProUGUI textMeshPro;
    private PhotonView PV;
    private void Start()
    {
        PV = GetComponentInParent<PhotonView>();
        if(PV.IsMine)
        {
            textMeshPro.text = PhotonNetwork.LocalPlayer.NickName;
        }
        else 
        {
            textMeshPro.text = PV.Owner.NickName;
        }
    }
}
