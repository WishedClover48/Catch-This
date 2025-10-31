using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownUI : MonoBehaviour
{
    [SerializeField] private MonoTimer MonoTimer;
    [SerializeField] private GodPlayer player;

    void Start()
    {
        MonoTimer.SetTimer(4);
    }

}
