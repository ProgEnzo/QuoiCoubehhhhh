using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public PlayerController playerController;
    public Animator animator;

    public GameObject p1;
    public GameObject p2;
    void Start()
    {
        //animator = GetComponentInChildren<Animator>(true);

        switch (PlayerManager.instance.count)
        {
            case 1 :
                p2.SetActive(false);
                animator = p1.GetComponent<Animator>();
                break;
            
            case 2:
                p1.SetActive(false);
                animator = p2.GetComponent<Animator>();
                break;
        }
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
        
        if (playerController.isCuisine)
        {
            animator.SetBool("Cuisine", true);
        }
        else
        {
            animator.SetBool("Cuisine", false);
        }

    }
}
