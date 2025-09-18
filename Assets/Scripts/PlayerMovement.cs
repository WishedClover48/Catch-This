using System;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviourPunCallbacks
{
    public float moveSpeed = 5f;
    [SerializeField]private Camera playerCam;
    private Vector3 camaraStartingPosition;
    [SerializeField] private GameObject bulletPrefab;
    public LayerMask groundMask;

    public Action<int, string> OnHit;
    private PhotonView pv;

    public PhotonView Pv { get => pv; }

    void Start()
    {
        pv = GetComponent<PhotonView>();

        // Enable it only for the local player
        if (photonView.IsMine)
        {
            if (playerCam != null) playerCam.enabled = true;
            camaraStartingPosition=playerCam.transform.position;
            //photonView.RPC("AddPlayer", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer, this.gameObject); //Modificar
        }

        OnHit += Killed;
    }

    void Update()
    {
        // Ensure only the local player moves this instance
        if (!photonView.IsMine) return;
        LookAt();   
        // Get input
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        // Calculate direction
        Vector3 movement = new Vector3(moveX, 0f, moveZ).normalized;
        playerCam.transform.position = camaraStartingPosition+transform.position;

        // Move the player
        transform.position += movement * (moveSpeed * Time.deltaTime);

        if (Input.GetMouseButtonDown(0)) {
            
            PhotonNetwork.Instantiate(bulletPrefab.name, transform.position + Vector3.forward * 2, transform.rotation);
        }
    }
    void LookAt()
    {
        Ray ray = playerCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Raycast to the ground
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundMask))
        {
            Vector3 lookAtPoint = hit.point;
            lookAtPoint.y = transform.position.y; // Keep only horizontal rotation
            transform.LookAt(lookAtPoint);
        }
    }

    private void Killed(int ID, string source)
    {
        //Play death anim
        Debug.Log($"{photonView.Owner.NickName} killed by {ID} using {source}");

        playerCam.enabled = false;
        if (!XRayCam.Instance.MainCamera.enabled&&photonView.IsMine)
        {
            XRayCam.Instance.MainCamera.enabled = true;
        }

        PhotonNetwork.Destroy(gameObject);
    }

    [PunRPC]
    public void KillPlayer()
    {
        OnHit.Invoke(1,"a");
    }

}