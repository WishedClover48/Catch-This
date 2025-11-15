using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Notif : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private TMP_Text text;

    private void Start()
    {
        canvas.SetActive(false);
    }

    public void ShowText(string value)
    {
        canvas.SetActive(true);
        text.text = value;
    }

    public void HideText()
    {
        text.text = "";
        canvas.SetActive(false);
    }
}
