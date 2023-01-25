using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

[Serializable]
class PlayerData
{
    public string Name;
    public string Email;
    public string Password;
    public int Wins;

}
public class PlayerDataManager : MonoBehaviour
{
    #region singleton
    public static PlayerDataManager Instance = null;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }
    #endregion

    private const string PLAYER_EMAIL_KEY = "PLAYER_EMAIL";

    private void Start()
    {
        
    }

    public string GetEmail()
    {
        if (PlayerPrefs.HasKey(PLAYER_EMAIL_KEY))
        {
            name = PlayerPrefs.GetString(PLAYER_EMAIL_KEY);
            return name;
        }
        else
            return "No email found";
    }

    public void SavePlayerInlog(string email, string password)
    {
        PlayerPrefs.SetString(PLAYER_EMAIL_KEY, email);
        PlayerData saveData = new PlayerData();

        //saveData.Email = email;
        saveData.Password = password;

        string jsonString = JsonUtility.ToJson(saveData);

        PlayerPrefs.SetString("PlayerSaveData", jsonString);
    }
}
