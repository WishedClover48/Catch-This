using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviourPunCallbacks
{
    public float moveSpeed = 5f;
    private Camera playerCam;
    [SerializeField] private GameObject bulletPrefab;

    void Start()
    {
        // Get the camera from the child
        playerCam = GetComponentInChildren<Camera>();

        // Enable it only for the local player
        if (photonView.IsMine)
        {
            if (playerCam != null)
                playerCam.enabled = true;
        }
    }

    void Update()
    {
        // Ensure only the local player moves this instance
        if (!photonView.IsMine)
            return;

        // Get input
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        // Calculate direction
        Vector3 movement = new Vector3(moveX, 0f, moveZ).normalized;

        // Move the player
        transform.position += movement * moveSpeed * Time.deltaTime;
        if(Input.GetMouseButtonDown(0)) {
            PhotonNetwork.Instantiate(bulletPrefab.name, transform.position  + Vector3.forward * 2, transform.rotation);
        }
    }

    public void Killed()
    {
        //Play death anim

        playerCam.enabled = false;
        if (!XRayCam.Instance.MainCamera.enabled&&photonView.IsMine)
        {
            XRayCam.Instance.MainCamera.enabled = true;
        }
            this.gameObject.SetActive(false);

    }
}