using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    private GameObject gridBox;

    private bool greenTurn = true;
    public bool ongoingTurn;

    public Dictionary<int, bool> takenCell = new();

    private void Start()
    {
        gridBox = GameObject.Find("GridBox");

        if (greenTurn)
            RotateBoxLeft();
        else if(!greenTurn)
            RotateBoxRight();
    }

    private void RotateBoxLeft()
    {
        gridBox.transform.Rotate(0, 0, 20);
    }
    private void RotateBoxRight()
    {
        gridBox.transform.Rotate(0, 0, -20);

    }

    public void CheckWin()
    {
        foreach (KeyValuePair<int, bool> kvp in takenCell)
        {
            Debug.LogFormat("takenCell: {0} - {1}", kvp.Key, kvp.Value);
        }
    }

}
