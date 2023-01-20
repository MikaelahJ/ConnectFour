using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoordinatesOf : MonoBehaviour
{
    int xAxis;
    int yAxis;

    private CheckLines checkLines;
    private void Start()
    {
        checkLines = transform.GetComponent<CheckLines>();
        xAxis = checkLines.grid.GetLength(0);
        yAxis = checkLines.grid.GetLength(1);

        int i = 0;
        for (int y = 0; y < xAxis; y++)
        {
            for (int x = 0; x < yAxis; x++)
            {
                checkLines.grid[y, x] = i;
                i++;
            }
        }
    }

    public void GetCoordinates(int cellNumber)
    {
        for (int y = 0; y < xAxis; y++)
        {
            for (int x = 0; x < yAxis; x++)
            {
                if (checkLines.grid[y, x].Equals(cellNumber))
                {
                    Debug.Log("Hej");
                    checkLines.CheckXAxis(y, x);
                }
            }
        }
    }
}
