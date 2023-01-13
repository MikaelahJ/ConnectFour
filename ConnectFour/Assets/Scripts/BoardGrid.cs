using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardGrid : MonoBehaviour
{
    private Dictionary<GameObject, int> cells = new();

    void Start()
    {
        int i = 0;
        foreach(BoxCollider2D cell in transform.GetComponentsInChildren<BoxCollider2D>())
        {
            if (cell.gameObject.CompareTag("Cell"))
            {
                cells.Add(cell.gameObject, i);
                i++;
                //Debug.Log(cells[cell.gameObject]);
            }
        }
    }

    public void SetCellTaken(GameObject cell, GameObject plupp)
    {
        bool color = false;
        if (plupp.CompareTag("Green")) //if color green = true, purple = false
        {
            color = true;
        }
        Debug.Log("cell " + cells[cell]);
        Debug.Log("plupp " + plupp.tag);
        GameManager.Instance.takenCell.Add(cells[cell], color);
        GameManager.Instance.CheckWin();
    }
}
