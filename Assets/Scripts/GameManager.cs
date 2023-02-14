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
    }
    #endregion

    FirebaseDatabase db;


    [SerializeField] private GameObject greenCanon;
    [SerializeField] private GameObject purpleCanon;

    [SerializeField] private Canvas winCanvas;


    public Camera cam;

    public Dictionary<string, bool> playerColour = new Dictionary<string, bool>();
    public string playerOneID;
    public string playerTwoID;

    public bool greenTurn = true;
    public bool isLocalPlayerTurn;
    private bool arePlayersSet;
    private string playerID;

    public string winner;

    private float timer = 0;
    private bool timerRunning;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            cam = Camera.main;

            db = FirebaseManager.Instance.db;
            playerID = FirebaseManager.Instance.auth.CurrentUser.UserId;

            //db.GetReference("games/" + FirebaseManager.Instance.currentGameID + "greenTurn").ValueChanged += GetTurn;
            db.RootReference.Child("games/" + FirebaseManager.Instance.currentGameID + "/greenTurn").ValueChanged += GetTurn;

            //FirebaseManager.Instance.LoadGameData("games/" + FirebaseManager.Instance.currentGameID, SetPlayers);
        }
    }

    public void GetTurn(object sender, ValueChangedEventArgs e)
    {
        if (timerRunning) { return; }
        StartCoroutine(StartTimer());

        if (e.DatabaseError != null)
        {
            Debug.LogError(e.DatabaseError.Message);
            return;
        }

        Debug.Log("vafan");
        FirebaseManager.Instance.LoadGameData("games/" + FirebaseManager.Instance.currentGameID, ChangeTurn);
    }

    public void ChangeTurn(DataSnapshot snap)
    {
        var loadedGame = JsonUtility.FromJson<GameData>(snap.GetRawJsonValue());

        if (loadedGame == null)
        {
            FirebaseManager.Instance.LoadGameData("games/" + FirebaseManager.Instance.currentGameID, ChangeTurn);
            return;
        }

        if (!arePlayersSet)
        {
            SetPlayers(loadedGame);
        }
        Debug.Log(loadedGame.greenTurn);
        Debug.Log(loadedGame.playerOneID);

        if (loadedGame.greenTurn)
        {
            if (playerID == playerOneID)
            {
                Debug.Log("player 1 turn");

                cam.gameObject.GetComponent<CameraMover>().isGreenTurn = true;
                GreenTurn();
            }
        }
        else
        {
            if (playerID == playerTwoID)
            {
                Debug.Log("player 2 turn");

                cam.gameObject.GetComponent<CameraMover>().isGreenTurn = false;
                PurpleTurn();
            }
        }
        Debug.Log(loadedGame.greenTurn);

    }

    private void SetPlayers(GameData loadedGame)
    {
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
        {
            winner = "Green";
            //FirebaseManager.Instance.AddPointToUser(playerOneID);
        }
        if (purple >= 3)
        {
            winner = "Purple";
            //FirebaseManager.Instance.AddPointToUser(playerTwoID);
        }

        winCanvas.gameObject.SetActive(true);
    }


    private IEnumerator StartTimer()
    {
        timerRunning = true;
        yield return new WaitForSeconds(0.1f);
        timerRunning = false;
    }
}