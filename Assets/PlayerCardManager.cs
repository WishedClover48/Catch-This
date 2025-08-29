using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCardManager : MonoBehaviour
{
    [SerializeField] public GameObject playerCardPrefab;
    [SerializeField] public HorizontalLayoutGroup container;

    private void Start()
    {
        // Now we can spawn the player card
        SpawnPlayerCard();
    }

    public void SpawnPlayerCard()
    {
        if (playerCardPrefab == null)
        {
            Debug.LogError("Player card Prefab is not assigned in Manager!");
            return;
        }

    // Instantiate the player card over the network
    GameObject playerCard = PhotonNetwork.Instantiate(playerCardPrefab.name, new Vector3(0,0,0), Quaternion.identity);
    GameObject playerName = GetChild(playerCard, "PlayerName");
    GameObject playerState = GetChild(playerCard, "PlayerState");

        if (playerName != null)
        {
            SetTMPText(playerName, "lol");
        }
        if (playerState != null)
        {
            SetTMPText(playerState, "xd");
        }
    playerCard.transform.SetParent(container.transform, false);
    }

    GameObject GetChild(GameObject parentGO, string ChildName)
    {
        Transform child = parentGO.transform.Find(ChildName);
        return child.gameObject;
    }

    void SetTMPText(GameObject textGO,string text)
    {
        TextMeshProUGUI TMP = textGO.GetComponent<TextMeshProUGUI>();
        TMP.text = text;
    }
}
