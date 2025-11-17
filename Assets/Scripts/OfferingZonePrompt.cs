using UnityEngine;
using TMPro;

public class OfferingZonePrompt : MonoBehaviour
{
    public GameObject promptObject;   // Offering Text

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (promptObject != null)
                promptObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (promptObject != null)
                promptObject.SetActive(false);
        }
    }
}
