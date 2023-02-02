using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WinScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI winnerText; 

    void Start()
    {
        winnerText.text = GameManager.Instance.winner;
    }
    

    
}
