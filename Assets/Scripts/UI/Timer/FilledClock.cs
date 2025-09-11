using UnityEngine;
using UnityEngine.UI;

public class FilledClock : MonoBehaviour
{
    [SerializeField]private Timer timer;
    private Image _clock;
    void Awake()
    {
        _clock = GetComponent<Image>();
    }

    void Update()
    {
        _clock.fillAmount = timer.GetTimePercent();
    }
}
