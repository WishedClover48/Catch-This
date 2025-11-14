using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PowerUpChooser : MonoBehaviourPun
{
    [SerializeField] private List<GameObject> powerUpList;
    [SerializeField] private PowerUpRespawn spawner;

    private GameObject _currentPowerUpInstance;

    private void OnEnable()
    {
        if (powerUpList == null || powerUpList.Count == 0)
        {
            Debug.LogWarning("PowerUpChooser: No power-ups assigned!");
            return;
        }

        if (PhotonNetwork.IsMasterClient)
        {
            int randomIndex = Random.Range(0, powerUpList.Count);

            photonView.RPC(nameof(RPC_SpawnPowerUp), RpcTarget.All, randomIndex);
        }
    }

    [PunRPC]
    private void RPC_SpawnPowerUp(int index)
    {
        if (_currentPowerUpInstance != null)
        {
            Destroy(_currentPowerUpInstance);
        }

        GameObject chosenPrefab = powerUpList[index];

        _currentPowerUpInstance = Instantiate(
            chosenPrefab,
            transform.position,
            transform.rotation,
            transform
        );
    }

    private void OnDisable()
    {
        if (_currentPowerUpInstance != null)
        {
            Destroy(_currentPowerUpInstance);
            _currentPowerUpInstance = null;
        }

        spawner.DisableCreator();
    }

    private void OnTriggerEnter(Collider other)
    {
        gameObject.SetActive(false);
    }
}