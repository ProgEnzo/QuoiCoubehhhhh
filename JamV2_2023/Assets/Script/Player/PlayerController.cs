using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using NaughtyAttributes;
using Objects.Target;


[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    public bool isWalking;
    
    [SerializeField]
    public float maxSpeed = 2.0f;
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

    public Transform waypointInteract; // endroit ou interact object doit se situer quand player tient l'ojbet

    public GameObject objectInHand;
    public Rigidbody objectInHandRB;
    public float shootForce = 10;
    
    [Tooltip("Porté pour detecter les cibles possbiles"), SerializeField] private float targetDectionRange;
    [Tooltip("Porté pour detecter les cibles possbiles"), SerializeField] private float maxAngleFromTarget;
    [SerializeField] private float maxHeightDifPlayerTarget;
    [SerializeField] private Target lockedTarget;
    
    [SerializeField] private ParticleSystem vfxuppuissance;
    [SerializeField] private ParticleSystem vfxupspeed;
    [SerializeField] private ParticleSystem vfxweak;
    [SerializeField] public ParticleSystem vfxslow;
    [SerializeField] private ParticleSystem vfxupdash;
    
    public CapsuleCollider playerInteract;
    
    private Target LockedTarget
    {
        get => lockedTarget;

        set
        { 
            if (lockedTarget) lockedTarget.ManagerLockUI(false);
            
            lockedTarget = value;
            if (lockedTarget) lockedTarget.ManagerLockUI(value);
        }
    }
      
    public List<Target> targets = new();
    private bool _islockedTargetNull;
    
    private bool _waitingForShoot;
    private bool _canGather;
    public bool isNeedingSnowBall;


    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        isDashingReload = true;
        isNeedingSnowBall = true;
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
            //isInteracting.GetComponent<InteractSphere>().StartCoroutine(isInteracting.GetComponent<InteractSphere>().start());
            isInteractingg = true;
        }
        else
        {
            isInteractingg = false;
        }
    }

    public bool isEat;
    public void OnEat(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            //isInteracting.GetComponent<InteractSphere>().StartCoroutine(isInteracting.GetComponent<InteractSphere>().start());
            isEat = true;
        }
        else
        {
            isEat = false;
        }
    }

    public bool isThrow;
    public void OnThrow(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            //isInteracting.GetComponent<InteractSphere>().StartCoroutine(isInteracting.GetComponent<InteractSphere>().start());
            isThrow = true;
        }
        else
        {
            isThrow = false;
        }
    }
    
    
    
    private void LoockingForTarget()
    {
        if (!objectInHand) return;
         
        Vector3 playerPos = transform.position;
        Vector3 forward = transform.forward;
         
        foreach (var t in targets)
        {
            // Si la cible est trop loins OU
            // que elle est en dehors de l'angle de dection, la skip et passé à la prochaine OU
            // que la différence de hauteur entre la cible est le joueur est trop grande
            if (
                Vector3.Distance(playerPos, t.transform.position) > targetDectionRange ||
                Vector3.Angle(new Vector3(t.transform.position.x,0, t.transform.position.z) 
                              - new Vector3(playerPos.x,0, playerPos.z), forward) > maxAngleFromTarget || 
                transform.position.y - t.transform.position.y > maxHeightDifPlayerTarget || 
                transform.position.y - t.transform.position.y < -maxHeightDifPlayerTarget)
            {
                continue;
            }

            if (_islockedTargetNull) LockedTarget = t;
            else
            {
                float angleFormCurrentT = Vector3.Angle(
                    new Vector3(t.transform.position.x,0, t.transform.position.z)
                    - new Vector3(playerPos.x,0, playerPos.z),forward);
               
                float angleFromCurrentLockedTarget = Vector3.Angle(
                    new Vector3(LockedTarget.transform.position.x, 0, LockedTarget.transform.position.z) 
                    - new Vector3(playerPos.x,0, playerPos.z), forward);

                if (angleFormCurrentT < angleFromCurrentLockedTarget) LockedTarget = t;
            }
        }
    }
    
    
    public void ThrowSnowBall()
    {
        objectInHandRB = objectInHand.GetComponent<Rigidbody>();
        objectInHandRB.constraints = RigidbodyConstraints.FreezeRotation;
        objectInHandRB.useGravity = true;
        objectInHand.transform.parent = null;
        objectInHand = null;

        // Par sécurité
        if (!objectInHandRB)
        {
            Debug.LogError("snowball rb is null");
            return; 
        }
         
        Vector3 playerPos = transform.position;
        Vector3 forward = transform.forward;
         
        // Tire de boule de neige
        if (LockedTarget)
        {
            Vector3 topDownPlayerPos = new Vector3(playerPos.x, 0, playerPos.z);
            Vector3 lockedTargetPos = LockedTarget.transform.position;
            Vector3 topDownLockedTargetPos = new Vector3(lockedTargetPos.x, 0, lockedTargetPos.z);
            
            if ( Vector3.Distance(playerPos, LockedTarget.transform.position) <= targetDectionRange 
                 &&  Vector3.Angle(topDownLockedTargetPos - topDownPlayerPos, forward) <= maxAngleFromTarget) 
            {
                var dir = LockedTarget.transform.position - transform.position;
                ShootSnowball(dir.normalized);
            }
            else
            {
                ShootSnowball((transform.forward + (transform.up / 4f)).normalized);
            }
        }
        else 
        {
            ShootSnowball((transform.forward + (transform.up / 4f)).normalized);
        }

        objectInHandRB = null;
        LockedTarget = null;
        _waitingForShoot = false;
        isNeedingSnowBall = true;
    }
    
    private void ShootSnowball(Vector3 dir)
    {
        objectInHandRB.AddForce(dir * shootForce, ForceMode.Impulse);
    }

    void FixedUpdate()
    {
        Movement();
        LoockingForTarget();
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
        vfxupdash.Play();
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
