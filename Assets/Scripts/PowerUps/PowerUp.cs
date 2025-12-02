using UnityEngine;
using Photon.Pun;
using Unity.Services.Analytics;

public class PowerUp : MonoBehaviourPun
{
    protected PlayerMovement _player;
    private bool _picked = false;
    private float _spawnTime;

    [SerializeField] protected PowerUpType powerUpType = PowerUpType.None;

    protected virtual void Awake()
    {
        _spawnTime = Time.time;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (_picked) return;

        if (collision.TryGetComponent<PlayerMovement>(out _player))
        {
            _picked = true;

            if (PhotonNetwork.IsMasterClient)
            {
                int IDsend = 0;
                foreach (char letter in _player.Pv.Owner.UserId)
                {
                    IDsend += (int)letter;
                }
                SendAnalyticsEvent(IDsend);
            }

            ApplyEffect(_player);
        }
    }

    protected virtual void ApplyEffect(PlayerMovement playerMovement)
    {
        Debug.LogWarning("Apply effect of " + powerUpType + " is empty.");
    }

    private void OnDestroy()
    {
        if (!_picked && PhotonNetwork.IsMasterClient)
        {
            SendAnalyticsEvent(0); // Destroyed without being picked
        }
    }

    private void SendAnalyticsEvent(int playerID)
    {
        float lifetime = Time.time - _spawnTime;

        PowerUpPickedEvent evt = new PowerUpPickedEvent
        {
            MatchID = ID.GetMatchID(),
            PowerUpType = powerUpType.ToString(),
            LifeTime = Mathf.RoundToInt(lifetime),
            PlayerID = playerID
        };
        
        AnalyticsService.Instance.RecordEvent(evt);
    }
}
