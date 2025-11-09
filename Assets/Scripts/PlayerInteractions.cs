using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    [Header("InteractableInfo")]
    public float sphereCastRadius = 0.5f;
    public int interactableLayerIndex;
    private Vector3 raycastPos;
    public GameObject lookObject;
    private FPSGrab physicsObject;
    [SerializeField] Camera mainCamera;

    [Header("Pickup")]
    [SerializeField] private Transform pickupParent;
    public GameObject currentlyPickedUpObject;
    private Rigidbody pickupRB;

    [Header("ObjectFollow")]
    [SerializeField] private float minSpeed = 0;
    [SerializeField] private float maxSpeed = 300f;
    [SerializeField] private float maxDistance = 10f;
    private float currentSpeed = 0f;
    private float currentDist = 0f;

    [Header("Rotation")]
    public float rotationSpeed = 100f;
    Quaternion lookRot;

    //A simple visualization of the point we're following in the scene view
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(pickupParent.position, 0.5f);
    }

    //Interactable Object detections and distance check
    void Update()
    {
        //Here we check if we're currently looking at an interactable object
        raycastPos = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;
        Debug.DrawRay(raycastPos, mainCamera.transform.forward, Color.green);
        if (Physics.SphereCast(raycastPos, sphereCastRadius, mainCamera.transform.forward, out hit, maxDistance, 1 << interactableLayerIndex))
        {

            lookObject = hit.collider.transform.root.gameObject;

        }
        else
        {
            lookObject = null;

        }



        /*if we press the button of choice
        if (Input.GetButtonDown("Fire2"))
        {

        }
        */


    }

    public void OnGrabPressed()
    {
        //if we're not holding anything
        if (currentlyPickedUpObject == null)
        {
            //and we are looking an interactable object
            if (lookObject != null)
            {

                PickUpObject();
            }

        }
        //if we press the pickup button and have something, we drop it
        else
        {
            BreakConnection();
        }
    }


    //Velocity movement toward pickup parent and rotation
    private void FixedUpdate()
    {
        if (currentlyPickedUpObject == null) return;

        // distance & direction
        Vector3 toTarget = pickupParent.position - pickupRB.position;
        float dist = toTarget.magnitude;

        // pick a proportional speed (no deltaTime on velocity!)
        // tune these in the Inspector:
        float followGain = 12f;                 // how aggressively it chases
        float maxChaseSpeed = 20f;              // hard cap

        // proportional control
        float speed = Mathf.Min(dist * followGain, maxChaseSpeed);
        pickupRB.velocity = toTarget.normalized * speed;

        // rotation (keep if you like the look)
        Quaternion lookRot = Quaternion.LookRotation(mainCamera.transform.position - pickupRB.position);
        lookRot = Quaternion.Slerp(mainCamera.transform.rotation, lookRot, rotationSpeed * Time.fixedDeltaTime);
        pickupRB.MoveRotation(lookRot);
    }


}

// In PlayerInteractions

public void BreakConnection()
    {
        if (pickupRB)
        {
            pickupRB.useGravity = true;                    // re-enable gravity
            pickupRB.constraints = RigidbodyConstraints.None;
        }
        currentlyPickedUpObject = null;
        currentDist = 0;
        if (physicsObject) physicsObject.pickedUp = false; // null-safe
        physicsObject = null;                              // clear reference
    }

    public void PickUpObject()
    {
        physicsObject = lookObject.GetComponentInChildren<FPSGrab>();
        currentlyPickedUpObject = lookObject;
        pickupRB = currentlyPickedUpObject.GetComponent<Rigidbody>();

        pickupRB.useGravity = false;                       // <— carry nicely
        pickupRB.constraints = RigidbodyConstraints.FreezeRotation;

        physicsObject.playerInteractions = this;
        StartCoroutine(physicsObject.PickUp());            // delay before breaks allowed
    }



}