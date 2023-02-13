using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCntrl : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float cameraSpeed;
    [SerializeField] private PlayerData player;
    [SerializeField] private Transform cameraObservePos;
    [SerializeField] private Transform cameraFollowPos;

    private CharacterController controller;
    private Animator animator;

    private PlayerInputActions playerInputAction;
    private InputAction movement;

    Vector2 move;

    public void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    private void Update() 
    {
        move = movement.ReadValue<Vector2>();

        player.movement(move, controller, Time.deltaTime);
    }

    void LateUpdate() 
    {
        if (move.y <= 0) {
            playerCamera.transform.position = Vector3.Lerp(playerCamera.transform.position, cameraObservePos.position, cameraSpeed * Time.deltaTime);
        } else {
            playerCamera.transform.position = Vector3.Lerp(playerCamera.transform.position, cameraFollowPos.position, cameraSpeed * Time.deltaTime);
        }

    }

    private void DoFire(InputAction.CallbackContext context)
    {
        Debug.Log("DoFire ...");
    }

    private void Awake() 
    {
        playerInputAction = new PlayerInputActions();
    }

    private void OnEnable()
    {
        movement = playerInputAction.Player.Movement;
        movement.Enable();

        playerInputAction.Player.Fire.performed += DoFire;
        playerInputAction.Player.Fire.Enable();
    }

    private void OnDisable() 
    {
        movement.Disable();
        playerInputAction.Player.Fire.Disable();
    }
}

public enum PlayerState
{
    IDLE,
    MOVING
}
