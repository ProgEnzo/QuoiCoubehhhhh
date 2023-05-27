using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    
    

    public int count;
    
    

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


    
}
