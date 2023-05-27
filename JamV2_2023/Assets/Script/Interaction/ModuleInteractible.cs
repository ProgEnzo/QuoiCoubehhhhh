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

    [SerializeField] private bool verticalMovement;
    public List<GameObject> ElementVerticalMouvemet;
    void Start()
    {
        SecureSO();
        if (verticalMovement)
        {
            foreach (var i in ElementVerticalMouvemet)
            {
                i.SetActive(true);
            }
        }
        else
        {
            foreach (var i in ElementVerticalMouvemet)
            {
                i.SetActive(false);
            }
            GetComponent<BoxCollider>().isTrigger = true;
        }
    }

    void SecureSO()
    {
        moduleName = moduleInteractibleSO.moduleName;
        mesh = moduleInteractibleSO.mesh;
        moduleMovingSpeed = moduleInteractibleSO.moduleMovingSpeed;
        timeToCook = moduleInteractibleSO.timeToCook;
        verticalMovement = moduleInteractibleSO.verticalMovement;

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
        if (verticalMovement)
        {
            
        }
        else
        {
            
        }
    }

    void TakeElement()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("ici");
            if (other.GetComponent<PlayerController>().isInteractingg)
            {
                gameObject.transform.parent = other.GetComponent<PlayerController>().transform;
            }
        }
    }
}
