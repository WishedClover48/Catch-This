using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            yield return new WaitForSeconds(pair.time);
        }
        currentPair.gObject.SetActive(false);
        SequenceFinished?.Invoke();
    }
    [Serializable]
    public struct ObjectTimePair
    {
        public GameObject gObject;
        public float time;
    }
}