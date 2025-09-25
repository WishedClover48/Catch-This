using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SequenceActivator : MonoBehaviour
{
    [SerializeField] private List<ObjectTimePair> _objects = new List<ObjectTimePair>();
    public event Action SequenceFinished;

    [ContextMenu("Start Sequence")]
    public void StartSequence()
    {
        StartCoroutine(Sequence());
    }
    [ContextMenu("Pause Sequence")]
    public void StopSequence()
    {
        StopAllCoroutines();
        foreach (var pair in _objects)
        {
            pair.gObject.SetActive(false);
        }
    }

    private IEnumerator Sequence()
    {
        ObjectTimePair currentPair = _objects[0];
        foreach (var pair in _objects)
        {
            currentPair.gObject.SetActive(false);
            pair.gObject.SetActive(true);
            currentPair=pair;
            StartCoroutine(Expand(pair));
            yield return new WaitForSeconds(pair.time);
        }
        currentPair.gObject.SetActive(false);
        SequenceFinished?.Invoke();
    }

    private IEnumerator Expand(ObjectTimePair pair)
    {
        var time=0f;
        var initialScale = pair.gObject.transform.localScale;
        var targetScale = initialScale*pair.Expacion;
        var image = pair.gObject.GetComponent<Image>();
        while (time < pair.time)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / pair.time);
            pair.gObject.transform.localScale = Vector3.Lerp(initialScale, targetScale, t);
            if(t >= 0.7f)
                image.color = Color.Lerp(pair.gObject.GetComponent<Image>().color, new Color(1,1,1,0.3f), t-0.7f);
            yield return 0;
        }
    }
    [Serializable]
    public struct ObjectTimePair
    {
        public GameObject gObject;
        public float time;
        public float Expacion;
    }
}