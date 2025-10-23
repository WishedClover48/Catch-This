using UnityEngine;

public class PowerUp : MonoBehaviour
{
    PlayerMovement _player;
    private void OnTriggerEnter(Collider collision)
    {
         collision.TryGetComponent<PlayerMovement>(out _player);
        if (_player != null)
        {
            ApplyEffect(_player);
        }
    }
    protected virtual void ApplyEffect(PlayerMovement playerMovement)
    {
        Debug.LogWarning("Apply effect of " + gameObject.name + " is empty.");
    }
}
