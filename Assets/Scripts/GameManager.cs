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

    private GameObject gridBox;
    private GameObject greenCanon;
    private GameObject purpleCanon;

    public Camera cam;

    public bool greenTurn = true;
    public bool ongoingTurn;



    private int cellToCheck = 0;
    private int greenAmountInLine = 0;
    private int purpleAmountInLine = 0;

    public Dictionary<int, bool> takenCell = new();
    public int[,] grid = new int[6, 7];

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            cam = Camera.main;

            gridBox = GameObject.Find("GridBox");
            greenCanon = GameObject.Find("GreenCanon");
            purpleCanon = GameObject.Find("PurpleCanon");

            GreenTurn();

        }


        //if (greenTurn)
        //    RotateBoxLeft();
        //else if (!greenTurn)
        //    RotateBoxRight();
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
        Debug.Log("hej");
        purpleCanon.GetComponent<Cannon>().GetPlupp();
    }

    public void StartCheckWin(int cellNumber)
    {
        foreach (KeyValuePair<int, bool> kvp in takenCell)
        {
            Debug.LogFormat("takenCell: {0} - {1}", kvp.Key, kvp.Value);
        }

        grid[1, 1] = 2;
        int[] test = new int[2] { 1, 1 };
        //Debug.Log(grid.GetValue(test));

        StartCoroutine(CheckXAxis(cellNumber));
    }

    private IEnumerator CheckXAxis(int cellNumber)
    {
        yield return new WaitForSeconds(0.2f);
        // -1  +7  +1
        // -1 cell +1
        // -1  -7  +1

        cellToCheck = cellNumber - 1;
        for (int i = 0; i < 2; i++)
        {
            if (i == 1)
                cellToCheck = cellNumber + 1;

            if (i == 0 && cellNumber % 7 == 0)//Check if at left/right wall
            {
                continue;
            }

            bool isAtRightWall = false;
            if (cellNumber == 6 || cellNumber == 13 || cellNumber == 20 || cellNumber == 27 || cellNumber == 33 || cellNumber == 40)
            {
                isAtRightWall = true;
            }

            if (i == 1 && isAtRightWall)
            {
                continue;
            }

            if (takenCell.ContainsKey(cellToCheck))//is the cell taken
            {
                while (takenCell.ContainsKey(cellToCheck))
                {
                    if (takenCell[cellToCheck] == takenCell[cellNumber])//if true then same color of dot
                    {
                        if (takenCell[cellNumber])
                            greenAmountInLine++;

                        else
                            purpleAmountInLine++;

                        Debug.Log("Green amountinLine " + greenAmountInLine);
                        Debug.Log("Purple amountinLine " + purpleAmountInLine);

                        if (greenAmountInLine >= 3 || purpleAmountInLine >= 3)
                        {
                            ShowWinner();
                            yield break;
                        }
                        Debug.Log("celltoCheck " + cellToCheck);
                        if (i == 0)
                        {
                            cellToCheck--;
                            if (cellToCheck < 0)
                            {
                                break;
                            }
                        }

                        if (i == 1)
                        {
                            cellToCheck++;
                            if (cellToCheck == 7 || cellToCheck == 14 || cellToCheck == 21 || cellToCheck == 28 || cellToCheck == 35 || cellToCheck == 42)
                            {
                                break;
                            }
                        }
                    }
                }
            }
        }
        ResetLineAmounts();
        CheckYAxis(cellNumber);
    }

    private void CheckYAxis(int cellNumber)
    {
        cellToCheck = cellNumber - 7;
        for (int i = 0; i < 2; i++)
        {
            if (i == 1 && cellNumber + 7 < 42)
                cellToCheck = cellNumber + 7;

            if (i == 0 && cellNumber - 7 < 0)
                continue;

            if (takenCell.ContainsKey(cellToCheck))//is the cell taken
            {
                while (takenCell.ContainsKey(cellToCheck))
                {
                    if (takenCell[cellToCheck] == takenCell[cellNumber])//if true then same color of dot
                    {
                        if (takenCell[cellNumber])
                            greenAmountInLine++;

                        else
                            purpleAmountInLine++;

                        Debug.Log("Green amountinLine " + greenAmountInLine);
                        Debug.Log("Purple amountinLine " + purpleAmountInLine);

                        if (greenAmountInLine >= 3 || purpleAmountInLine >= 3)
                        {
                            ShowWinner();
                            break;
                        }
                        Debug.Log("celltoCheck " + cellToCheck);
                        if (i == 0)
                        {
                            cellToCheck -= 7;
                            if (cellToCheck < 0)
                            {
                                break;
                            }
                        }

                        if (i == 1)
                        {
                            cellToCheck += 7;
                            if (cellToCheck > 42)
                            {
                                break;
                            }
                        }
                    }
                }
            }
        }
        ResetLineAmounts();
        CheckDiagonal(cellNumber);
    }

    private void CheckDiagonal(int cellNumber)
    {
        Debug.Log("checkDiagonal");
    }

    private void ResetLineAmounts()
    {
        greenAmountInLine = 0;
        purpleAmountInLine = 0;
    }
    private void ShowWinner()
    {
        if (greenAmountInLine >= 3)
            Debug.Log("green wins");

        if (purpleAmountInLine >= 3)
            Debug.Log("purple wins");
    }

}