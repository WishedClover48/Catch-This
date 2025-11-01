using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSampleScene : MonoBehaviour
{
    private GameManager _gameManager;
    private void Awake()
    {
        _gameManager = GameManager.GetGameManager();
        _gameManager.RoundStart += ToggleCamera;
    }

    private void ToggleCamera()
    {
        gameObject.SetActive(!gameObject.activeInHierarchy);
    }
}
