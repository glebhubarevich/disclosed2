using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] Transform cameraTransform;
    [SerializeField] float mouseSensivity = 3f;
    public float movementSpeed = 5f;
    [SerializeField] float mass = 100f;
    [SerializeField] float jumpHeight = 5f;
    [SerializeField] float acceleration = 10f;
    [SerializeField] float crouchHeight = 1f;
    [SerializeField] float crouchTransitionSpeed = 10f;

    float standingHeight;
    float currentHeight;
    bool isCrouching => standingHeight - currentHeight > 0.1f;

    CharacterController controller;
    Vector2 look;
    Vector3 velocity;

    PlayerInput playerInput;
    InputAction moveAction;
    InputAction lookAction;
    InputAction jumpAction;
    InputAction sprintAction;
    InputAction crouchAction;

    Vector3 initialCameraPosition;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        lookAction = playerInput.actions["Look"];
        jumpAction = playerInput.actions["Jump"];
        sprintAction = playerInput.actions["Sprint"];
        crouchAction = playerInput.actions["Crouch"];
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        standingHeight = currentHeight = controller.height;
        initialCameraPosition = cameraTransform.localPosition;

    }

    void Update()
    {
        UpdateGravity();
        UpdateLook();
        UpdateMovement();

    }

    void UpdateMovement()
    {
        var moveInput = moveAction.ReadValue<Vector2>();
        var sprintInput = sprintAction.ReadValue<float>();

        var input = new Vector3();

        input += transform.forward * moveInput.y;
        input += transform.right * moveInput.x;
        input = Vector3.ClampMagnitude(input, 1f);

        var speedMultiplier = sprintInput > 0 ? 1.5f : 1f;
        speedMultiplier = isCrouching && controller.isGrounded ? 0.4f : 1f;
        input *= movementSpeed * speedMultiplier;

        var factor = acceleration * Time.deltaTime;
        velocity.x = Mathf.Lerp(velocity.x, input.x, factor);
        velocity.z = Mathf.Lerp(velocity.z, input.z, factor);

        //JUMPING
        var jumpInput = jumpAction.ReadValue<float>();
        if (jumpInput > 0 && controller.isGrounded)
        {
            velocity.y += jumpHeight;
        }

        //CROUCHING 
        var isTryingToCrouch = crouchAction.ReadValue<float>() > 0;
        var heightTarget = isTryingToCrouch? crouchHeight : standingHeight;

        if(isCrouching && !isTryingToCrouch)
        {
            var castOrigin = transform.position + new Vector3(0, currentHeight / 2, 0);
            if(Physics.Raycast(castOrigin, Vector3.up, out RaycastHit hit, 0.2f))
            {
                var distanceToCeiling = hit.point.y - castOrigin.y;
                heightTarget = Mathf.Max(currentHeight + distanceToCeiling - 0.1f, crouchHeight);
            }
        }

        if(!Mathf.Approximately(heightTarget, currentHeight))
        {
            var crouchDelta = Time.deltaTime * crouchTransitionSpeed;
            currentHeight = Mathf.Lerp(currentHeight, heightTarget, crouchDelta);

            var halfHeightDifference = new Vector3(0, (standingHeight - currentHeight) / 2, 0);
            var newCameraPosition = initialCameraPosition - halfHeightDifference;
            cameraTransform.localPosition = newCameraPosition;
            controller.height = heightTarget;
        }

        


        controller.Move(velocity * Time.deltaTime);
    }
    void UpdateLook()
    {
        var lookInput = lookAction.ReadValue<Vector2>();
        look.x += lookInput.x * mouseSensivity;
        look.y += lookInput.y * mouseSensivity;

        look.y = Mathf.Clamp(look.y, -89f, 89);

        transform.localRotation = Quaternion.Euler(0, look.x, 0);
        cameraTransform.localRotation = Quaternion.Euler(-look.y, 0, 0);
    }
    void UpdateGravity()
    {
        var gravity = Physics.gravity * mass * Time.deltaTime;
        velocity.y = controller.isGrounded ? -1f : velocity.y + gravity.y; 
    }
}
