using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    AudioManager audioManager;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            audioManager.PlaySFX(audioManager.click);
        }
    }

    public void MainManu()
    {
        SceneManager.LoadSceneAsync("Main Manu");
        audioManager.PlaySFX(audioManager.click);
    }

    public void SelectCharacter()
    {
        SceneManager.LoadSceneAsync("Select Character");
        audioManager.PlaySFX(audioManager.click);
    }

    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("Game Play");
        audioManager.PlaySFX(audioManager.click);
    }

    public void QuitGame()
    {
        Application.Quit();
        audioManager.PlaySFX(audioManager.click);
    }
}
