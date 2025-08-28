using UnityEngine;

public class XRayCam : MonoBehaviour
{
    public static XRayCam Instance { get; private set; }

    public Camera MainCamera { get; private set; }

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        MainCamera = GetComponent<Camera>();

        if (MainCamera == null)
        {
            Debug.LogError("SingletonCamera: No Camera component found!");
        }
    }
}
