using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class MenuManager_START : MonoBehaviour
{
    [SerializeField] private GameObject painelMenuInicial;
    [SerializeField] private GameObject painelOpcoes;
    [SerializeField] private GameObject StartingSceneTransition;
    [SerializeField] private GameObject MenuSound;

    public void StartGame()
    {
        SceneManager.LoadScene("MenuBooks");

    }

    private void OnCutsceneLoaded(AsyncOperation asyncOp)
        {
            Debug.Log("Cutscene loaded");
        }

    public void OpenOptions()
    {
        painelMenuInicial.SetActive(false);
        painelOpcoes.SetActive(true);
    }

       public void CloseOptions()
    {
        painelMenuInicial.SetActive(true);
        painelOpcoes.SetActive(false);
    }

    public void Quit()
    {
        Debug.Log("Quitting the Game");
        Application.Quit();
    }

    private void DisableStartingSceneTransition()
    {
        StartingSceneTransition.SetActive(true);
    }


}
