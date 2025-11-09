using UnityEngine;
public class GrabInputBridge : MonoBehaviour
{
    public PlayerInteractions interactions;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            interactions.OnGrabPressed();
    }
}