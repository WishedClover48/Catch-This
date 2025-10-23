using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : PowerUp
{
    [SerializeField] private float speedModifier;
    [SerializeField] private float buffDuration;
    protected override void ApplyEffect(PlayerMovement playerMovement)
    {
        ApplyLogic(playerMovement);

        playerMovement.RunCoroutine(() => FinishBuff(playerMovement), buffDuration);
    }
    void ApplyLogic(PlayerMovement playerMovement)
    {
        playerMovement.moveSpeed += speedModifier;
    }

    void FinishBuff(PlayerMovement playerMovement)
    {
        playerMovement.moveSpeed -= speedModifier;
    }
}
