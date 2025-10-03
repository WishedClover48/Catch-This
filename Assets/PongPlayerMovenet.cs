using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PongPlayerMovenet : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    [SerializeField] public float speed;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow)) {
                transform.position -= new Vector3(0, speed);
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                transform.position += new Vector3(0, speed);
            }
        }
    }
}
