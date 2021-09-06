using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
    private bool gamePaused = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!gamePaused)
            {
                PauseGame();
            }
            else
            {
                Continue();
            }
        }
    }

    public void RestartGame()
    {
        MusicManager.instance.StopClip();
        LevelLoader.instance.LoadNextLevel(1);
    }

    public void BackToMenu()
    {
        MusicManager.instance.StopClip();
        LevelLoader.instance.LoadNextLevel(0);
    }

    public void StartGame()
    {
        MusicManager.instance.StopClip();
        LevelLoader.instance.LoadNextLevel(1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void PauseGame()
    {
        gamePaused = true;
        GridManager.instance.SetGameStarted(false);
        UIManager.instance.ShowPausePanel();
    }

    public void Continue()
    {
        gamePaused = false;
        UIManager.instance.HidePausePanel();
        GridManager.instance.SetGameStarted(true);
    }
}
