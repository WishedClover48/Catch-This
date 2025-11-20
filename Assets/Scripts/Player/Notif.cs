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
            Hide();
        }
    }
    public void ShowText(string value)
    {
        Show();
        text.text = value;
    }
    public void HideText()
    {
        if (image.IsActive())
        {
            Hide();
        }
    }

    private void Show()
    {
        image.enabled = true;
        text.enabled = true;
    }
    private void Hide()
    {
        image.enabled = false;
        text.enabled = false;
    }
}
