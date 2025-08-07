using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public bool isPaused;
    private AudioSource musicAudioSource;
    private float musicTimeScale;
    public GameObject UI_comp;
    public GameObject Options;

    void Start()
    {
        pauseMenu.SetActive(false);
        UI_comp.SetActive(true);
        musicAudioSource = GameObject.Find("BG music").GetComponent<AudioSource>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isPaused)
            {
                ResumeGame();
                UI_comp.SetActive(true);
                Options.SetActive(false);
            }
            else
            {
                PauseGame();
                UI_comp.SetActive(false);
            }
        }
        
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        PauseMusic();
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        ResumeMusic();
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void PauseMusic()
    {
        musicTimeScale = musicAudioSource.pitch;
        musicAudioSource.Pause();
    }

    public void ResumeMusic()
    {
        musicAudioSource.pitch = musicTimeScale;
        musicAudioSource.Play();
    }

    public void GoToOptions()
    {
        Time.timeScale = 0f;
        Options.SetActive(true);
        pauseMenu.SetActive(false);
        isPaused = true;

    }

    public void Back()
    {
        Time.timeScale = 0f;
        Options.SetActive(false);
        pauseMenu.SetActive(true);
        isPaused = true;

    }

    

}
