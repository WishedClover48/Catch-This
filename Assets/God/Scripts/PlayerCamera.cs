using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public static class PlayerCamera
{
    public static Camera Camera { private set; get; }

    public static void CreateCamera(Transform parent, int fieldOfView, Quaternion rotation)
    {
        GameObject cameraObject = new GameObject("PlayerCamera(Local)");
        
        Camera = cameraObject.AddComponent<Camera>();
        
        Camera.orthographic = false;
        Camera.fieldOfView = fieldOfView;
        
        cameraObject.transform.parent = parent;
        cameraObject.transform.rotation = rotation;
        cameraObject.transform.position = parent.position;
    }
}
