using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Class responsible for the movement of the player.
/// We don't need AI in this game so it's ok to have input management and movement logic in one place.
/// </summary>

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed = 10f;

    [SerializeField]
    private float jumpStrength = 10f;

    [SerializeField]
    private float gravity = 9.81f;

    [SerializeField]
    private bool invertYAxis = false;

    [SerializeField]
    [Range(0f, 10f)]
    private float cameraXYSensitivity = 10f;

    private PlayerInput playerInput;
    private CharacterController characterController;

    private float cameraVerticalRotation = 0f;
    private float yMovementVelocity = 0f;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        characterController = GetComponent<CharacterController>();

        if (!playerInput || !characterController)
            Debug.LogError("[PlayerMovement] Couldn't initialize PlayerMovement required components!");

    }

    private void Update()
    {
        ApplyGravity();
        ApplyPlayerMovement();
        ApplyPlayerCameraRotation();
    }

    private void ApplyGravity()
    {
        yMovementVelocity -= gravity * Time.deltaTime;
    }

    private void ApplyPlayerMovement()
    {
        // Find the movement direction
        var movementInput = playerInput.actions["Move"].ReadValue<Vector2>();
        Vector3 moveDirection = new(movementInput.x, 0, movementInput.y);
        moveDirection = transform.TransformDirection(moveDirection.normalized);

        // Compute final movement vector for this frame 
        var movement = (movementSpeed * moveDirection) + (Vector3.up * yMovementVelocity);

        // Apply movement
        characterController.Move(movement * Time.deltaTime);
    }

    private void ApplyPlayerCameraRotation()
    {
        var mouseDelta = playerInput.actions["Look"].ReadValue<Vector2>();

        // Apply settings
        mouseDelta *= cameraXYSensitivity / 10f;
        if (invertYAxis)
            mouseDelta.y = -mouseDelta.y;

        // Rotate the character on the horizontal axis
        transform.Rotate(new Vector3(0, 1, 0), mouseDelta.x);

        // Rotate only the camera on the vertical axis
        // and cap the min and max pitch angles
        cameraVerticalRotation -= mouseDelta.y;
        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -80f, 80f);

        Camera.main.transform.localRotation = Quaternion.Euler(cameraVerticalRotation, 0, 0);
    }

    // PlayerInput JUMP event
    private void OnJump()
    {
        if (characterController.isGrounded)
            yMovementVelocity = jumpStrength;
    }
}