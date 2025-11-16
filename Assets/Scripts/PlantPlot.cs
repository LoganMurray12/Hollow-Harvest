using System.Collections;
using UnityEngine;

public class PlantPlot : MonoBehaviour
{
    [Header("Growth Stages")]
    
    public GameObject[] stages;

    [Header("Harvest Settings")]
    public GameObject offeringPrefab;  // harvested crop
    public float growthTime = 5f;      // growth time in seconds
    public float interactRange = 3f;   // range to interact

    private int stageIndex = -1;       // -1 = dirt only, 0..N-1 = plant stage
    private Coroutine growRoutine;    // coroutine runs in background, using it here to wait and change between growth stages
    private Camera cam;

    void Start()
    {
        cam = Camera.main;            // selecting camera
        ShowOnlyCurrentStage();       // hides later stages
    }

    void Update()
    {
        // Is player pressing E
        if (!Input.GetKeyDown(KeyCode.E))
            return;

        // raycast from the center of the camera, basically a line that shoots out from camera to detect what it hits
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactRange))
        {
            // get root object infront of player
            GameObject root = hit.collider.transform.root.gameObject;

            // only react if looking at plot
            if (root == gameObject)
            {
                HandleInteraction();
            }
        }
    }

    void HandleInteraction()
    {
        // if no plant, plant seed
        if (stageIndex == -1)
        {
            PlantCrop();
        }
        // if plant is fully grown, harvest
        else if (stageIndex == stages.Length - 1)
        {
            HarvestCrop();
        }
        // if growing do nothing
    }

    void PlantCrop()
    {
        stageIndex = 0;          // start at first stage
        ShowOnlyCurrentStage();

        // restart coroutine
        if (growRoutine != null)
            StopCoroutine(growRoutine);

        growRoutine = StartCoroutine(GrowCrop());
    }

    IEnumerator GrowCrop()
    {
        // keeps looping growth until at final stage
        while (stageIndex < stages.Length - 1)
        {
            yield return new WaitForSeconds(growthTime);
            stageIndex++;
            ShowOnlyCurrentStage();
        }

        growRoutine = null;  // done growing
    }

    void HarvestCrop()
    {
        // offering spawn point
        if (offeringPrefab != null)
        {
            Vector3 spawnPos = transform.position + Vector3.up * 0.5f;
            GameObject offering = Instantiate(offeringPrefab, spawnPos, Quaternion.identity);

            // pops offering upwards
            Rigidbody rb = offering.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(Vector3.up * 2f, ForceMode.Impulse);
            }
        }

        // reset to dirt stage
        stageIndex = -1;
        ShowOnlyCurrentStage();
    }

    void ShowOnlyCurrentStage()
    {
        // hides all models except current one
        for (int i = 0; i < stages.Length; i++)
        {
            if (stages[i] == null) continue;

            bool shouldShow = (i == stageIndex);
            stages[i].SetActive(shouldShow);
        }
    }
}
