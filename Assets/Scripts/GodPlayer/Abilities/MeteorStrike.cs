using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorStrike : MonoBehaviour
{
    public int PlayerID { get; private set; }
    public Vector3 CrashPoint { get; private set; }
    
    [Header("Stats")]
    [SerializeField] private float fallTime = 2.0f;
    [SerializeField] private float hitRadius = 4;
    
    [Header("Spawn")]
    [SerializeField] private float spawnHeight = 35f;
    [SerializeField] private float spawnRadius = 5f;
    private Vector3 _spawnPoint;
    
    [Header("Layers")]
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private LayerMask floorMask;
    [SerializeField] private LayerMask obstacleMask;
    
    [Header("Hit ring")]
    [SerializeField] private GameObject ringGameObject;
    
    [SerializeField] private bool _initialized;
    private float _t;
    private LineRenderer _ring;
    
    private void Awake()
    {
        var col = GetComponent<Collider>();
        col.isTrigger = true;
        
        _ring = ringGameObject.GetComponent<LineRenderer>();
    }

    private void Start()
    {
        if (!_initialized)
        {
            Debug.LogWarning($"MeteorStrike not initialized. Call Initialize(crashPoint, playerId) right after Instantiate.");
        }
    }

    private void Update()
    {
        if (!_initialized) return;
        
        _t += Time.deltaTime / Mathf.Max(0.0001f, fallTime);
        _t = Mathf.Clamp01(_t);
        transform.position = Vector3.Lerp(_spawnPoint, CrashPoint, _t);
    }
    public void Initialize(Vector3 crashPoint, int playerId)
    {
        PlayerID   = playerId;
        CrashPoint = crashPoint;
        
        Vector2 rand = UnityEngine.Random.insideUnitCircle * spawnRadius;
        _spawnPoint = new Vector3(crashPoint.x + rand.x, spawnHeight, crashPoint.z + rand.y);
        
        transform.position = _spawnPoint;
        Vector3 fallDir = (CrashPoint - _spawnPoint);
        if (fallDir.sqrMagnitude > 0.0001f) transform.rotation = Quaternion.LookRotation(fallDir.normalized, Vector3.up);
        
        CreateRing();

        _initialized = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        int otherLayer = other.gameObject.layer;

        if (InMask(otherLayer, playerMask))
        {
            KillPlayer(other.gameObject);
            return;
        }

        if (InMask(otherLayer, obstacleMask))
        {
            Debug.Log("Hit an obstacle");
            SelfDestruct();
            return;
        }
        
        if (InMask(otherLayer, floorMask))
        {
            Debug.Log("Hit floor");
            HandleFloorImpact();
            return;
        }
    }
    private void HandleFloorImpact()
    {
        float yMin = CrashPoint.y, yMax = CrashPoint.y + 1f;
        float r2 = hitRadius * hitRadius;
        

        foreach (var player in GameManager.Instance.AllPlayers)
        {
            if (player == null) continue;
            Vector3 p = player.transform.position;
            if (p.y < yMin || p.y > yMax) continue;

            float dx = p.x - CrashPoint.x;
            float dz = p.z - CrashPoint.z;
            
            if (dx * dx + dz * dz <= r2)
            {
                KillPlayer(player.gameObject);
            }
        }

        SelfDestruct();
    }
    private void KillPlayer(GameObject playerGo)
    {
        var pm = playerGo.GetComponent<PlayerMovement>();
        if (pm != null)
        {
            pm.OnHit?.Invoke(PlayerID, gameObject.name);
        }
    }
    private void SelfDestruct()
    {
        _initialized = false;
        ringGameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    private static bool InMask(int layer, LayerMask mask)
    {
        return (mask.value & (1 << layer)) != 0;
    }

    private void CreateRing()
    {
        int previewCircleSegments = 48;
    
        _ring.positionCount = previewCircleSegments + 1;
        _ring.widthMultiplier = 0.1f;
        
        ringGameObject.transform.position = new Vector3(CrashPoint.x, CrashPoint.y + 0.03f, CrashPoint.z);

        float step = Mathf.PI * 2f / previewCircleSegments;
        for (int i = 0; i <= previewCircleSegments; i++)
        {
            float a = i * step;
            Vector3 p = new Vector3(Mathf.Cos(a) * hitRadius, 0f, Mathf.Sin(a) * hitRadius);
            _ring.SetPosition(i, ringGameObject.transform.position + p);
        }
    }
    
}
