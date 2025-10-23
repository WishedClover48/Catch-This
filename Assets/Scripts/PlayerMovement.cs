using Photon.Pun;
using System;
using System.Collections;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
public class PlayerMovement : MonoBehaviourPunCallbacks
{
    public float moveSpeed = 5f; 
    private Camera _playerCam;
    [SerializeField] private Vector3 cameraOffset;
    [SerializeField] private Quaternion cameraRotation;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] public float firerate;
    public LayerMask groundMask;
    public Action<int, string> OnHit;
    private PhotonView pv;
    
    private float nextFireTime = 0f;
    public PhotonView Pv { get => pv; }

    void Start()
    {
        pv = GetComponent<PhotonView>();

        // Enable it only for the local player
        if (photonView.IsMine)
        {
            CreateCamera();
        }
        PlayersManager.Instance.MarkAsAlive(PhotonNetwork.LocalPlayer);
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

        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + 1f / firerate;

            var bullet = PhotonNetwork.Instantiate(bulletPrefab.name, transform.position, transform.rotation);
            bullet.GetComponent<BulletMovement>().SetUpOwner(gameObject, photonView.Owner);
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
        //_playerCam.enabled = false;

        PlayersManager.Instance.MarkAsDead(PhotonNetwork.LocalPlayer);
        gameObject.SetActive(false);
        if (photonView.IsMine)
        {
            RepositionCamera();
            TurnOnLeaderBoardToggel();
        }
        
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
        cameraObject.transform.rotation = cameraRotation;
        cameraObject.transform.position = transform.position + cameraOffset;

        _playerCam = cam;
    }

    private void RepositionCamera()
    {
        PlayersManager.Instance.SetCamaraOnDeath(_playerCam);
    }
    private void TurnOnLeaderBoardToggel()
    {
        UIManager.Instance.LeaderBoardToggleButton();
    }

    public void RunCoroutine(Action func, float delay)
    {
        StartCoroutine(ExecuteAfterDelay(func, delay));
    }
    IEnumerator ExecuteAfterDelay(Action func, float delay) 
    {
        yield return new WaitForSeconds(delay);
        func();
    }


    [PunRPC]
    public void KillPlayer()
    {
        UnityEngine.Debug.Log("KILLPLAYER WAS CALLED FOR " + PhotonNetwork.LocalPlayer.UserId);
        Killed(1, "A");
    }

}