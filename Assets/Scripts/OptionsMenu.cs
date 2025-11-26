using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [Header("Panels")]
    public GameObject previousPanel;
    public GameObject optionsPanel;

    [Header("Audio Settings")]
    public AudioMixer audioMixer;          // Assign MasterMixer
    public Slider volumeSlider;            // Assign volume slider in inspector

    [Header("Resolution Settings")]
    public Dropdown resolutionDropdown;    // Assign dropdown in inspector

    private Resolution[] resolutions;

    private void Start()
    {
        // Load the volume
        if (volumeSlider != null && audioMixer != null)
        {
            if (PlayerPrefs.HasKey("MasterVolume"))
            {
                float savedVol = PlayerPrefs.GetFloat("MasterVolume");
                volumeSlider.value = savedVol;
                SetVolume(savedVol);
            }
        }

        // resolution dropdown
        if (resolutionDropdown != null)
            SetupResolutions();
    }

  // Panel Switching Section
    public void OpenOptions()
    {
        if (previousPanel != null)
            previousPanel.SetActive(false);

        if (optionsPanel != null)
            optionsPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        if (optionsPanel != null)
            optionsPanel.SetActive(false);

        if (previousPanel != null)
            previousPanel.SetActive(true);
    }

   // Volume Section
    public void SetVolume(float value)
    {
        // Slider has 0–1 value; AudioMixer uses decibels so this converts
        float dB = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat("MasterVolume", dB);
        // Line above saves volume when game exits
        PlayerPrefs.SetFloat("MasterVolume", value);
    }

    // Resolution Section
    private void SetupResolutions()
    {
        // stores screen resolution
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        // options are filled automatically so this clears it

        int currentIndex = 0;
        var options = new System.Collections.Generic.List<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            // tis adds readable options
            string option =
                resolutions[i].width + " x " + resolutions[i].height +
                " @ " + resolutions[i].refreshRate + "Hz";

            options.Add(option);

            // checks and highlights current resolution
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int index)
    {
        // This applies the resolution when players select a resolution
        Resolution res = resolutions[index];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }
}
