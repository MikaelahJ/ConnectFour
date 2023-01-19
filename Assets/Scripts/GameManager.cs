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

    bool bottom = false;
    bool top = false;
    bool right = false;
    bool left = false;

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

    public void StartCheckWin(int x, int y)
    {
        // -1  +7  +1
        // -1 cell +1
        // -1  -7  +1

        foreach (KeyValuePair<int, bool> kvp in takenCell)
        {
            //Debug.LogFormat("takenCell: {0} - {1}", kvp.Key, kvp.Value);
        }

        grid[1, 1] = 2;
        int[] test = new int[2] { 1, 1 };
        //Debug.Log(grid.GetValue(test));

        StartCoroutine(CheckXAxis(x, y));
    }

    private IEnumerator CheckXAxis(int y, int x)
    {
        yield return new WaitForSeconds(0.2f);
        // -1 cell +1
        cellToCheck = x + 1;

        for (int i = 0; i < 2; i++)
        {
            if (i == 0 && x == 6)//Check if at right wall
            {
                continue;
            }

            if (i == 1)
                cellToCheck = x - 1;

            if (i == 1 && x == 0)//Check if at left wall
                continue;

            if (takenCell.ContainsKey(grid[y, cellToCheck]))//is the cell taken
            {
                while (takenCell.ContainsKey(grid[y, cellToCheck]))
                {
                    if (takenCell[grid[y, cellToCheck]] == takenCell[grid[y, x]])//if true then same color of dot
                    {
                        if (takenCell[grid[y, x]])//if true then green 
                            greenAmountInLine++;

                        else//else purple
                            purpleAmountInLine++;

                        if (greenAmountInLine >= 3 || purpleAmountInLine >= 3)
                        {
                            ShowWinner();
                            yield break;
                        }

                        if (i == 0)
                        {
                            cellToCheck++;
                            if (cellToCheck % 7 == 0)
                            {
                                break;
                            }
                        }

                        if (i == 1)
                        {
                            if (cellToCheck == 0)
                                break;

                            cellToCheck--;
                            if (cellToCheck < 0)
                            {
                                break;
                            }
                        }
                    }
                }
            }
        }
        ResetLineAmounts();
        CheckYAxis(y, x);
    }

    private void CheckYAxis(int y, int x)
    {   //  +1  
        // cell
        //  -1 
        cellToCheck = y + 1;

        for (int i = 0; i < 2; i++)
        {
            if (i == 1)
                cellToCheck = y - 1;

            if (cellToCheck < 0 || cellToCheck > 5)//check if outside top and bottom
                continue;

            if (takenCell.ContainsKey(grid[cellToCheck, x]))//is the cell taken
            {
                while (takenCell.ContainsKey(grid[cellToCheck, x]))
                {
                    if (takenCell[grid[cellToCheck, x]] == takenCell[grid[y, x]])//if true then same color of dot
                    {
                        if (takenCell[grid[y, x]])//green
                            greenAmountInLine++;

                        else//purple
                            purpleAmountInLine++;

                        if (greenAmountInLine >= 3 || purpleAmountInLine >= 3)
                        {
                            ShowWinner();
                            break;
                        }
                        if (i == 0)
                        {
                            cellToCheck++;
                        }

                        if (i == 0)
                            cellToCheck++;

                        if (i == 1)
                            cellToCheck--;

                        if (cellToCheck < 0 || cellToCheck > 5)
                            break;
                    }
                }
            }
        }
        ResetLineAmounts();
        CheckDiagonal(y, x);
    }


    private void CheckDiagonal(int y, int x)
    {
        int step = 1;//each step is the next cell in diagonal line

        for (int i = 0; i < 4; i++)
        {
            SetDiagonalCellToCheck(i, y, x, step);

            Debug.Log(cellToCheck);

            if (takenCell.ContainsKey(cellToCheck))//is the cell taken
            {
                while (takenCell.ContainsKey(cellToCheck))
                {
                    if (takenCell[cellToCheck] == takenCell[grid[y, x]])//if true then same color of dot
                    {
                        if (takenCell[grid[y, x]])//green
                            greenAmountInLine++;

                        else//purple
                            purpleAmountInLine++;
                        Debug.Log("GreenAmountInLine " + greenAmountInLine);
                        Debug.Log("PurpleAmountInLine " + purpleAmountInLine);
                        if (greenAmountInLine >= 3 || purpleAmountInLine >= 3)
                        {
                            ShowWinner();
                            break;
                        }
                    }
                    step++;
                    SetDiagonalCellToCheck(i, y, x, step);
                }
            }
            step = 1;
            if (i == 1)
                ResetLineAmounts();
        }
    }

    private void SetDiagonalCellToCheck(int i, int y, int x, int step)
    {
        bottom = false;
        top = false;
        right = false;
        left = false;

        //check if at a border
        if (y + step > 5)
            bottom = true;
        if (y - step < 0)
            top = true;
        if (x + step > 6)
            right = true;
        if (x - step < 0)
            left = true;

        switch (i)//set cellToCheck direction
        {
            case 0: //Check up-right /
                if (!top && !right)
                    cellToCheck = grid[y - step, x + step];
                break;

            case 1://Check down-left /
                if (!bottom && !left)
                    cellToCheck = grid[y + step, x - step];
                break;

            case 2://Check up-left \
                if (!top && !left)
                    cellToCheck = grid[y - step, x - step];
                break;

            case 3://Check down-right \
                if (!bottom && !right)
                    cellToCheck = grid[y + step, x + step];
                break;
        }
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