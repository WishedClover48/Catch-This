using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageResolution : MonoBehaviour
{
    private void Start()
    {
        int width = PlayerPrefs.GetInt("ResolutionWidth", 1920/2);
        int height = PlayerPrefs.GetInt("ResolutionHeight", 1080/2);
        bool isFullscreen = PlayerPrefs.GetInt("Fullscreen", 0) == 1;

        Screen.SetResolution(width, height, isFullscreen);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
