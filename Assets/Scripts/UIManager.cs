using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField]
    private GameOverPanel gameOverPanel;
    [SerializeField]
    private GamePanel gamePanel;
    [SerializeField]
    private PausePanel pausePanel;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ShowGameOverPanel()
    {
        gameOverPanel.finished = false;
        gameOverPanel.Show();
    }

    public void HideGameOverPanel()
    {
        gameOverPanel.Hide();
    }

    public void SetFinalScore(int finalScore)
    {
        gameOverPanel.SetScoreGoal(finalScore);
    }

    public void UpdateGameScore(int newScore)
    {
        gamePanel.UpdateScore(newScore);
    }

    public void ShowPausePanel()
    {
        pausePanel.Show();
    }

    public void HidePausePanel()
    {
        pausePanel.Hide();
    }

    public void DrawLevel(int level)
    {
        gamePanel.DrawLevel(level);
    }
}
