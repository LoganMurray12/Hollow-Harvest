using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    [Header("Panels")]
    public GameObject previousPanel;   // back to previous panel
    public GameObject optionsPanel;    // going to use this panel in main menu aswell

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
}
