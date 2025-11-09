using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] private Camera mainCamera;

    [Header("Pickup")]
    [SerializeField] private Transform pickupParent;     // assign a HoldPoint
    public GameObject currentlyPickedUpObject;
    private Rigidbody pickupRB;

    [Header("ObjectFollow")]
    [SerializeField] private float maxDistance = 10f;
    [SerializeField] private float followGain = 12f;     // how aggressively it chases
    [SerializeField] private float maxChaseSpeed = 20f;  // hard cap

    [Header("Rotation")]
    public float rotationSpeed = 100f;
    private Quaternion lookRot;

    private void OnDrawGizmos()
    {
        if (!pickupParent) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(pickupParent.position, 0.5f);
    }

    void Update()
    {
        // Center-screen spherecast for interactables
        raycastPos = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        RaycastHit hit;
        Debug.DrawRay(raycastPos, mainCamera.transform.forward, Color.green);

        int layerMask = 1 << interactableLayerIndex;
        if (Physics.SphereCast(raycastPos, sphereCastRadius, mainCamera.transform.forward, out hit, maxDistance, layerMask))
        {
            lookObject = hit.collider.transform.root.gameObject;
        }
        else
        {
            lookObject = null;
        }

        // (Your input bridge can call OnGrabPressed on KeyCode.E)
    }

    public void OnGrabPressed()
    {
        if (currentlyPickedUpObject == null)
        {
            if (lookObject != null)
                PickUpObject();
        }
        else
        {
            BreakConnection();
        }
    }

    // Velocity follow toward pickupParent + gentle facing
    private void FixedUpdate()
    {
        if (currentlyPickedUpObject == null) return;

        Vector3 toTarget = pickupParent.position - pickupRB.position;
        float dist = toTarget.magnitude;

        float speed = Mathf.Min(dist * followGain, maxChaseSpeed);
        pickupRB.linearVelocity = toTarget.normalized * speed; // no deltaTime on velocity

        lookRot = Quaternion.LookRotation(mainCamera.transform.position - pickupRB.position);
        lookRot = Quaternion.Slerp(mainCamera.transform.rotation, lookRot, rotationSpeed * Time.fixedDeltaTime);
        pickupRB.MoveRotation(lookRot);
    }

    // Release the object
    public void BreakConnection()
    {
        if (pickupRB)
        {
            pickupRB.useGravity = true;
            pickupRB.constraints = RigidbodyConstraints.None;
        }

        currentlyPickedUpObject = null;
        if (physicsObject) physicsObject.pickedUp = false;

        physicsObject = null;
        pickupRB = null;
    }

    public void PickUpObject()
    {
        physicsObject = lookObject.GetComponentInChildren<FPSGrab>();
        currentlyPickedUpObject = lookObject;
        pickupRB = currentlyPickedUpObject.GetComponent<Rigidbody>();

        pickupRB.useGravity = false; // carry nicely
        pickupRB.constraints = RigidbodyConstraints.FreezeRotation;

        physicsObject.playerInteractions = this;
        StartCoroutine(physicsObject.PickUp()); // delay before break checks
    }
}
