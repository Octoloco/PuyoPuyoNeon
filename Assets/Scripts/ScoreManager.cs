using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public int totalPoints = 0;

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
        UpdatePoints();
    }

    public void AddPoints(int pointsToAdd)
    {
        totalPoints += pointsToAdd;
        UpdatePoints();
    }

    public int GetPoints()
    {
        return totalPoints;
    }

    private void UpdatePoints()
    {
        UIManager.instance.UpdateGameScore(GetPoints());
    }
}
