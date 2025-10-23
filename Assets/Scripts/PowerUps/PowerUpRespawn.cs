using System.Collections;
using UnityEngine;

public class PowerUpRespawn : MonoBehaviour
{
    [SerializeField] private GameObject creator;
    [SerializeField] private float respawnDelay = 5f;
    public void DisableCreator()
    {
            creator.SetActive(false);
            StartCoroutine(RespawnCreator());
    }

    private IEnumerator RespawnCreator()
    {
        yield return new WaitForSeconds(respawnDelay);

        creator.SetActive(true);
    }
}