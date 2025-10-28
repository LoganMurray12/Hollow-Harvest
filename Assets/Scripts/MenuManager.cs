using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Scene to Load")]
    public string gameSceneName = "Game";

    [Header("UI Panels")]
    public GameObject mainMenuPanel;   
    public GameObject optionsPanel;    

    
    public void StartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    
    public void OpenOptions()
    {
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    
    public void BackToMenu()
    {
        optionsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}