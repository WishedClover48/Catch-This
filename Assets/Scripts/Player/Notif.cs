using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Notif : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text text;

    private void Start()
    {
        if (image.IsActive())
        {
            image.enabled = false;
        }
    }

    public void ShowText(string value)
    {
        image.enabled = true;
        text.text = value;
    }

    public void HideText()
    {
        if (image.IsActive())
        {
            image.enabled = false;
        }
    }
}
