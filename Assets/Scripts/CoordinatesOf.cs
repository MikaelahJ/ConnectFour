using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoordinatesOf : MonoBehaviour
{
    int xAxis;
    int yAxis;

    private void Start()
    {
        xAxis = GameManager.Instance.grid.GetLength(0);
        yAxis = GameManager.Instance.grid.GetLength(1);

        int i = 0;
        for (int y = 0; y < xAxis; y++)
        {
            for (int x = 0; x < yAxis; x++)
            {
                GameManager.Instance.grid[y, x] = i;
                i++;
                //Debug.Log("X " + x);
                //Debug.Log("Y " + y);
            }
        }
    }

    public void GetCoordinates(int cellNumber)
    {
        for (int y = 0; y < xAxis; y++)
        {
            for (int x = 0; x < yAxis; x++)
            {
                if (GameManager.Instance.grid[y, x].Equals(cellNumber))
                {
                    GameManager.Instance.StartCheckWin(y, x);
                }
            }
        }
    }
}
