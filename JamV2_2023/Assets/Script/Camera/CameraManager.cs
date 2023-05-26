using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    public CinemachineTargetGroup targetGroup;


    public static CameraManager instance;
    private void Awake()
    {
        if (instance)
        {
            instance = this;
        }
    }

    void Start()
    {
        
    }
    
    void Update()
    {
        
    }


    public void AddPlayer()
    {
        Transform p = GameObject.Find("Player(Clone)").transform;
        p.name = "Player";
        targetGroup.AddMember(p, 1,1); 
    }
}
