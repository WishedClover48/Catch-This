using System.Collections.Generic;
using UnityEngine;

public class PowerUpChooser : MonoBehaviour
{
    [SerializeField] private List<GameObject> powerUpList;

    private GameObject _currentPowerUpInstance;

    private void OnTriggerEnter(Collider other)
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (powerUpList == null || powerUpList.Count == 0)
        {
            Debug.LogWarning("PowerUpChooser: No power-ups assigned!");
            return;
        }

        int randomIndex = Random.Range(0, powerUpList.Count);
        GameObject chosenPrefab = powerUpList[randomIndex];

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
    }
}
