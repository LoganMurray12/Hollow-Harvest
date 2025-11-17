using UnityEngine;

public class OfferZone : MonoBehaviour
{
    [Header("Scoring")]
    public int pointsPerOffer = 10; // to change points

    [Header("Audio")]
    public AudioSource offerSound;   // to assign audio

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Offering")) return;

        // play sound
        if (offerSound != null)
            offerSound.Play();

        // add points
        LevelManager level = FindObjectOfType<LevelManager>();
        if (level != null)
        {
            level.AddProgress(pointsPerOffer);
        }

        // destroy object
        Destroy(other.gameObject);
    }
}
