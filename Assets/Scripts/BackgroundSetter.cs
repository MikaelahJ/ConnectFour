using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundSetter : MonoBehaviour
{
    [SerializeField] private List<Sprite> backgroundSprites = new List<Sprite>();

    void Start()
    {
        GetComponentInChildren<Image>().sprite = backgroundSprites[Random.Range(0, backgroundSprites.Count)];
    }

}
