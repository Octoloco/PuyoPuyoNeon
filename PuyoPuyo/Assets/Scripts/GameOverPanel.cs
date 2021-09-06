using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameOverPanel : UIPanel
{
    [SerializeField]
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private TextMeshProUGUI highScoreText;

    [SerializeField]
    private Button tryAgainButton;
    [SerializeField]
    private Button menuButton;
    [SerializeField]
    private Button exitButton;

    private float scoreUpTimer = .05f;
    private float startScoreTimer = .5f;
    public int scoreToShow = 0;
    public int scoreGoal = 0;
    public int highScore = 0;
    public bool finished = true;
    private bool newHighscore = false;

    private void Start()
    {
        scoreText.text = scoreToShow.ToString();
        Load();
    }

    new private void Update()
    {
        base.Update();

        if (startScoreTimer > 0)
        {
            startScoreTimer -= Time.deltaTime;
        }
        else if (scoreUpTimer > 0)
        {
            scoreUpTimer -= Time.deltaTime;
        }
        if (scoreGoal < highScore)
        {
            highScoreText.text = highScore.ToString();
        }

        if (scoreUpTimer <= 0 && scoreToShow < scoreGoal) 
        {
            
            scoreUpTimer = .05f;
            scoreToShow++;
            scoreText.text = scoreToShow.ToString();
            if (highScore < scoreGoal)
            {
                highScoreText.text = scoreToShow.ToString();
            }
        }
        else if (scoreToShow >= scoreGoal && !finished)
        {
            finished = true;
            if (scoreGoal > highScore)
            {
                GetComponentInChildren<TitleFadeIn>().activate = true;
                Save();
            }
        }
        else if (finished)
        {
            UnlockGameOverButtons();
        }
    }

    public void UnlockGameOverButtons()
    {
        exitButton.interactable = true;
        menuButton.interactable = true;
        tryAgainButton.interactable = true;
    }

    public void SetScoreGoal(int newScoreGoal)
    {
        scoreGoal = newScoreGoal;
    }

    private void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerScore.dat");

        PlayerData data = new PlayerData();
        data.highScore = scoreGoal;

        bf.Serialize(file, data);
        file.Close();
    }

    private void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/playerScore.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerScore.dat", FileMode.Open);

            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            Debug.Log("loaded = " + data.highScore);
            highScore = data.highScore;
            Debug.Log("Highscore = " + data.highScore);
        }
    }
}

[System.Serializable]
class PlayerData
{
    public int highScore;
}
