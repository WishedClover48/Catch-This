using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon.StructWrapping;
using System;

public class SkinAssigner : MonoBehaviourPunCallbacks
{
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private Material mat;
    [SerializeField] SkinPair[] skinsList;
    private Dictionary<Skins, Mesh> skinsDictionary = new Dictionary<Skins, Mesh>();

    private void Awake()
    {
        Photon.Realtime.Player goOwner = photonView.Owner;

        foreach(SkinPair sk in skinsList)
        {
            skinsDictionary.Add(sk.Skin, sk.Mesh);
        }

        goOwner.CustomProperties.TryGetValue<object>("Skin", out object value);

        if (value == null)
            value = 0;
        var skin = (Skins)value;
        skinsDictionary.TryGetValue(skin, out var boca);

        meshFilter.mesh = boca;
        
    }


    [Serializable]
    struct SkinPair
    {
        public Skins Skin;
        public Mesh Mesh;
        public Material Material;
    }
}


