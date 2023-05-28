using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ModuleInteractible : MonoBehaviour
{
    public ModuleInteractibleSO moduleInteractibleSO;

    public BoxCollider box1;
    public BoxCollider box2;

    public GameObject waypointCuisine;

    public float movingModuleSpeed = 5;

    public bool occupied;

    [Space]
    [Space]
    [SerializeField]private string moduleName;
    [SerializeField]private GameObject mesh;
    [SerializeField]private float moduleMovingSpeed;
    [SerializeField]private float timeToCook;

    [SerializeField] private bool Cuisine;
    public List<GameObject> ElementVerticalMouvemet;
    
    
    
    public float effectDuration;
    public float forceMultiplier;
    
    public float speedMultiplierBonus;
    public float speedMultiplierMalus;
    
    public bool canMoveModule;
    public float modifiedMovingModuleSpeedBonus;
    public float modifiedMovingModuleSpeedMalus;

    public float modifiedTimeToCook;
    public bool canCookImmediately;
    [SerializeField] private ParticleSystem vfxcooked;
    [SerializeField] private ParticleSystem vfxhit;

    public MeshRenderer[] meshRenderers;
    void Start()
    {
        SecureSO();
        if (Cuisine)
        {
            var a = GetComponent<Rigidbody>();
                
            a.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | 
                            RigidbodyConstraints.FreezeRotation ;

        }
        else
        {
            /*foreach (var i in ElementVerticalMouvemet)
            {
                i.SetActive(false);
            }*/
            meshRenderers = gameObject.GetComponentsInChildren<MeshRenderer>();
        }
        GetComponent<BoxCollider>().isTrigger = true;
    }

    void SecureSO()
    {
        moduleName = moduleInteractibleSO.moduleName;
        mesh = moduleInteractibleSO.mesh;
        moduleMovingSpeed = moduleInteractibleSO.moduleMovingSpeed;
        timeToCook = moduleInteractibleSO.timeToCook;
        Cuisine = moduleInteractibleSO.verticalMovement;

        /*var selection = Selection.gameObjects;

        for (var i = selection.Length; i < 0; i++)
        {
            var selected = selection[i];
            GameObject newObject;
            
            newObject = (GameObject)PrefabUtility.InstantiatePrefab(mesh);
            Undo.RegisterCreatedObjectUndo(newObject, "Replace with Prefab");
            newObject.transform.parent = selected.transform.parent;
            newObject.transform.localPosition = selected.transform.localPosition;
            newObject.transform.localRotation = selected.transform.localRotation;
            newObject.transform.SetSiblingIndex(selected.transform.GetSiblingIndex());
            Undo.DestroyObjectImmediate(selected);
        }*/

        effectDuration = moduleInteractibleSO.effectDuration;
        forceMultiplier = moduleInteractibleSO.forceMultiplier;
        speedMultiplierBonus = moduleInteractibleSO.speedMultiplierBonus;
        speedMultiplierMalus = moduleInteractibleSO.speedMultiplierMalus;
        canMoveModule = moduleInteractibleSO.canMoveModule;
        modifiedMovingModuleSpeedBonus
            = moduleInteractibleSO.movingModuleSpeedBonus;
        modifiedMovingModuleSpeedMalus = moduleInteractibleSO.movingModuleSpeedMalus;
        modifiedTimeToCook = moduleInteractibleSO.modifiedTimeToCook;
        canCookImmediately = moduleInteractibleSO.canCookImmediately;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void TakeElement()
    {
        
    }

    public Transform parent;
    public bool isOn;
    public bool thrown;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Interact"))
        {
            if (Cuisine)
            {
                if (!isOn)
                {
                    if (other.GetComponent<InteractSphere>().playercontroller.isInteractingg && other.GetComponent<InteractSphere>().playercontroller.objectInHand == null) // déplace la cuisine
                    {
                        player = other.GetComponent<InteractSphere>().playercontroller;
                        if (!other.GetComponent<InteractSphere>().playercontroller.canMoveModule)
                        {
                            other.GetComponent<InteractSphere>().playercontroller.maxSpeed = 2.5f;
                        }
                        else
                        {
                            other.GetComponent<InteractSphere>().playercontroller.maxSpeed =
                                other.GetComponent<InteractSphere>().playercontroller.movingModuleSpeed;
                        }
                        
                        gameObject.transform.parent = other.GetComponent<InteractSphere>().playercontroller.transform; // set child
                        gameObject.transform.position = other.GetComponent<InteractSphere>().playercontroller.waypointInteractDeplacementCuisine.transform.position; // set position
                        other.GetComponent<InteractSphere>().playercontroller.objectInHand = gameObject; // dans la main 
                        other.GetComponent<InteractSphere>().playercontroller.objectInHand.GetComponent<Rigidbody>()
                            .constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | 
                                           RigidbodyConstraints.FreezeRotation ;
                        other.GetComponent<InteractSphere>().playercontroller.isInteractingg = false;
                        isOn = true;
                        other.GetComponent<InteractSphere>().playercontroller.canRotatePlayer = false;
                        
                    }
                }
                else
                {
                    if (other.GetComponent<InteractSphere>().playercontroller.isInteractingg)// repose la cuisine
                    {
                        player.objectInHand = null;
                        other.GetComponent<InteractSphere>().playercontroller.maxSpeed = 5;

                        other.GetComponent<InteractSphere>().playercontroller.isInteractingg = false;
                        gameObject.transform.parent = null;
                        other.GetComponent<InteractSphere>().playercontroller.canRotatePlayer = true;
                        isOn = false;
                        
                    }
                }
            }
            else
            {
                if (!isOn)
                {
                    if (other.GetComponent<InteractSphere>().playercontroller.isInteractingg && !iscooking) // prend alliment
                    {
                        player = other.GetComponent<InteractSphere>().playercontroller;
                        gameObject.transform.parent = other.GetComponent<InteractSphere>().playercontroller.transform; // set child
                        gameObject.transform.position = other.GetComponent<InteractSphere>().playercontroller.waypointInteract.transform.position; // set position
                        other.GetComponent<InteractSphere>().playercontroller.objectInHand = gameObject; // dans la main 
                        other.GetComponent<InteractSphere>().playercontroller.objectInHand.GetComponent<Rigidbody>()
                            .constraints = RigidbodyConstraints.FreezeAll;
                        other.GetComponent<InteractSphere>().playercontroller.isInteractingg = false;
                        
                        isOn = true;
                    }
                }
                else 
                {
                    if (other.GetComponent<InteractSphere>().playercontroller.isThrow && !iscooking) // lance alliment
                    {
                        gameObject.transform.parent = null;
                        other.GetComponent<InteractSphere>().playercontroller.objectInHand.GetComponent<Rigidbody>()
                            .constraints = RigidbodyConstraints.None;
                        other.GetComponent<InteractSphere>().playercontroller.ThrowSnowBall();
                        other.GetComponent<InteractSphere>().playercontroller.isThrow = false;
                        other.GetComponent<InteractSphere>().playercontroller.playerInteract.enabled = false;
                        isOn = false;
                        thrown = true;
                    }
                    
                    if (other.GetComponent<InteractSphere>().playercontroller.isEat && !iscooking) // mange aliment
                    {
                        gameObject.transform.parent = null;
                        PowerSelf(other.GetComponent<InteractSphere>().playercontroller);
                        //Destroy(gameObject);
                        box1.enabled = false;
                        box2.enabled = false;
                        foreach (var x in meshRenderers)
                        {
                            x.enabled = false;
                            vfxcooked.Stop();
                        }
                        GetComponent<MeshRenderer>().enabled = false;
                        isOn = false;
                    }

                    if (other.GetComponent<InteractSphere>().playercontroller.isInteractingg && !iscooking)
                    {
                        other.GetComponent<InteractSphere>().playercontroller.objectInHand.GetComponent<Rigidbody>()
                            .constraints = RigidbodyConstraints.None;
                        gameObject.transform.parent = null;
                        player.objectInHand = null;
                        other.GetComponent<InteractSphere>().playercontroller.isInteractingg = false;
                        isOn = false;
                        Debug.Log("qsdqzd");
                    }
                }
            }
        }

        if (other.CompareTag("PlayerInteract") && thrown)
        {
            vfxhit.Play();
            PowerTo(other.GetComponent<InteractSphere>().playercontroller);
            other.GetComponent<InteractSphere>().playercontroller.life--;
            player.vfxslow.Play();
            thrown = false;
        }

        if (other.CompareTag("Cuisine") && !other.GetComponent<ModuleInteractible>().occupied && !isCoocked)
        {
            other.GetComponent<ModuleInteractible>().occupied = true;
            Debug.Log("cuisin");
            
            Debug.Log("interagie en cuisine");
            gameObject.transform.parent = null;
            gameObject.transform.position = other.GetComponent<ModuleInteractible>().waypointCuisine.transform.position;
            /*other.GetComponent<InteractSphere>().playercontroller.objectInHand.GetComponent<Rigidbody>()
                .constraints = RigidbodyConstraints.None;*/
            iscooking = true;
            player.canWalk = false;
            player.isCuisine = true;
            
            StartCoroutine(cook(other.GetComponent<ModuleInteractible>()));

            /* if (GetComponent<ModuleInteractible>().GetComponent<InteractSphere>().playercontroller.isInteractingg) // pose aliment et cuisinent 
             {
                 
             }*/
        }
    }

    public PlayerController player;
    public bool isCoocked;
    public bool iscooking;
    
    IEnumerator cook(ModuleInteractible other)
    {
        player.canRotatePlayer = false;
        yield return new WaitForSeconds(3);
        
        gameObject.transform.parent = player.transform;
        gameObject.transform.position = player.waypointInteract.transform.position;
        player.objectInHand = gameObject; // dans la main 
        player.objectInHand.GetComponent<Rigidbody>()
            .constraints = RigidbodyConstraints.FreezeAll;
        player.isInteractingg = false;

        player.canRotatePlayer = true;
        player.canWalk = true;
        iscooking = false;
        isCoocked = true;
        vfxcooked.Play();
        player.isCuisine = false;
        
        other.occupied = false;
    }


    void PowerSelf(PlayerController other)
    {
        switch (moduleName)
        {
            case "Chromodium":
                ChromodiumSelf(other);
                break;
            case "Crokaium":
                CrokaiumSelf(other);
                break;
            case "Prepinrium":
                PepinriumSelf(other);
                break;
            case "Tremium":
                TremiumSelf(other);
                break;
        }
    }
    void ChromodiumSelf(PlayerController other)
    {
        StartCoroutine(chromodiumSelf(other));
    }
    IEnumerator chromodiumSelf(PlayerController other)
    {
        if (isCoocked)
        {
            var keepValueSpeed = other.maxSpeed;
            other.maxSpeed *= speedMultiplierBonus;
            yield return new WaitForSeconds(effectDuration*1.5f);
            other.maxSpeed = keepValueSpeed;
        }
        else
        {
            var keepValueSpeed = other.maxSpeed;
            other.maxSpeed *= speedMultiplierBonus;
            yield return new WaitForSeconds(effectDuration);
            other.maxSpeed = keepValueSpeed;
        }
    }
    
    void CrokaiumSelf(PlayerController other)
    {
        StartCoroutine(crokaiumSelf(other));
    }
    IEnumerator crokaiumSelf(PlayerController other)
    {
        if (isCoocked)
        {
            var keepValueSpeed = other.maxSpeed;
            other.maxSpeed *= speedMultiplierBonus;
            yield return new WaitForSeconds(effectDuration*1.5f);
            other.maxSpeed = keepValueSpeed;
        }
        else
        {
            var keepValueSpeed = other.maxSpeed;
            other.maxSpeed *= speedMultiplierBonus;
            yield return new WaitForSeconds(effectDuration);
            other.maxSpeed = keepValueSpeed;
        }
    }
    
    void PepinriumSelf(PlayerController other)
    {
        StartCoroutine(pepinriumSelf(other));
    }
    IEnumerator pepinriumSelf(PlayerController other)
    {
        if (isCoocked)
        {
            other.canMoveModule = canMoveModule;
            var keepValueSpeed = other.movingModuleSpeed;
            other.movingModuleSpeed = modifiedMovingModuleSpeedBonus;
            Debug.Log(movingModuleSpeed);
            yield return new WaitForSeconds(effectDuration*1.5f);
            other.canMoveModule = false;
            other.movingModuleSpeed = keepValueSpeed;
        }
        else
        {
            other.canMoveModule = canMoveModule;
            var keepValueSpeed = other.movingModuleSpeed;
            other.movingModuleSpeed = modifiedMovingModuleSpeedBonus;
            Debug.Log(movingModuleSpeed);
            yield return new WaitForSeconds(effectDuration);
            other.canMoveModule = false;
            other.movingModuleSpeed = keepValueSpeed;
        }
    }
    
    
    void TremiumSelf(PlayerController other)
    {
        canCookImmediately = false;
        //StartCoroutine(tremiumSelf(other));
    }


        //-------------------------------------

    void PowerTo(PlayerController other)
    {   
        switch (moduleName)
        {
            case "Chromodium":
                ChromodiumTo(other);
                break;
            case "Crokaium":    
                CrokaiumTo(other);
                break;
            case "Prepinrium":
                PepinriumTo(other);
                break;
            case "Tremium":
                TremiumTo(other);
                break;
        }
    }

    void ChromodiumTo(PlayerController other)
    {
        
    }
    
    void CrokaiumTo(PlayerController other)
    {
        StartCoroutine(crokaiumTo(other));
    }
    
    IEnumerator crokaiumTo(PlayerController other)
    {
        if (isCoocked)
        {
            var keepValueSpeed = other.maxSpeed;
            other.maxSpeed *= speedMultiplierMalus;
            yield return new WaitForSeconds(effectDuration*1.5f);
            other.maxSpeed = keepValueSpeed;
        }
        else
        {
            var keepValueSpeed = other.maxSpeed;
            other.maxSpeed *= speedMultiplierMalus;
            yield return new WaitForSeconds(effectDuration);
            other.maxSpeed = keepValueSpeed;
        }
        
    }
    
    void PepinriumTo(PlayerController other)
    {
        StartCoroutine(pepinriumTo(other));
    }

    IEnumerator pepinriumTo(PlayerController other)
    {
        if (isCoocked)
        {
            other.canMoveModule = canMoveModule;
            var keepValueSpeed = other.movingModuleSpeed;
            other.movingModuleSpeed = modifiedMovingModuleSpeedMalus;
            Debug.Log(movingModuleSpeed);
            yield return new WaitForSeconds(effectDuration*1.5f);
            other.canMoveModule = false;
            other.movingModuleSpeed = keepValueSpeed;
        }
        else
        {
            other.canMoveModule = canMoveModule;
            var keepValueSpeed = other.movingModuleSpeed;
            other.movingModuleSpeed = modifiedMovingModuleSpeedMalus;
            Debug.Log(movingModuleSpeed);
            yield return new WaitForSeconds(effectDuration);
            other.canMoveModule = false;
            other.movingModuleSpeed = keepValueSpeed;
        }
        
    }
    
    void TremiumTo(PlayerController other)
    {
        StartCoroutine(tremiumTo(other));
    }
    
    IEnumerator tremiumTo(PlayerController other)
    {
        if (isCoocked)
        {
            var keepValueSpeed = timeToCook;
            timeToCook = modifiedTimeToCook;
            yield return new WaitForSeconds(effectDuration*1.5f);
            timeToCook = keepValueSpeed;
        }
        else
        {
            var keepValueSpeed = timeToCook;
            timeToCook = modifiedTimeToCook;
            yield return new WaitForSeconds(effectDuration);
            timeToCook = keepValueSpeed;
        }
    }
    

}
