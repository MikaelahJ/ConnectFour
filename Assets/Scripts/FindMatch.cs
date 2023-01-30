using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindMatch : MonoBehaviour
{
    [SerializeField] private GameObject matchPrefab;
    [SerializeField] private GameObject matchHolder;


    public void CreateNewMatch()
    {
        Instantiate(matchPrefab, matchHolder.transform);
    }

    public void RefreshList()
    {

    }

    public void JoinMatch()
    {

    }
}
