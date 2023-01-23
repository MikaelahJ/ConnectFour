using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region singleton
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
    #endregion

    private GameObject greenCanon;
    private GameObject purpleCanon;

    public Camera cam;

    public bool greenTurn = true;
    public bool ongoingTurn;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            cam = Camera.main;

            greenCanon = GameObject.Find("GreenCanon");
            purpleCanon = GameObject.Find("PurpleCanon");

            GreenTurn();
        }
    }

    public void ChangeTurn()
    {
        if (greenTurn)
        {
            PurpleTurn();
            greenTurn = false;
            cam.gameObject.GetComponent<CameraMover>().isGreenTurn = false;
        }
        else
        {
            greenTurn = true;
            cam.gameObject.GetComponent<CameraMover>().isGreenTurn = true;
            GreenTurn();
        }
    }

    private void GreenTurn()
    {
        greenCanon.GetComponent<Cannon>().GetPlupp();
    }

    private void PurpleTurn()
    {
        purpleCanon.GetComponent<Cannon>().GetPlupp();
    }    

    public void ShowWinner(int green, int purple)
    {
        if (green >= 3)
            Debug.Log("green wins");

        if (purple >= 3)
            Debug.Log("purple wins");
    }
}