using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerCanvas : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private float _popScale = 1.2f;
    [SerializeField] private float _popDuration = 0.2f;
    [SerializeField] private RoundsManager _roundsManager;
    private float _roundTimer;
    private int _lastSecond = 0;
    private Vector3 _originalScale;

    void Start()
    {
        _roundTimer = _roundsManager.RoundDuration; //This makes it dependant of the manager.
        _originalScale = _timerText.transform.localScale;
    }

    void Update()
    {
        _roundTimer -= Time.deltaTime;
        ManageTimerText();
    }

    void ManageTimerText()
    {
        int seconds = Mathf.Clamp(Mathf.FloorToInt(_roundTimer), 0, 999);
        if (seconds != _lastSecond)
        {
            _lastSecond = seconds;
            _timerText.text = seconds.ToString();

            StopAllCoroutines(); //This stops all coroutines inside this code, refactor if more coroutines are used here.
            StartCoroutine(PopAnimation());
        }
    }
    private IEnumerator PopAnimation()
    {
        _timerText.transform.localScale = _originalScale * _popScale;

        float t = 0f;
        while (t < _popDuration)
        {
            t += Time.deltaTime;
            _timerText.transform.localScale = Vector3.Lerp(_originalScale * _popScale, _originalScale, t / _popDuration);
            yield return null;
        }

        _timerText.transform.localScale = _originalScale;
    }

}
