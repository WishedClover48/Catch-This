using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TestZeki : MonoBehaviour
{
    public TMP_Text test;
    public Image image;

    private void Update()
    {
        test.text = "Hi: " + image.fillAmount;
    }
}
