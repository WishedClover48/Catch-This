using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorStrike : MonoBehaviour
{
    [Header("Falling Settings")]
    public float fallSpeed = 5f;
    public LayerMask floorLayer;

    private void Update()
    {
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & floorLayer) == 0) return;
        Debug.Log("Hit");
        Destroy(gameObject);
    }
}
