using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disabler : MonoBehaviour
{
    private void Awake()
    {
        gameObject.SetActive(false);
    }
}
