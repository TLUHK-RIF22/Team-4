using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenuUI;

    private bool isPaused = false;

void Start()
{
    pauseMenuUI.SetActive(false);  
    isPaused = false;              
}
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void LoadMenu()
    {
        // Load your menu scene here
        Time.timeScale = 1f;
        Debug.Log("Loading menu...");
        SceneManager.LoadScene("MenuScene");
    }

public void QuitGame()
    {
        Time.timeScale = 1f;
        // Quit the application
        Application.Quit();

        // If running in the Unity editor, log quitting for clarity
        #if UNITY_EDITOR
        Debug.Log("Game is exiting...");
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
