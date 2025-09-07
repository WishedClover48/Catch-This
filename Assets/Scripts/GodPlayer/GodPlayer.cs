using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodPlayer : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PerformRaycastAndHandleClick(0);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            PerformRaycastAndHandleClick(1);
        }
    }
    void PerformRaycastAndHandleClick(int buttonIndex)
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Check if we hit something, and if it's on the ground layer or has a specific tag
            if (hit.collider != null)
            {
                Vector3 clickPosition = hit.point;
                Debug.Log($"Clicked on position: {clickPosition} with button {buttonIndex}");

                // Now you can handle different actions based on the button index
                if (buttonIndex == 0)
                {
                    // Left click action, for example a primary attack
                }
                else if (buttonIndex == 1)
                {
                    // Right click action, for example a secondary attack
                }
            }
        }
    }

}
