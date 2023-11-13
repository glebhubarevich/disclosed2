using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragableController : MonoBehaviour
{
    GameObject player;
    Vector3 playerPos;
    Camera playerCamera;
    Player playerScript;

    PlayerInputController playerInput;
    InputAction interactAction;

    float prevPlayerSpeed;

    [SerializeField] float mass = 4;

    bool isDragged = false;

    private void Awake()
    {
        playerInput = new PlayerInputController();
        interactAction = playerInput.Player.Interact;
        interactAction.Enable();
        player = GameObject.Find("Player");
        playerCamera = Camera.main;
        playerScript = player.GetComponent<Player>();
        prevPlayerSpeed = playerScript.movementSpeed;

    }

    void Update()
    {
        interactAction.performed += context =>
        {
            isDragged = false;
        };

        if (isDragged)
        {
            var newPosition = playerCamera.transform.position + playerCamera.transform.forward * 2f;
            newPosition = new Vector3(newPosition.x, transform.position.y, newPosition.z);
            playerScript.movementSpeed = prevPlayerSpeed / mass;
            transform.position = newPosition;
            transform.rotation = new Quaternion(0.0f, playerCamera.transform.rotation.y, 0.0f, playerCamera.transform.rotation.w);
            Debug.Log(playerScript.movementSpeed);
        }
        else
        {
            playerScript.movementSpeed = prevPlayerSpeed;
        }
    }

    public void dragObject()
    {
        isDragged = true;
    }

}
