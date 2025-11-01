using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownController : MonoBehaviour
{
    [SerializeField] private MonoTimer maskTimer;
    [SerializeField] private Image logoImage;
    private GodAttack _attack;

    public void SetUp(GodAttack attack)
    {
        if (_attack != null) _attack.OnCooldownStart -= StartTimer;

        _attack = attack;
        maskTimer.SetTimer(_attack.Cooldown);

        logoImage.sprite = _attack.logo;
        
        _attack.OnCooldownStart += StartTimer;
        maskTimer.TimerFinished += ResetFill;
    }
    void StartTimer()
    {
        maskTimer.StartTimer();
    }

    void ResetFill()
    {
        maskTimer.SetCurrentTime(maskTimer.GetMaxTime());
    }
    
    private void OnDisable()
    {
        _attack.OnCooldownStart -= StartTimer;
        maskTimer.TimerFinished -= ResetFill;
    }
}
