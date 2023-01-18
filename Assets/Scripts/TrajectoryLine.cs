using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryLine : MonoBehaviour
{
    [SerializeField] private int dotsNumber;
    [SerializeField] private GameObject dotsParent;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private float spacing;

    private Transform[] dotsList;

    private Vector2 pos;
    private float timeStamp;

    private void Start()
    {
        HideDots();
        PrepareDots();
    }

    private void PrepareDots()
    {
        dotsList = new Transform[dotsNumber];
        for (int i = 0; i < dotsNumber; i++)
        {
            dotsList[i] = Instantiate(dotPrefab.transform);
            dotsList[i].parent = dotsParent.transform;
        }
    }

    public void UpdateDots(Vector3 ballPos, Vector2 forceApplied)
    {
        timeStamp = spacing;
        for (int i = 0; i < dotsNumber; i++)
        {
            pos.x = ballPos.x + forceApplied.x * timeStamp;
            pos.y = (ballPos.y + forceApplied.y * timeStamp) - (Physics2D.gravity.magnitude * timeStamp * timeStamp) / 2f;

            dotsList[i].position = pos;
            timeStamp += spacing;
        }
    }

    public void ShowDots()
    {
        dotsParent.SetActive(true);
    }
    public void HideDots()
    {
        dotsParent.SetActive(false);
    }
}
