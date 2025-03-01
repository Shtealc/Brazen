using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI; // Reference to the pause menu UI panel
    public AudioSource pauseMusic; // Background music, just make it the cave sounds

    private bool isPaused = false;
    private AudioSource[] allAudioSources; // Stores all game audio sources

    void Start()
    {
        allAudioSources = FindObjectsOfType<AudioSource>(); // Get all audio sources
    }

    void Update()
    {
        // Toggle pause when pressing escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    // Resumes the game
    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false); // Hide pause menu
        Time.timeScale = 1f; // Resume game time

        // Resume all paused sounds
        foreach (AudioSource audio in allAudioSources)
        {
            if (audio != pauseMusic && !audio.isPlaying) // Avoid resuming pause music
            {
                audio.UnPause();
            }
        }

        pauseMusic.Stop(); // Stop pause music
        isPaused = false;
    }

    // Pauses the game
    public void PauseGame()
    {
        pauseMenuUI.SetActive(true); // Show pause menu
        Time.timeScale = 0f; // Freeze game time

        foreach (AudioSource audio in allAudioSources)
        {
            if (audio != pauseMusic && audio.isPlaying) // Avoid pausing pause music
            {
                audio.Pause();
            }
        }

        pauseMusic.Play(); // Start playing pause menu music
        isPaused = true;
    }

    // For quitting the game
    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}
