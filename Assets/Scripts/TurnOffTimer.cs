using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TurnOffTimer : MonoBehaviour
{
    [SerializeField] float aliveTime = 1;
    private void OnEnable()
    {
        StartCoroutine(Timer());
    }
    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(aliveTime);
        gameObject.SetActive(false);
    }
}
