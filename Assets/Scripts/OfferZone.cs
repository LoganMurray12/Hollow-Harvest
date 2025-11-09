using UnityEngine;

public class OfferZone : MonoBehaviour
{
    public int pointsPerOffer = 10; // sets points amount

    private void OnTriggerEnter(Collider other)
    {
        // checks offering tag
        if (!other.CompareTag("Offering")) return;

        // adding points
        LevelManager level = FindObjectOfType<LevelManager>();
        if (level != null)
        {
            level.AddProgress(pointsPerOffer);
        }

        // destroy object
        Destroy(other.gameObject);
    }
}
