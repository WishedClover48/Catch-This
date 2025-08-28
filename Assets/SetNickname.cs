using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetNickname : MonoBehaviour
{
    [SerializeField] TMP_InputField TMP_InputField;
    public void NicknameSetter()
    {
        PhotonNetwork.NickName = TMP_InputField.text;
    }

}
