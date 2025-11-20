using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun.UtilityScripts;

public class ChestLogic : MonoBehaviourPunCallbacks
{
    private int ChestID;
    Coroutine Coroutine;
    [SerializeField] private LayerMask layer;
    [SerializeField] private float activatedTime;
    string HashName;
    [SerializeField] private Animator animator;
    private bool isOpened;
    void Start()
    {
        HashName = ChestID.ToString() + "ChestPress";
        isOpened = false;
    }

    void Update()
    {
        if (PhotonNetwork.IsMasterClient && !isOpened)
            CheckIfAllAreActive();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && Input.GetKeyDown(KeyCode.Space))
        {
            if (Coroutine != null) 
                StopCoroutine(Coroutine);
            SetPlayerVariable(PhotonNetwork.LocalPlayer, HashName, true);
            Coroutine = StartCoroutine(TurnOfProperty());
        }
    }
    
    private void SetPlayerVariable(Player player, string variable, bool value)
    {  
        ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable { { variable, value } };
        player.SetCustomProperties(props);
    }
    IEnumerator TurnOfProperty()
    {
        yield return new WaitForSeconds(activatedTime);
        SetPlayerVariable(PhotonNetwork.LocalPlayer, HashName, false);
    }
    void CheckIfAllAreActive()
    {
        Collider[] list = Physics.OverlapSphere(transform.position, 5, layer);
        if (list.Length <= 1)
        {
            return;
        }
        List<Player> playersInRange = new List<Player>();
        foreach (Collider item in list)
        {
            Player currentPlayer = item.GetComponent<PhotonView>().Owner;
            currentPlayer.CustomProperties.TryGetValue(HashName,out var chestPressed);
            if ((bool)chestPressed == false)
            {
                return;
            }
            playersInRange.Add(currentPlayer);
        }
        isOpened = true;
        GiveRewards(playersInRange);
        ChestEffect();
        photonView.RPC("ChestEffect",RpcTarget.Others);
    }

    private void GiveRewards(List<Player> players)
    {
        PhotonNetwork.Instantiate("PowerUpChest", transform.position + new Vector3(0,1f,0), transform.localRotation);
        foreach (Player player in players)
        {
            player.AddScore(2);
        }
    }

    [PunRPC]
    private void ChestEffect()
    {
        animator.SetTrigger("IsOpened");
        StartCoroutine(WaitForAnimation());
    }
    IEnumerator WaitForAnimation()
    {
        yield return new WaitForSeconds(2);

        gameObject.SetActive(false);
    }
}
