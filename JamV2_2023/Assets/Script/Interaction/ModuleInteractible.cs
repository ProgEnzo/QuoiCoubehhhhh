using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ModuleInteractible : MonoBehaviour
{
    public ModuleInteractibleSO moduleInteractibleSO;

    [Space]
    [SerializeField]private string moduleName;
    [SerializeField]private GameObject mesh;
    [SerializeField]private float moduleMovingSpeed;
    [SerializeField]private float timeToCook;

    [SerializeField] private bool Cuisine;
    public List<GameObject> ElementVerticalMouvemet;
    
    
    
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
                        isOn = true;
                    }
                }
                else
                {
                    if (other.GetComponent<InteractSphere>().playercontroller.isInteractingg)
                    {
                        gameObject.transform.parent = null;
                        isOn = false;
                    }
                }
            }
        }
    }
    
}
