using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckLines : MonoBehaviour
{
    private int cellToCheck = 0;
    private int greenAmountInLine = 0;
    private int purpleAmountInLine = 0;

    bool bottom = false;
    bool top = false;
    bool right = false;
    bool left = false;

    public Dictionary<int, bool> takenCell = new();
    public int[,] grid = new int[6, 7];

    public void CheckXAxis(int y, int x)
    {
        Debug.Log("CheckX");
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
                Debug.Log("contains key");

                while (takenCell.ContainsKey(grid[y, cellToCheck]))
                {
                    if (takenCell[grid[y, cellToCheck]] == takenCell[grid[y, x]])//if true then same color of dot
                    {
                        Debug.Log("same color");

                        if (takenCell[grid[y, x]])//if true then green 
                            greenAmountInLine++;

                        else//else purple
                            purpleAmountInLine++;

                        if (greenAmountInLine >= 3 || purpleAmountInLine >= 3)
                        {
                            GameManager.Instance.ShowWinner(greenAmountInLine, purpleAmountInLine);
                            break;
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
                            GameManager.Instance.ShowWinner(greenAmountInLine, purpleAmountInLine);
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
                        if (greenAmountInLine >= 3 || purpleAmountInLine >= 3)
                        {
                            GameManager.Instance.ShowWinner(greenAmountInLine, purpleAmountInLine);
                            break;
                        }
                    }
                    step++;
                    SetDiagonalCellToCheck(i, y, x, step);
                }
            }
            step = 1;
            if (i == 1 || i == 3)
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
}
