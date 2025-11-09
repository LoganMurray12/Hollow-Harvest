using UnityEngine;

public class HarvestableCrop : MonoBehaviour
{
    [Header("Offering Prefab")]
    public GameObject offeringPrefab;     // drag your Offering prefab here

    [Header("Spawn Settings")]
    public Transform spawnPoint;          // optional, can use object’s position if empty
    public float spawnForce = 2f;         // force that item pops out

    [Header("Harvest Control")]
    public KeyCode harvestKey = KeyCode.E;
    public bool canHarvest = true;

    private Camera playerCam;

    void Start()
    {
        playerCam = Camera.main; // or assign manually if needed
    }

    void Update()
    {
        if (!canHarvest) return;

        // Is the player looking at the crop?
        Ray ray = new Ray(playerCam.transform.position, playerCam.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 3f)) // adjust range here
        {
            if (hit.collider.gameObject == gameObject)
            {
                if (Input.GetKeyDown(harvestKey))
                {
                    Harvest();
                }
            }
        }
    }

    void Harvest()
    {
        if (!offeringPrefab) return;

        // Where to spawn
        Vector3 pos = spawnPoint ? spawnPoint.position : transform.position + Vector3.up * 0.5f;

        // Create the offering
        GameObject offering = Instantiate(offeringPrefab, pos, Quaternion.identity);

        // Give it a gentle upward push so it feels physical
        Rigidbody rb = offering.GetComponent<Rigidbody>();
        if (rb) rb.AddForce(Vector3.up * spawnForce, ForceMode.Impulse);

        // Disable the crop so it looks harvested
        canHarvest = false;
        gameObject.SetActive(false);
    }
}
