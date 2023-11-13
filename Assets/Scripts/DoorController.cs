using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private Animator doorAnimations;
    private bool doorOpen = false;
    private bool animationIsFinished = true;

    private void Awake()
    {
        doorAnimations = gameObject.GetComponent<Animator>();
    }
    
    public void interactDoor()
    {
        Debug.Log("Door");
        if (animationIsFinished)
        {
            if (!doorOpen)
            {
                doorAnimations.Play("DoorOpen", 0, 0.0f);
                //doorOpen = true;
                animationIsFinished = false;
            }
            else
            {
                doorAnimations.Play("DoorClose", 0, 0.0f);
                //doorOpen = false;
                animationIsFinished = false;
            }
        }
    }
    public void finished()
    {
        animationIsFinished= true;
        Debug.Log("finished!");
        doorOpen = !doorOpen;

    }
}
