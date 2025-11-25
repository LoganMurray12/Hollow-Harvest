using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [Header("UI")]
    public Slider progressBar;

    [Header("Progress Settings")]
    public float currentValue = 0f;
    public float goalValue = 100f;

    private AudioSource music;

    [Header("Plant Plots")]
    public PlantPlot[] plantplots;
    private int currentUnlockedIndex = 0;

    void Start()
    {
        music = GetComponent<AudioSource>();
        if (music != null)
        {
            music.Play();
        }

        UpdateBar();

        // Lock all except the first
        for (int i = 0; i < plantplots.Length; i++)
        {
            if (plantplots[i] == null) continue;

            bool lockThis = (i > 0);
            plantplots[i].SetLocked(lockThis);
        }
    }

    public void AddProgress(float amount)
    {
        currentValue += amount;
        currentValue = Mathf.Clamp(currentValue, 0, goalValue);
        UpdateBar();

        if (currentValue >= goalValue)
        {
            currentValue = 0;
            UpdateBar();
            UnlockNextPlantPlot();
        }
    }

    private void UnlockNextPlantPlot()
    {
        currentUnlockedIndex++;

        if (currentUnlockedIndex < plantplots.Length)
        {
            PlantPlot nextPlot = plantplots[currentUnlockedIndex];

            if (nextPlot != null)
            {
                nextPlot.SetLocked(false);
                Debug.Log("Unlocked plot: " + nextPlot.name);
            }
        }
        else
        {
            Debug.Log("All plant plots already unlocked");
        }
    }

    void UpdateBar()
    {
        if (progressBar)
            progressBar.value = currentValue / goalValue;
    }
}
