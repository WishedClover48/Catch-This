using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SetModel : MonoBehaviourPunCallbacks
{
    [SerializeField] private Animator animator;
    [Header("Models")]
    [SerializeField] private Model[] modelList;
    [Space]
    [SerializeField] private Skins defaultModel = 0;

    private void Start()
    {
        var selectedSkin = defaultModel;
        var owner = photonView?.Owner;

        if (owner != null && owner.CustomProperties.TryGetValue("Skin", out var raw))
        {
            selectedSkin = raw switch
            {
                Skins s => s,
                int i => (Skins)i,
                _ => selectedSkin
            };
        }

        var m = FindModel(selectedSkin);
        var pos = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
        Instantiate(m.model, pos, transform.rotation, transform);
        animator.avatar = m.avatar;
    }
    
    private Model FindModel(Skins skin)
    {
        foreach (var m in modelList)
        {
            if (m.name == skin)
                return m;
        }
        
        foreach (var m in modelList)
        {
            if (m.name == defaultModel)
                return m;
        }
        
        return modelList[0];
    }
    
    [Serializable]
    public struct Model
    {
        public Skins name; //Skins is an Enum, each skin has its own value.
        public GameObject model;
        public Avatar avatar;
    }
}
