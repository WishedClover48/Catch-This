using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerMovement : MonoBehaviourPunCallbacks
{
    public float moveSpeed = 5f;
    private Camera playerCam;

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

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        photonView.RPC(nameof(RPC_syncState), newPlayer, 1);
    }

    [PunRPC]
    private void RPC_syncState(int want)
    {

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
    }
}