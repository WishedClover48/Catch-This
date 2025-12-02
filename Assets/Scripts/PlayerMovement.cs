using Photon.Pun;
using System;
using System.Collections;
using System.Diagnostics;
using Unity.Services.Analytics;
using Unity.VisualScripting;
using UnityEngine;
public class PlayerMovement : MonoBehaviourPunCallbacks
{
    public float moveSpeed = 5f; 
    [Space]
    [Header("Camera config")]
    [SerializeField] private Vector3 cameraOffset;
    [SerializeField] private Quaternion cameraRotation;
    [Space]
    [Header("Bullet config")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] public float firerate;
    [Space]
    public LayerMask groundMask;
    [SerializeField] private Animator animator;

    [Space]
    [SerializeField] private AnimationSV animationSync;

    [SerializeField] public string CurrentPowerUp= "null";
    
    public Action<int, string,string> OnHit;
    public PhotonView Pv { get; private set; }
    private float _nextFireTime = 0f;
    private Camera _playerCam;
    
    private Vector3 _lastPosition;
    void Start()
    {
        Pv = GetComponent<PhotonView>();

        _lastPosition = transform.position;
        
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
        if (photonView.IsMine)
        {
            LookAt();

            // Get input
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveZ = Input.GetAxisRaw("Vertical");

            // Direction
            Vector3 movement = new Vector3(moveX, 0f, moveZ).normalized;

            // Move player
            transform.position += movement * (moveSpeed * Time.deltaTime);
            //if(transform.position.y >= 1.7f)
            //{
            //    transform.position = new Vector3(transform.position.x, 1, transform.position.z);
            //}
            // Move camera
            if (_playerCam != null)
            {
                _playerCam.transform.position = transform.position + cameraOffset;
            }

            // Shooting
            if (Input.GetMouseButton(0) && Time.time >= _nextFireTime)
            {
                animator.SetTrigger("IsShooting");
                animationSync.FireShootTrigger();

                _nextFireTime = Time.time + 1f / firerate;

                var bullet = PhotonNetwork.Instantiate(bulletPrefab.name, transform.position, transform.rotation);
                bullet.GetComponent<Bullet>().SetUpOwner(gameObject, photonView.Owner,CurrentPowerUp);
            }
        }

        UpdateAnimation();
    }
    
    private void UpdateAnimation()
    {
        if (animator == null)
            return;

        Vector3 delta = transform.position - _lastPosition;
        float speed = delta.magnitude / Mathf.Max(Time.deltaTime, 0.0001f);

        bool isWalking = speed > 0.01f;
        animator.SetBool("IsWalking", isWalking);
    
        _lastPosition = transform.position;
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

    private void Killed(int ID, string source,string powerUpType = "null")
    {
        //_playerCam.enabled = false;

        PlayersManager.Instance.MarkAsDead(PhotonNetwork.LocalPlayer);
        
        
        gameObject.SetActive(false);
        if (photonView.IsMine)
        {
            RepositionCamera();
            TurnOnLeaderBoardToggel();
            PlayerKilled(global::ID.GetPlayerID(),global::ID.GetMatchID(),powerUpType);
        }
        
        PhotonNetwork.Destroy(gameObject);
    }
    public void PlayerKilled(int playerID, int matchID, string powerUpType)
    {
        PlayerKilledEvent evt = new PlayerKilledEvent{ PlayerID = playerID, MatchID = matchID, PowerUpType = powerUpType};
        
        AnalyticsService.Instance.RecordEvent(evt);
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
    [PunRPC]
    public void KillPlayerWithBullet(string powerUpType)
    {
        UnityEngine.Debug.Log("KILLPLAYER WAS CALLED FOR " + PhotonNetwork.LocalPlayer.UserId);
        Killed(1, "A", powerUpType);
    }

}