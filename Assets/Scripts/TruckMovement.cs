using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckMovement : MonoBehaviour
{
    public GameObject stopTrigger;
    public float speed = 10f;
    
    bool isMoving = true;
    Collider stopCollider;

    private void Awake()
    {
        stopCollider = stopTrigger.GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if(other == stopCollider)
        {
            isMoving = false;
        }
    }
    void Update()
    {
        if (isMoving)
        {
            transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z) ;
        }
    }
}
