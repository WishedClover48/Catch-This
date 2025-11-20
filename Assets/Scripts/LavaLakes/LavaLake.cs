using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class LavaLake : MonoBehaviourPunCallbacks
{
    [Header("Visuals")]
    [SerializeField] private Vector3 finalScale = new Vector3(40f, 1f, 40f);
    [SerializeField] private float expandDuration = 15f;

    [Header("Kill Logic")]
    [SerializeField] private LayerMask mask;
    [SerializeField] private float killTime = 4f;
    
    private readonly Dictionary<GameObject, float> _playersOnLava = new Dictionary<GameObject, float>();
    
    private void OnEnable()
    {
        transform.localScale = Vector3.one;
        StartCoroutine(ExpandRoutine());
        _playersOnLava.Clear();
    }
    private IEnumerator ExpandRoutine()
    {
        float elapsed = 0f;
        Vector3 startScale = Vector3.one;

        while (elapsed < expandDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / expandDuration);
            transform.localScale = Vector3.Lerp(startScale, finalScale, t);
            yield return null;
        }

        transform.localScale = finalScale;
    }
    
    private void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        if (_playersOnLava.Count == 0)
            return;

        // We’ll iterate over a copy to avoid modifying the dictionary while iterating
        var keys = new List<GameObject>(_playersOnLava.Keys);

        foreach (var playerObj in keys)
        {
            if (playerObj == null)
            {
                _playersOnLava.Remove(playerObj);
                continue;
            }

            _playersOnLava[playerObj] += Time.deltaTime;

            if (_playersOnLava[playerObj] >= killTime)
            {
                KillPlayer(playerObj);
                _playersOnLava.Remove(playerObj);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.GetComponent<Notif>()?.ShowText("Danger Lava");
        if (!PhotonNetwork.IsMasterClient) return;

        if (!_playersOnLava.ContainsKey(other.gameObject))
        {
            _playersOnLava.Add(other.gameObject, 0f);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        other.gameObject.GetComponent<Notif>().HideText();
        
        if (!PhotonNetwork.IsMasterClient) return;

        if (_playersOnLava.ContainsKey(other.gameObject))
        {
            _playersOnLava.Remove(other.gameObject);
        }
    }
    
    private void KillPlayer(GameObject playerObj)
    {
        if(!photonView.IsMine) return;

        if (InMask(mask, playerObj.layer))
        {
            PhotonView pv = playerObj.GetComponent<PhotonView>();
            if (pv != null)
            {
                pv.RPC("KillPlayer", pv.Owner);
            }
        }
    }
    
    private static bool InMask( LayerMask mask,int layer)
    {
        return (mask.value & (1 << layer)) != 0;
    }
}
