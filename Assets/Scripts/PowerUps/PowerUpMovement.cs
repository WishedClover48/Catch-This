using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpMovement : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float hoverSpeed;
    [SerializeField] private float hoverDifference;
    private Vector3 _initialPosition;
    private void Awake()
    {
        _initialPosition = transform.position;
    }
    private void FixedUpdate()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.fixedDeltaTime, Space.World);

        float newY = _initialPosition.y + Mathf.Sin(Time.time * hoverSpeed) * hoverDifference;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
