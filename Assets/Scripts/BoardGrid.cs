using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardGrid : MonoBehaviour
{
    private Dictionary<GameObject, int> cells = new();

    void Start()
    {
        int i = 0;
        foreach (BoxCollider2D cell in transform.GetComponentsInChildren<BoxCollider2D>())
        {
            if (cell.gameObject.CompareTag("Cell"))
            {
                cells.Add(cell.gameObject, i);
                i++;
            }
        }
    }

    public void SetCellTaken(GameObject cell, GameObject plupp, float[] xPos, float[] yPos)
    {
        bool color = false;
        if (plupp.CompareTag("Green"))//if color green = true, purple = false
        {
            color = true;
        }
        
        transform.GetComponent<CheckLines>().takenCell.Add(cells[cell], color);
        transform.GetComponent<CoordinatesOf>().GetCoordinates(cells[cell]);

        Debug.Log(cell);
        FirebaseManager.Instance.SaveBallPath(xPos, yPos, cells[cell]);

        //foreach (KeyValuePair<int, bool> kvp in transform.GetComponent<CheckLines>().takenCell)
        //{
        //    Debug.LogFormat("TakenCell: {0} - {1}", kvp.Key, kvp.Value);
        //}
    }
}