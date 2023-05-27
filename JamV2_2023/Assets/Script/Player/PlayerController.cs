using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using NaughtyAttributes;


[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    public bool isWalking;
    
    [SerializeField]
    private float maxSpeed = 2.0f;
    [SerializeField]
    private float dashSpeed = 2.0f;
    [SerializeField] private float timeToDash = 2.0f;
    [SerializeField] private float dashDuration = 2.0f;
    [SerializeField] private bool isDashing;
    [SerializeField] private bool isDashingReload;
    
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;

    [SerializeField] private GameObject directionOfPlayer;
    private Vector3 movementInput = Vector3.zero;


    public GameObject isInteracting;
    public bool isInteractingg;

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        isDashingReload = true;
        isInteracting.SetActive(false); 
    }


    public void OnMove(InputAction.CallbackContext ctx)
    {
        movementInput = ctx.ReadValue<Vector2>();
    }
    
    public void OnDash(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            isDashing = true;
        }
        else
        {
            isDashing = false;
        }
    }

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            isInteracting.SetActive(true);
            isInteracting.GetComponent<InteractSphere>().StartCoroutine(isInteracting.GetComponent<InteractSphere>().start());
            isInteractingg = true;
        }
        else
        {
            isInteractingg = false;
        }
    }

    private void Update()
    {
        
    }

    void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector3 move = new Vector3(movementInput.x, 0, movementInput.y);
        controller.Move(move * Time.deltaTime * maxSpeed);

        if (move != Vector3.zero)
        {
            //gameObject.transform.forward = move;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(move), 0.1f);
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }

        if (isDashing && isDashingReload)
        {
            Dash();
        }
        /*else if (isDashing && !isDashingReload)
        {
            isDashing = false;
        }*/
        

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    void Dash()
    {
        //isDashing = false;
        isDashingReload = false;
        StartCoroutine(DashDuration());
        StartCoroutine(DashReload());
    }

    IEnumerator DashDuration()
    {
        var keepSpeedValue = maxSpeed;
        maxSpeed = dashSpeed;
        Physics.IgnoreLayerCollision(6,7);
        yield return new WaitForSeconds(dashDuration);
        Physics.IgnoreLayerCollision(6,7, false);
        maxSpeed = keepSpeedValue;
    }

    IEnumerator DashReload()
    {
        yield return new WaitForSeconds(timeToDash);
        isDashingReload = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, directionOfPlayer.transform.position);
    }
}
