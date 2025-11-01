using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownController : MonoBehaviour
{
    [SerializeField] private MonoTimer maskTimer;
    [SerializeField] private Image logoMaskImage;
    [SerializeField] private Image logoImage;
    [SerializeField] private TMPro.TMP_Text text;
    private GodAttack _attack;

    public void SetUp(GodAttack attack)
    {
        if (_attack != null) _attack.OnCooldownStart -= StartTimer;

        _attack = attack;
        maskTimer.SetTimer(_attack.Cooldown);
        
        maskTimer.gameObject.SetActive(false);

        logoImage.sprite = _attack.logo;
        logoMaskImage.sprite = _attack.logo;

        text.text = _attack.input.ToString();
        
        _attack.OnCooldownStart += StartTimer;
        maskTimer.TimerFinished += ResetFill;
        maskTimer.gameObject.SetActive(false);
    }
    void StartTimer()
    {
        maskTimer.gameObject.SetActive(true);
        maskTimer.StartTimer();
    }
    void ResetFill()
    {
        maskTimer.SetCurrentTime(maskTimer.GetMaxTime());
        maskTimer.gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        _attack.OnCooldownStart -= StartTimer;
        maskTimer.TimerFinished -= ResetFill;
    }
}
