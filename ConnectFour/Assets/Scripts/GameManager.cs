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

    private bool greenTurn = true;
    public bool ongoingTurn;

    private int greenAmountInLine = 0;
    private int purpleAmountInLine = 0;

    public Dictionary<int, bool> takenCell = new();

    private void Start()
    {
        gridBox = GameObject.Find("GridBox");

        //if (greenTurn)
        //    RotateBoxLeft();
        //else if (!greenTurn)
        //    RotateBoxRight();
    }

    private void RotateBoxLeft()
    {
        gridBox.transform.Rotate(0, 0, 20);
    }
    private void RotateBoxRight()
    {
        gridBox.transform.Rotate(0, 0, -20);
    }

    public void StartCheckWin(int cellNumber)
    {
        foreach (KeyValuePair<int, bool> kvp in takenCell)
        {
            Debug.LogFormat("takenCell: {0} - {1}", kvp.Key, kvp.Value);
        }

        StartCoroutine(CheckXAxis(cellNumber));
    }

    private IEnumerator CheckXAxis(int cellNumber)
    {
        yield return new WaitForSeconds(0.2f);
        //                                                                                  -1  +7  +1
        //kolla rad +-1 och collumn +-7 samt +-1 på cellen ovanför och under för diagonalen -1 cell +1
        //                                                                                  -1  -7  +1

        int cellToCheck = cellNumber - 1;
        for (int i = 0; i < 2; i++)
        {
            if (i == 1)
                cellToCheck = cellNumber + 1;

            if (i == 0 && cellNumber % 7 == 0)//Check if at left/right wall, doesn't need to check x axis
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


        //for (int i = 0; i < 4; i++)//check for line y,x axis
        //{
        //    if (i == 0)
        //    {
        //        if (cellNumber % 7 == 0)//Check if at left wall, doesn't need to check for dots to the left
        //        {
        //            continue;
        //        }
        //    }
        //    else if (i == 1)
        //    {

        //        cellToCheck = cellNumber + 1;
        //    }
        //    else if (i == 2)
        //    {
        //        if (cellNumber - 7 < 0)//Check if at top
        //        {
        //            continue;
        //        }
        //        cellToCheck = cellNumber - 7;
        //    }
        //    else if (i == 3)
        //    {
        //        if (cellNumber + 7 > 41)//Check if at top
        //        {
        //            continue;
        //        }
        //        cellToCheck = cellNumber + 7;
        //    }




        //    if (takenCell.ContainsKey(cellToCheck))//is the cell taken
        //    {
        //        if (takenCell[cellToCheck] == takenCell[cellNumber])//if true then same color of dot
        //        {
        //            if (takenCell[cellNumber])
        //                greenAmountInLine++;
        //            else
        //                purpleAmountInLine++;

        //            Debug.Log("Green amountinLine " + greenAmountInLine);
        //            Debug.Log("Purple amountinLine " + purpleAmountInLine);
        //        }
        //    }
        //    if (i == 2 || i == 3)//check diagonal up
        //    {
        //        int diagonalCellCheck = cellToCheck - 1;

        //        if (takenCell.ContainsKey(diagonalCellCheck))
        //        {

        //        }
        //    }
        //}
    }
    private void CheckYAxis(int cellNumber)
    {
        Debug.Log("check y axis");
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