using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ModulesInteractibles", order = 0, fileName = "New ModulesInteractibles")]
public class ModuleInteractibleSO : ScriptableObject
{
    public int id;
    public string moduleName;
    public GameObject mesh;
    
    
    public float moduleMovingSpeed;
    public float timeToCook;

    public bool verticalMovement;
    
    
    [Space]
    
    public float effectDuration;
    public float forceMultiplier;
    
    public float speedMultiplierBonus;
    public float speedMultiplierMalus;
    
    public bool canMoveModule;
    public float movingModuleSpeedBonus;
    public float movingModuleSpeedMalus;

    public float modifiedTimeToCook;
    public bool canCookImmediately; 
}
