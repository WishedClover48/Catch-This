using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon.StructWrapping;
using System;

public class SkinAssigner : MonoBehaviourPunCallbacks
{
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private MeshRenderer meshRenderer;
    
    [Header("Skins")]
    [SerializeField] private SkinPair[] skinsList;
    [SerializeField] private Skins defaultSkin = 0; 
    
    private readonly Dictionary<Skins, SkinPair> _skins = new();

    private void Awake()
    {
        if (!meshFilter)   meshFilter   = GetComponent<MeshFilter>();
        if (!meshRenderer) meshRenderer = GetComponent<MeshRenderer>();
        
        BuildDictionary();
        ApplySkin();
    }

    private void BuildDictionary()
    {
        _skins.Clear();
        foreach (var sk in skinsList)
        {
            _skins.TryAdd(sk.skin, sk);
        }
    }
    private void ApplySkin()
    {
        var skin = defaultSkin;
        var owner = photonView?.Owner;

        if (owner != null && owner.CustomProperties.TryGetValue("Skin", out var raw))
        {
            skin = raw switch
            {
                Skins s => s,
                int i => (Skins)i,
                _ => skin
            };
        }
        
        if (!_skins.TryGetValue(skin, out var pair) &&
            !_skins.TryGetValue(defaultSkin, out pair))
        {
            return; 
        }

        meshFilter.sharedMesh = pair.mesh;
        meshRenderer.sharedMaterial = pair.material;
    }
    
    [Serializable]
    public struct SkinPair
    {
        public Skins skin; //Skins is an Enum, each skin has its own value.
        public GameObject skinObject;
        public Mesh mesh;
        public Material material;
    }
}


