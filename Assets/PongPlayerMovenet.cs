using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PongPlayerMovenet : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    [SerializeField] public float speed;
    private float x;
    void Start()
    {
        x= transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y > 5)
            transform.position = new Vector2(x,5);

        if (transform.position.y < -3)
            transform.position = new Vector2(x, -3);
        if (photonView.IsMine )
        {
            if (Input.GetKey(KeyCode.DownArrow)) {
                transform.position -= new Vector3(0, speed*Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                transform.position += new Vector3(0, speed*Time.deltaTime);
            }
        }
    }
}
