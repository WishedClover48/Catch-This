using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    [SerializeField] private Vector2 Direction = Vector2.one;
    [SerializeField] private float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (Vector3)Direction *speed* Time.deltaTime;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Get the contact point of the collision
        Vector2 hitPoint = collision.contacts[0].point;
        Vector2 objectCenter = transform.position;
        Vector2 hitDirection = hitPoint - objectCenter;

        DetermineSide(hitDirection);
    }

    void DetermineSide(Vector2 hitDirection)
    {
        float x = hitDirection.x;
        float y = hitDirection.y;

        if (Mathf.Abs(x) > Mathf.Abs(y))
        {
            Direction.x *= -1;
        }
        else
        {
            Direction.y *= -1;
        }
    }
}
