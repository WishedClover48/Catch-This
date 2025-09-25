using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialRandomizer : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material[] materials;

    private void Start()
    {
        if (meshRenderer == null) meshRenderer = GetComponent<MeshRenderer>();

        if (materials != null && materials.Length > 0)
        {
            int index = Random.Range(0, materials.Length);
            meshRenderer.material = materials[index];
        }
        else
        {
            Debug.LogWarning("No materials assigned!");
        }
    }
}
