using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [Header("UI")]
    public Slider progressBar;      // UI Slider
    [Header("Progress Settings")]
    public float currentValue = 0f; // Min
    public float goalValue = 100f;  // Max
    private AudioSource music;

    void Start()
    {
        music = GetComponent<AudioSource>();
        if (music != null)
        {
            music.Play();
        }

        UpdateBar();
    }


    public void AddProgress(float amount)
    {
        currentValue += amount;
        currentValue = Mathf.Clamp(currentValue, 0, goalValue);
        UpdateBar();
    }

    void UpdateBar()
    {
        if (progressBar)
            progressBar.value = currentValue / goalValue;
    }
}