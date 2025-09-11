using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SkillIcon : MonoBehaviour
{
    [FormerlySerializedAs("CooldownIndicator")] [SerializeField] private Image cooldownIndicator;
    [SerializeField] private Image icon;

    private Timer _timer=new Timer();

    [ContextMenu("start cooldown")]
    private void Start5SCooldown()
    {
        StartCooldown(5);
    }

    public void SetIcon(Sprite sprite)
    {
        icon.sprite = sprite;
    }
    public void StartCooldown(float time)
    {
        cooldownIndicator.fillAmount = 1;
        _timer.StartTimer(time);
    }
    
    private void Update()
    {
        _timer.Count();
        cooldownIndicator.fillAmount = _timer.GetTimePercent();
    }
}
