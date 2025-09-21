using System;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviourPunCallbacks
{
    public float moveSpeed = 5f; 
    private Camera _playerCam;
    [SerializeField] private Vector3 cameraOffset;
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
            CreateCamera();
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

        // Move the player & camera
        transform.position += movement * (moveSpeed * Time.deltaTime);
        _playerCam.transform.position = transform.position + cameraOffset;

        if (Input.GetMouseButtonDown(0)) {
            
            PhotonNetwork.Instantiate(bulletPrefab.name, transform.position + Vector3.forward * 2, transform.rotation);
        }
    }
    void LookAt()
    {
        Ray ray = _playerCam.ScreenPointToRay(Input.mousePosition);
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
        _playerCam.enabled = false;
        
        gameObject.SetActive(false);
        
        PhotonNetwork.Destroy(gameObject);
    }

    private void CreateCamera()
    {
        GameObject cameraObject = new GameObject("MyCamera");

        // Add a Camera component
        Camera cam = cameraObject.AddComponent<Camera>();
        
        cam.orthographic = false;
        cam.fieldOfView = 40;
        
        cameraObject.transform.parent = transform.parent;
        cameraObject.transform.rotation = Quaternion.Euler(35f, 0f, 0f);
        cameraObject.transform.position = transform.position + cameraOffset;

        _playerCam = cam;
    }

    [PunRPC]
    public void KillPlayer()
    {
        Killed(1, "A");
    }

}