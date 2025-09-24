using UnityEngine;
using UnityEngine.UI;

namespace UI.Timer
{
    public class FilledClock : MonoBehaviour
    {
        [SerializeField]private MonoTimer monoTimer;
        private Image _clock;
        void Awake()
        {
            _clock = GetComponent<Image>();
        }

        void Update()
        {
            _clock.fillAmount = monoTimer.GetTimePercent();
        }
    }
}
