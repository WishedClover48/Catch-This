using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NicknameCanvas : MonoBehaviourPun
{
    [SerializeField] TextMeshProUGUI textMeshPro;
    private void Start()
    {
        if (photonView.IsMine)
        {
            textMeshPro.text = PhotonNetwork.LocalPlayer.NickName;
        }
    }
}
