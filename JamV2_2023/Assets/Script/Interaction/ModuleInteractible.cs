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

    public float movingModuleSpeed = 5;
    
    
    
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
    public float modifiedMovingModuleSpeed;

    public float modifiedTimeToCook;
    public bool canCookImmediately; 
    
    void Start()
    {
        SecureSO();
        if (Cuisine)
        {
            /*foreach (var i in ElementVerticalMouvemet)
            {
                i.SetActive(true);
            }*/
        }
        else
        {
            /*foreach (var i in ElementVerticalMouvemet)
            {
                i.SetActive(false);
            }*/
            
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

        var selection = Selection.gameObjects;

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
        }

        effectDuration = moduleInteractibleSO.effectDuration;
        forceMultiplier = moduleInteractibleSO.forceMultiplier;
        speedMultiplierBonus = moduleInteractibleSO.speedMultiplierBonus;
        speedMultiplierMalus = moduleInteractibleSO.speedMultiplierMalus;
        canMoveModule = moduleInteractibleSO.canMoveModule;
        modifiedMovingModuleSpeed = moduleInteractibleSO.movingModuleSpeed;
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
                
            }
            else
            {
                if (!isOn)
                {
                    if (other.GetComponent<InteractSphere>().playercontroller.isInteractingg)
                    {
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
                    if (other.GetComponent<InteractSphere>().playercontroller.isThrow)
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
                    
                    if (other.GetComponent<InteractSphere>().playercontroller.isEat)
                    {
                        gameObject.transform.parent = null;
                        PowerSelf(other.GetComponent<InteractSphere>().playercontroller);
                        //Destroy(gameObject);
                        box1.enabled = false;
                        box2.enabled = false;
                        GetComponent<MeshRenderer>().enabled = false;
                        isOn = false;
                    }
                }
            }
        }

        if (other.CompareTag("PlayerInteract") && thrown)
        {
            PowerTo(other.GetComponent<InteractSphere>().playercontroller);
            thrown = false;
        }
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
            case "Pepinrium":
                PepinriumSelf(other);
                break;
            case "Tremium":
                TremiumSelf(other);
                break;
        }
    }
    void ChromodiumSelf(PlayerController other)
    {
        //StartCoroutine(chromodiumSelf(other));
    }
    IEnumerator chromodiumSelf(PlayerController other)
    {
        var keepValueSpeed = other.maxSpeed;
        other.maxSpeed *= speedMultiplierBonus;
        yield return new WaitForSeconds(effectDuration);
        other.maxSpeed = keepValueSpeed;
    }
    
    void CrokaiumSelf(PlayerController other)
    {
        StartCoroutine(crokaiumSelf(other));
    }
    IEnumerator crokaiumSelf(PlayerController other)
    {
        var keepValueSpeed = other.maxSpeed;
        other.maxSpeed *= speedMultiplierBonus;
        yield return new WaitForSeconds(effectDuration);
        other.maxSpeed = keepValueSpeed;
    }
    
    void PepinriumSelf(PlayerController other)
    {
        StartCoroutine(pepinriumSelf(other));
    }
    IEnumerator pepinriumSelf(PlayerController other)
    {
        var keepValueSpeed = movingModuleSpeed;
        movingModuleSpeed = modifiedMovingModuleSpeed;
        yield return new WaitForSeconds(effectDuration);
        movingModuleSpeed = keepValueSpeed;
    }
    
    
    void TremiumSelf(PlayerController other)
    {
        canCookImmediately = true;
        //StartCoroutine(tremiumSelf(other));
    }


        //-------------------------------------.

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
            case "Pepinrium":
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
        var keepValueSpeed = other.maxSpeed;
        other.maxSpeed *= speedMultiplierMalus;
        yield return new WaitForSeconds(effectDuration);
        other.maxSpeed = keepValueSpeed;
    }
    
    void PepinriumTo(PlayerController other)
    {
        
    }
    
    void TremiumTo(PlayerController other)
    {
        
    }
    

}
