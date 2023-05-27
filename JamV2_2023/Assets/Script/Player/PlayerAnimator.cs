using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public PlayerController playerController;
    public Animator animator;
    void Start()
    {
        GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.isWalking)
        {
            animator.SetBool("walk", true);
        }
        else
        {
            animator.SetBool("walk", false);
        }
        
        if (playerController.isInteractingg)
        {
            animator.SetBool("grab", true);
        }
        else
        {
            animator.SetBool("grab", false);
        }
        
        
    }
}
