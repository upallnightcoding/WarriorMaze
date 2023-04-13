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
    [SerializeField] private float moveSpeed;
    [SerializeField] private Transform cameraObservePos;
    [SerializeField] private Transform cameraFollowPos;

    private CharacterController controller;
    private Animator animator;

    private PlayerInputActions playerInputAction;
    private InputAction movementInputAction;
    private InputAction mouseInputAction;

    private Vector3 deltaCameraPos;

    private Vector3 prevMousePosition = Vector3.zero;

    public void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        deltaCameraPos = playerCamera.transform.position - transform.position;
    }

    private void Update() 
    {
        Vector2 arrowKeys = movementInputAction.ReadValue<Vector2>();
        Vector2 mouse = mouseInputAction.ReadValue<Vector2>();

        animator.SetFloat("SpeedX", arrowKeys.x);
        animator.SetFloat("SpeedZ", arrowKeys.y);

        TopDownCntrl(arrowKeys, mouse, Time.deltaTime);

        prevMousePosition = mouse;
    }

    void LateUpdate() 
    {
        playerCamera.transform.position = deltaCameraPos + transform.position;
    }

    private void TopDownCntrl(Vector2 arrowKeys, Vector2 mousePosition, float dt) {
        Vector3 movementDirection = new Vector3(arrowKeys.x, 0.0f, arrowKeys.y);

        if (Aiming(mousePosition)) {
            LookIntoAim(mousePosition, dt);
        } else {
            //TurnToMovement(movementDirection, dt);
        }

        if (movementDirection != Vector3.zero) {
            controller.Move(movementDirection * moveSpeed * dt);
        }
    }

    private bool Aiming(Vector2 mousePosition) 
    {
        bool isAiming = 
            (Math.Abs(mousePosition.x - prevMousePosition.x) > 0.01) 
            || (Math.Abs(mousePosition.y - prevMousePosition.y) > 0.01);

        return(isAiming);
    }

    private void TurnToMovement(Vector3 movementDirection, float dt)
    {
        if (movementDirection != Vector3.zero) {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movementDirection), dt * 10);
        }
    }

    private void LookIntoAim(Vector2 mousePosition, float dt)
    {
        RaycastHit lookPoint;
        Ray ray = playerCamera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out lookPoint)) {
            Vector3 lookDirection = lookPoint.point - transform.position;
            lookDirection.y = 0.0f;

            Quaternion rotation = Quaternion.LookRotation(lookDirection);
            Vector3 aimDirection = new Vector3(lookPoint.point.x, 0.0f, lookPoint.point.z);

            if (aimDirection != Vector3.zero) {
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, dt * 10);
            }
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
        movementInputAction = playerInputAction.Player.Movement;
        mouseInputAction = playerInputAction.Player.MouseLook;

        movementInputAction.Enable();
        mouseInputAction.Enable();

        playerInputAction.Player.Fire.performed += DoFire;
        playerInputAction.Player.Fire.Enable();
    }

    private void OnDisable() 
    {
        movementInputAction.Disable();
        mouseInputAction.Disable();

        playerInputAction.Player.Fire.Disable();

    }
}

public enum PlayerState
{
    IDLE,
    MOVING
}

 //player.movement(move, controller, Time.deltaTime);
// if (move.y <= 0) {
        //     playerCamera.transform.position = Vector3.Lerp(playerCamera.transform.position, cameraObservePos.position, cameraSpeed * Time.deltaTime);
        // } else {
        //     playerCamera.transform.position = Vector3.Lerp(playerCamera.transform.position, cameraFollowPos.position, cameraSpeed * Time.deltaTime);
        // }