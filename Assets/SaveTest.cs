using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

[Serializable]
class PlayerData
{
    public string Name;
    public float ColorHUE;
    public bool Hidden;
    public Vector2 Position;

}

public class SaveTest : MonoBehaviour
{
    private const string PLAYER_NAME_KEY = "PLAYER_NAME";
    public Slider slider;
    public TMP_InputField inputField;
    public Button button;

    void Start()
    {
        if (PlayerPrefs.HasKey(PLAYER_NAME_KEY))
        {
            name = PlayerPrefs.GetString(PLAYER_NAME_KEY);
            inputField.text = name;
        }

        inputField.onValueChanged.AddListener(delegate { ValueChangeCheck(); });

        Save();
    }


    private void ValueChangeCheck()
    {
        PlayerPrefs.SetString(PLAYER_NAME_KEY, inputField.text);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            //PlayerPrefs.DeleteAll();
        }
    }

    void Save()
    {
        PlayerData saveData = new PlayerData();

        saveData.Name = inputField.text;
        saveData.ColorHUE = slider.value;
        saveData.Hidden = false;
        //saveData.Position =player.transform.position;

        string jsonString = JsonUtility.ToJson(saveData);

        PlayerPrefs.SetString("PlayerSaveData", jsonString);

    }
    void Load()
    {
        //Get the saved jsonString
        string jsonString = PlayerPrefs.GetString("PlayerSaveData");

        //Convert the data to a object
        PlayerData loadedData = JsonUtility.FromJson<PlayerData>(jsonString);
        
        //We probably would like to add some code in this function
        //that runs if we get broken or no data.
    }
}
