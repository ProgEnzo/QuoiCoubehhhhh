using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    
    private List<PlayerInput> players = new List<PlayerInput>();
    public CinemachineTargetGroup targetGroup;
    public List<Transform> placesToSpawns;

    private PlayerInputManager _playerInputManager;
    void Awake()
    {
        _playerInputManager = FindObjectOfType<PlayerInputManager>();
    }

    private void OnEnable()
    {
        _playerInputManager.onPlayerJoined += AddPlayer;
    }

    private void OnDisable()
    {
        _playerInputManager.onPlayerJoined -= AddPlayer;
    }


    public void AddPlayer(PlayerInput player)
    {
        players.Add(player);
        
        /*Transform playerParent = GameObject.FindWithTag("Player").transform;
        playerParent.name = "Player";*/
        
        player.transform.position = placesToSpawns[players.Count-1].position;
        player.GetComponent<CharacterController>().enabled = true;

        targetGroup.AddMember(player.transform, 1,1);
    }
}
