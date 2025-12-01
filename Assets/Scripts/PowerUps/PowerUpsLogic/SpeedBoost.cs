using UnityEngine;

public class SpeedBoost : PowerUp
{
    [SerializeField] private float speedModifier;
    [SerializeField] private float buffDuration;

    protected override void Awake()
    {
        powerUpType = PowerUpType.SpeedBoost;
        base.Awake();
    }

    protected override void ApplyEffect(PlayerMovement playerMovement)
    {
        ApplyLogic(playerMovement);
        playerMovement.RunCoroutine(() => FinishBuff(playerMovement), buffDuration);
    }

    void ApplyLogic(PlayerMovement playerMovement)
    {
        playerMovement.CurrentPowerUp = "SpeedBoost";
        playerMovement.moveSpeed += speedModifier;
    }

    void FinishBuff(PlayerMovement playerMovement)
    {
        playerMovement.CurrentPowerUp = "Null";
        playerMovement.moveSpeed -= speedModifier;
    }
}
