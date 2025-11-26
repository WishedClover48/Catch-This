using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoreFireRate : PowerUp
{
    [SerializeField] private float firerateModifier;
    [SerializeField] private float buffDuration;
    protected override void ApplyEffect(PlayerMovement playerMovement)
    {
        ApplyLogic(playerMovement);

        playerMovement.RunCoroutine(() => FinishBuff(playerMovement), buffDuration);
    }

    void ApplyLogic(PlayerMovement playerMovement)
    {
        playerMovement.CurrentPowerUp = "FireRate";
        playerMovement.firerate += firerateModifier;
    }

    void FinishBuff(PlayerMovement playerMovement)
    {
        playerMovement.CurrentPowerUp = "Null";
        playerMovement.firerate -= firerateModifier;
    }
}
