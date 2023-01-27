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

    public void SaveToFile(string fileName, string jsonString)
    {
        // Open a file in write mode. This will create the file if it's missing.
        // It is assumed that the path already exists.
        //using (var stream = File.OpenWrite(fileName))
        //{
        //    // Truncate the file if it exists (we want to overwrite the file)
        //    stream.SetLength(0);

        //    // Convert the string into bytes. Assume that the character-encoding is UTF8.
        //    // Do you not know what encoding you have? Then you have UTF-8
        //    var bytes = Encoding.UTF8.GetBytes(jsonString);

        //    // Write the bytes to the hard-drive
        //    stream.Write(bytes, 0, bytes.Length);

        //    // The "using" statement will automatically close the stream after we leave
        //    // the scope - this is VERY important
        //}
    }

    //Return the content of the file in a string
    //public string LoadFromFile(string fileName)
    //{
    //    // Open a stream for the supplied file name as a text file
    //    using (var stream = File.OpenText(fileName))
    //    {
    //        // Read the entire file and return the result. This assumes that we've written the
    //        // file in UTF-8
    //        return stream.ReadToEnd();
    //    }
    //}
}
