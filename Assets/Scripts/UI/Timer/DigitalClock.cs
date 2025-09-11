using TMPro;
using UnityEngine;

public class DigitalClock : MonoBehaviour
{
    [SerializeField] private Timer timer;

    private TextMeshProUGUI _clock;

    void Start()
    {
        _clock = GetComponent<TextMeshProUGUI>();
    }
    
    void Update()
    {
        var time = timer.GetTime();
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        //_clock.text = $"{minutes:00}:{seconds:00}";
        if (minutes > 0)
        {
            _clock.text = $"{minutes}:{seconds:00}";
        }
        else
        {
            _clock.text = $"{seconds}";
        }

    }
}