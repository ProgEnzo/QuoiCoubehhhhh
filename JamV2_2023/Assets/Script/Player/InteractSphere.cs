using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractSphere : MonoBehaviour
{

    public PlayerController playercontroller;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        /*if (playercontroller.isInteractingg)
        {
            GetComponent<SphereCollider>().enabled = true;
        }
        else
        {
            GetComponent<SphereCollider>().enabled = false;
        }*/
    }
    
    
    private void OnDrawGizmos()
    {

        if (playercontroller.isInteractingg)
        {
            Gizmos.color = Color.magenta;
//            Gizmos.DrawWireSphere(transform.position, GetComponent<SphereCollider>().radius);
        }
    }
}
