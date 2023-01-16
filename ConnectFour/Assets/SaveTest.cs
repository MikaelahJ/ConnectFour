using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SaveTest : MonoBehaviour
{
    private const string PLAYER_NAME_KEY = "PLAYER_NAME";
    public Slider slider;
    public TMP_InputField inputField;
    public Button button;

    string name;
    float color;

    void Start()
    {
        if (PlayerPrefs.HasKey(PLAYER_NAME_KEY))
        {
            name = PlayerPrefs.GetString(PLAYER_NAME_KEY);
            inputField.text = name;
        }

        inputField.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }

    private void ValueChangeCheck()
    {
        PlayerPrefs.SetString(PLAYER_NAME_KEY, inputField.text);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            PlayerPrefs.DeleteAll();
        }
    }
}
