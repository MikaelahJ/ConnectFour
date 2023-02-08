using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase.Database;

public class GameManager : MonoBehaviour
{
    #region singleton
    public static GameManager Instance = null;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }
    #endregion
    FirebaseDatabase db;


    private GameObject greenCanon;
    private GameObject purpleCanon;


    public Camera cam;

    public Dictionary<string, bool> playerColour = new Dictionary<string, bool>();
    public string playerOneID;
    public string playerTwoID;

    public bool greenTurn = true;
    public bool isLocalPlayerTurn;
    private bool arePlayersSet;
    private string playerID;

    public string winner;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            cam = Camera.main;
            greenCanon = GameObject.Find("GreenCanon");
            purpleCanon = GameObject.Find("PurpleCanon");

            db = FirebaseManager.Instance.db;
            playerID = FirebaseManager.Instance.auth.CurrentUser.UserId;

            //GetTurnFirst();
            db.RootReference.Child("games/" + FirebaseManager.Instance.currentGameID + "greenTurn").ValueChanged += GetTurn;

            //FirebaseManager.Instance.LoadGameData("games/" + FirebaseManager.Instance.currentGameID, SetPlayers);
        }
    }

    public void GetTurn(object sender, ValueChangedEventArgs e)
    {
        Debug.Log("hejsan1");

        if (e.DatabaseError != null)
        {
            Debug.LogError(e.DatabaseError.Message);
            return;
        }

        Debug.Log("Value has changed: " + e.Snapshot.GetRawJsonValue());

        Debug.Log("vafan");
        ChangeTurn(e.Snapshot);
        //FirebaseManager.Instance.LoadGameData("games/" + FirebaseManager.Instance.currentGameID, ChangeTurn);

    }

    public void ChangeTurn(DataSnapshot snap)
    {
        var loadedGame = JsonUtility.FromJson<GameData>(snap.GetRawJsonValue());

        if (!arePlayersSet)
        {
            SetPlayers(loadedGame);
        }
        Debug.Log("hejsan2");
        Debug.Log("greenturn " + loadedGame.greenTurn);
        if (loadedGame.greenTurn)
        {
            Debug.Log("hej");

            if (playerOneID == playerID)
            {



                Debug.Log("hej2");

                cam.gameObject.GetComponent<CameraMover>().isGreenTurn = true;
                GreenTurn();
            }
        }
        else
        {
            Debug.Log("hej3");
            if (playerTwoID == playerID)
            {

                Debug.Log("hej4");


                cam.gameObject.GetComponent<CameraMover>().isGreenTurn = false;
                PurpleTurn();
            }
        }
    }

    private void SetPlayers(GameData loadedGame)
    {
        //var loadedGame = JsonUtility.FromJson<GameData>(snap.GetRawJsonValue());

        playerOneID = loadedGame.playerOneID;
        playerTwoID = loadedGame.playerTwoID;

        arePlayersSet = true;
    }

    private void GreenTurn()
    {
        greenCanon.GetComponent<Cannon>().GetPlupp();
    }

    private void PurpleTurn()
    {
        purpleCanon.GetComponent<Cannon>().GetPlupp();
    }

    public void ShowWinner(int green, int purple)
    {
        if (green >= 3)
            winner = "Green";

        if (purple >= 3)
            winner = "Purple";
    }
}