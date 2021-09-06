using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GridManager : MonoBehaviour
{
    public static GridManager instance = null;

    [SerializeField]
    private int arrayYSize;
    [SerializeField]
    private int arrayXSize;
    [SerializeField]
    private bool gameStarted;
    [SerializeField]
    private bool gameOver = false;

    private PuyoScript[,] puyosArray;
    private PuyoScript[] continuosPuyosArray;
    private PuyoScript[] topPuyosArray;
    private PuyoScript[] puyosToCheck;
    private PuyoScript[] puyosToFall;
    private bool canSpawn = true;
    private bool broadcastSent = false;
    private int numberOfContinuosPuyos;
    private int puyosToCheckCount = 0;
    private int chainSize = 0;
    private int puyoPoints = 0;
    private int puyosToFallCount = 0;

    private int numberOfUnlockedPuyos = 0;

    private int destroyIndex = 0;
    private float destroyPhaseTimer = .25f;
    private float dropPhaseTimer = .25f;
    private bool startDropPhase = false;
    private bool startDestroyPhase = true;
    public float timeScaleDifficulty = 1;
    public int level = 1;

    private bool canLevelUp = true;

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
        StartCoroutine(WaitToStart());
        DrawLevel(level);
        Time.timeScale = timeScaleDifficulty;
        puyosArray = new PuyoScript[arrayYSize, arrayXSize];
        continuosPuyosArray = new PuyoScript[arrayYSize * arrayXSize];
        topPuyosArray = new PuyoScript[arrayYSize * arrayXSize];
        puyosToCheck = new PuyoScript[arrayYSize * arrayXSize];
        puyosToFall = new PuyoScript[arrayYSize * arrayXSize];
    }

    void Update()
    {
        if (gameStarted)
        {
            if (ScoreManager.instance.GetPoints() > 0 && timeScaleDifficulty < 3)
            {
                if (canLevelUp && ScoreManager.instance.GetPoints() > level * 10)
                {
                    level++;
                    timeScaleDifficulty += .1f;
                    Time.timeScale = timeScaleDifficulty;
                    canLevelUp = false;
                    DrawLevel(level);
                }


                if (ScoreManager.instance.GetPoints() % 10 != 0)
                {
                    canLevelUp = true;
                }
            }

            if (canSpawn)
            {
                
                PuyoPuyoSpawner.instance.Spawn();
                canSpawn = false;
            }

            if (startDropPhase)
            {
                dropPhaseTimer -= Time.deltaTime;
            }

            if (startDestroyPhase)
            {
                destroyPhaseTimer -= Time.deltaTime;
            }

            CheckPhases();
        }
        else if (gameOver)
        {
            UIManager.instance.ShowGameOverPanel();
            UIManager.instance.SetFinalScore(ScoreManager.instance.GetPoints());
        }
    }

    IEnumerator WaitToStart()
    {
        yield return new WaitForSeconds(1.5f);
        gameStarted = true;
        MusicManager.instance.PlayClip(1);
    }

    private void DrawLevel(int level)
    {
        UIManager.instance.DrawLevel(level);
    }

    public void SetGameStarted(bool state)
    {
        gameStarted = state;
    }

    public bool GetGameStarted()
    {
        return gameStarted;
    }

    public float GetTimeScale()
    {
        return timeScaleDifficulty;
    }

    public void PuyoUnlocked()
    {
        numberOfUnlockedPuyos++;
    }

    public void PuyoLocked()
    {
        numberOfUnlockedPuyos--;
    }

    public void CheckPhases()
    {
        if (numberOfUnlockedPuyos == 0)
        {
            if (destroyPhaseTimer <= 0)
            {
                startDestroyPhase = false;
                
                if (destroyIndex < puyosToCheckCount)
                {
                    if (puyosToCheck[destroyIndex] != null)
                    {
                        DestroyCheck(puyosToCheck[destroyIndex].GetPosX(), puyosToCheck[destroyIndex].GetPosY());
                    }
                    destroyIndex++;
                }
                else if (destroyIndex >= puyosToCheckCount)
                {
                    destroyIndex = 0;
                }
                startDropPhase = true;
            }

            if (dropPhaseTimer <= 0)
            {
                startDropPhase = false;
                puyosToFallCount = 0;
                for (int i = arrayYSize - 2; i > 0; i--)
                {
                    for (int j = 0; j < arrayXSize; j++)
                    {
                        if (puyosArray[i, j] != null)
                        {
                            puyosToFall[puyosToFallCount] = puyosArray[i, j];
                            puyosToFallCount++;

                        }
                    }
                }

                
                for (int i = 0; i < puyosToFallCount; i++)
                {
                    puyosToFall[i].Unlock();
                }

                puyosToCheckCount = 0;
                if (numberOfUnlockedPuyos > 0 && !broadcastSent)
                {
                    broadcastSent = true;
                    for (int i = 0; i < puyosToFallCount; i++)
                    {
                        puyosToFall[i].Settle();
                    }
                }
                else
                {
                    puyoPoints *= chainSize;
                    ScoreManager.instance.AddPoints(puyoPoints);
                    puyoPoints = 0;
                    chainSize = 0;
                    broadcastSent = false;
                    canSpawn = true;
                }
                dropPhaseTimer = .25f;
            }
        }
    }

    public void SettlePuyo(int posX, int posY, PuyoScript puyo)
    {
        if (posY == 0)
        {
            gameOver = true;
            gameStarted = false;
        }
        puyosToCheck[puyosToCheckCount] = puyosArray[posY, posX];
        puyosToCheckCount++;

    }

    public void UnlinkPuyoFromGrid(int posX, int posY)
    {
        puyosArray[posY, posX] = null;
    }

    public void LinkPuyoToGrid(int posX, int posY, PuyoScript puyo)
    {
        puyosArray[posY, posX] = puyo;
    }

    private void DestroyCheck(int posX, int posY)
    {
        numberOfContinuosPuyos = RecursiveCheck(0, posY, posX);
        if (numberOfContinuosPuyos > 3)
        {
            SFXManager.instance.PlayClip(2);
            chainSize++;
            destroyPhaseTimer = .25f;
            startDestroyPhase = true;
        }

        for (int i = 0; i < numberOfContinuosPuyos; i++)
        {
            if (numberOfContinuosPuyos > 3)
            {
                puyoPoints++;
                continuosPuyosArray[i].Die();
                puyosArray[continuosPuyosArray[i].GetPosY(), continuosPuyosArray[i].GetPosX()] = null;
            }
            continuosPuyosArray[i] = null;
        }
        
    }

    private PuyoScript GetPuyoAt(int posY, int posX)
    {
        if (posY < arrayYSize - 1 && posX < arrayXSize - 1 && posY > 0 && posX > 0)
        {
            return puyosArray[posY, posX];
        }
        else
        {
            return null;
        }
    }

    private int RecursiveCheck(int numberOfContinuosPuyos, int posY, int posX)
    {
        if (posY > 0)
        {
            if (puyosArray[posY - 1, posX] != null)
            {
                if (!continuosPuyosArray.Contains(puyosArray[posY - 1, posX]))
                {
                    if (puyosArray[posY, posX].GetColor() == puyosArray[posY - 1, posX].GetColor())
                    {
                        continuosPuyosArray[numberOfContinuosPuyos] = puyosArray[posY - 1, posX];
                        numberOfContinuosPuyos++;
                        numberOfContinuosPuyos = RecursiveCheck(numberOfContinuosPuyos, posY - 1, posX);
                    }
                }
            }
        }

        if (posX > 0)
        {
            if (puyosArray[posY, posX - 1] != null)
            {
                if (!continuosPuyosArray.Contains(puyosArray[posY, posX - 1]))
                {
                    if (puyosArray[posY, posX].GetColor() == puyosArray[posY, posX - 1].GetColor())
                    {
                        continuosPuyosArray[numberOfContinuosPuyos] = puyosArray[posY, posX - 1];
                        numberOfContinuosPuyos++;
                        numberOfContinuosPuyos = RecursiveCheck(numberOfContinuosPuyos, posY, posX - 1);
                    }
                }
            }
        }

        if (posY < arrayYSize - 1)
        {
            if (puyosArray[posY + 1, posX] != null)
            {
                if (!continuosPuyosArray.Contains(puyosArray[posY + 1, posX]))
                {
                    if (puyosArray[posY, posX].GetColor() == puyosArray[posY + 1, posX].GetColor())
                    {
                        continuosPuyosArray[numberOfContinuosPuyos] = puyosArray[posY + 1, posX];
                        numberOfContinuosPuyos++;
                        numberOfContinuosPuyos = RecursiveCheck(numberOfContinuosPuyos, posY + 1, posX);
                    }
                }
            }
        }

        if (posX < arrayXSize - 1)
        {
            if (puyosArray[posY, posX + 1] != null)
            {
                if (!continuosPuyosArray.Contains(puyosArray[posY, posX + 1]))
                {
                    if (puyosArray[posY, posX].GetColor() == puyosArray[posY, posX + 1].GetColor())
                    {
                        continuosPuyosArray[numberOfContinuosPuyos] = puyosArray[posY, posX + 1];
                        numberOfContinuosPuyos++;
                        numberOfContinuosPuyos = RecursiveCheck(numberOfContinuosPuyos, posY, posX + 1);
                    }
                }
            }
        }

        return numberOfContinuosPuyos;
    }

    public bool CanMoveHorizontal(int newPosX, int posY)
    {
        if (posY <= 0)
        {
            return true;
        }

        if (newPosX > arrayXSize - 1)
        {
            return false;
        }
        else if (newPosX < 0)
        {
            return false;
        }
        else
        {
            if (puyosArray[posY, newPosX] != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    public int CellToFallTo(int posY, int posX)
    {
        if (posY + 1 > arrayYSize - 1)
        {
            return 0;
        }
        else if (puyosArray[posY + 1, posX] != null)
        {
            return 0;
        }
        else
        {
            for (int i = posY + 1; i < arrayYSize; i++)
            {
                if (puyosArray[i, posX] != null)
                {
                    return i - 1;
                }
            }

            return arrayYSize - 1;
        }
    }

    public bool CanMoveVertical(int newPosY, int posX)
    {
        if (newPosY <= 0)
        {
            return true;
        }

        if (newPosY > arrayYSize - 1)
        {
            return false;
        }
        else
        {
            if (puyosArray[newPosY, posX] != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    public bool CanRotateLeft(int newPosition, int posY)
    {
        if (newPosition > arrayXSize - 1)
        {
            return false;
        }
        else if (newPosition < 0)
        {
            return false;
        }
        else
        {
            if (posY > arrayYSize - 1)
            {
                return false;
            }
            else
            {
                if (puyosArray[posY, newPosition] != null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }

    public bool CanRotateRight(int newPosition, int posY)
    {
        if (newPosition > arrayXSize - 1)
        {
            return false;
        }
        else if (newPosition < 0)
        {
            return false;
        }
        else
        {
            if (posY > arrayYSize - 1)
            {
                return false;
            }
            else
            {
                if (puyosArray[posY, newPosition] != null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }
}
