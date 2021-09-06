using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuyoPuyoSpawner : MonoBehaviour
{
    public static PuyoPuyoSpawner instance = null;

    [SerializeField]
    private GameObject puyoPuyoPrefab;

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

    public void Spawn() 
    {
        Instantiate(puyoPuyoPrefab, transform.position, Quaternion.identity);
    }
}
