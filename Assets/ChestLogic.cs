using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class ChestLogic : MonoBehaviourPunCallbacks
{
    private int ChestID;
    Coroutine Coroutine;
    [SerializeField] private LayerMask layer;
    string HashName;
    [SerializeField] private Color ClaimdColor;
    void Start()
    {
        HashName = ChestID.ToString() + "ChestPress";
    }

    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
            CheckIfAllAreActive();

    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && Input.GetKeyDown(KeyCode.Space)) 
        {
            if(Coroutine != null)
                StopCoroutine(Coroutine); 
            SetPlayerVariable(PhotonNetwork.LocalPlayer,HashName,true);
            //GetComponent<MeshRenderer>().material.color = Color.green;
            Coroutine = StartCoroutine(TurnOfProperty());
            StartCoroutine(TurnOfProperty());
        }
    }
    private void SetPlayerVariable(Player player, string variable, bool value)
    {  
        ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable { { variable, value } };
        player.SetCustomProperties(props);
    }
    IEnumerator TurnOfProperty()
    {
        yield return new WaitForSeconds(3);
        //GetComponent<MeshRenderer>().material.color = Color.white;
        SetPlayerVariable(PhotonNetwork.LocalPlayer, HashName, false);
    }
    void CheckIfAllAreActive()
    {
        var list = Physics.OverlapSphere(transform.position, 5, layer);
        if (list.Length == 0)
        {
            //GetComponent<MeshRenderer>().material.color = Color.blue;
            return;
        }
        //GetComponent<MeshRenderer>().material.color = Color.yellow;
        foreach (var item in list)
        {
            item.GetComponent<PhotonView>().Owner.CustomProperties.TryGetValue(HashName,out var test);
            if ((bool)test == false)
            {
                return;
            }
        }
        ChestEffect();
        photonView.RPC("ChestEffect",RpcTarget.AllBuffered);
    }
    [PunRPC]
    private void ChestEffect()
    {
        Debug.Log("pepe");   
        GetComponent<MeshRenderer>().material.color = Color.red;
    }
}
