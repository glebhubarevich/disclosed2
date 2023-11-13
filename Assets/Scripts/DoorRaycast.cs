using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DoorRaycast : MonoBehaviour
{
    [SerializeField] int rayLength = 5;
    [SerializeField] LayerMask layerMaskInteract;
    [SerializeField] string excludeLayerName = null;

    [SerializeField] Image crosshair = null;
    

    private DoorController raycastedDoor;
    private DragableController raycastedDragObject;

    private bool isCrosshairActive;
    private bool doOnce;

    PlayerInput playerInput;
    InputAction interactAction;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        interactAction = playerInput.actions["Interact"];
        crosshair.enabled= false;
    }

    private const string doorTag = "ActiveDoor";
    private const string dragableTag = "Dragable";

    private void Update()
    {
        RaycastHit hit;
        Vector3 forward = transform.TransformDirection(Vector3.forward);

        int mask = 1 << LayerMask.NameToLayer(excludeLayerName) | layerMaskInteract.value;
        if (Physics.Raycast(transform.position, forward, out hit, rayLength, mask))
        {
            //DOOR
            if (hit.collider.CompareTag(doorTag))
            {
                
                if (!doOnce)
                {
                    CrosshairChange(true);
                    raycastedDoor = hit.collider.gameObject.GetComponent<DoorController>();
                    
                }
                isCrosshairActive = true;
                doOnce = true;
    
                var interactInput = interactAction.ReadValue<float>();
                if (interactInput > 0)
                {
                    CrosshairChange(false);
                    raycastedDoor.interactDoor();
                    
                }
                
            }

            //DRAGABLE
            if (hit.collider.CompareTag(dragableTag))
            {
                if (!doOnce)
                {
                    CrosshairChange(true);
                    raycastedDragObject = hit.collider.gameObject.GetComponent<DragableController>();

                }
                isCrosshairActive = true;
                doOnce = true;

                var interactInput = interactAction.ReadValue<float>();
                if (interactInput > 0)
                {
                    //CrosshairChange(false);
                    raycastedDragObject.dragObject();
                }
            }
        }
        else
        {
            if (isCrosshairActive)
            {
                CrosshairChange(false);
                doOnce = false;
            } 
        }
    }

    void CrosshairChange(bool isOn)
    {
        if(isOn && !doOnce)
        {
            crosshair.enabled= true;
            
        }
        else
        {
            crosshair.enabled = false;
            isCrosshairActive = false;
        }
    }

}
