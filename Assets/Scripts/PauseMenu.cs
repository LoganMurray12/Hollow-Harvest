using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    
    public static bool IsPaused = false;

    [Header("UI")]
    public GameObject pauseMenuPanel;   // to assign pause panel

    [Header("Audio")]
    public AudioSource musicSource;     // to assign audio for pausing music

    private void Start()
    {
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);

        // lock cursor so its not moving the camera behind
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (IsPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Pause()
    {
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(true);

        Time.timeScale = 0f;
        IsPaused = true;

        // Pause music if assigned
        if (musicSource != null)
            musicSource.Pause();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Resume()
    {
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);

        Time.timeScale = 1f;
        IsPaused = false;

        // unpause music
        if (musicSource != null)
            musicSource.UnPause();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void QuitToMainMenu()
    {
        // Always unfreeze before changing scene
        Time.timeScale = 1f;
        IsPaused = false;

        
        if (musicSource != null)
            musicSource.Stop();

        SceneManager.LoadScene("MainMenu"); 
    }
}
