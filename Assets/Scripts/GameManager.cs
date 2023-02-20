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

    public bool greenTurn;
    public bool isLocalPlayerTurn;
    private bool arePlayersSet;
    private string playerID;

    public string winner;

    private bool timerRunning;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            greenTurn = true;

            cam = Camera.main;

            db = FirebaseManager.Instance.db;
            playerID = FirebaseManager.Instance.auth.CurrentUser.UserId;

            db.RootReference.Child("games/" + FirebaseManager.Instance.currentGameID + "/greenTurn").ValueChanged += GetTurn;
        }
    }

    public void GetTurn(object sender, ValueChangedEventArgs e)
    {
        if (e.DatabaseError != null)
        {
            Debug.LogError(e.DatabaseError.Message);
            return;
        }

        FirebaseManager.Instance.LoadGameData("games/" + FirebaseManager.Instance.currentGameID, SetTurn);
    }

    public void SetTurn(DataSnapshot snap)
    {
        var loadedGame = JsonUtility.FromJson<GameData>(snap.GetRawJsonValue());
        greenTurn = loadedGame.greenTurn;

        if (loadedGame.gameID == null)
        {
            FirebaseManager.Instance.LoadGameData("games/" + FirebaseManager.Instance.currentGameID, SetTurn);
            Debug.Log("No game found");
            return;
        }

        if (!arePlayersSet)
            SetPlayers(loadedGame);

        //show other players move

        if (loadedGame.greenTurn)
        {
            if (playerID == playerOneID)
            {
                FirebaseManager.Instance.LoadGameData("games/" + FirebaseManager.Instance.currentGameID + "/ballPath/", ShowOpponentBallPath);

                Debug.Log("player 1 turn");
            }
            //cam.gameObject.GetComponent<CameraMover>().isGreenTurn = true;
        }
        else
        {
            if (playerID == playerTwoID)
            {
                FirebaseManager.Instance.LoadGameData("games/" + FirebaseManager.Instance.currentGameID + "/ballPath/", ShowOpponentBallPath);

                Debug.Log("player 2 turn");
            }
            //cam.gameObject.GetComponent<CameraMover>().isGreenTurn = false;
        }
    }

    private void ShowOpponentBallPath(DataSnapshot snap)
    {
        var ballPath = JsonUtility.FromJson<PlayerMove>(snap.GetRawJsonValue());

        Debug.Log(ballPath.xPos[1]);
        Debug.Log(ballPath);

        if (ballPath.xPos.Length <= 1)
        {
            if(greenTurn) GreenTurn();
            else PurpleTurn();
        }

        if (greenTurn)
        {
            purpleCanon.GetComponent<Cannon>().ShowGhostPlupp(ballPath.xPos, ballPath.yPos, ballPath.cell);
        }
        else if (!greenTurn)
        {
            greenCanon.GetComponent<Cannon>().ShowGhostPlupp(ballPath.xPos, ballPath.yPos, ballPath.cell);
        }
    }

    private void SetPlayers(GameData loadedGame)
    {
        playerOneID = loadedGame.playerOneID;
        playerTwoID = loadedGame.playerTwoID;

        arePlayersSet = true;
    }

    public void GreenTurn()
    {
        greenCanon.GetComponent<Cannon>().GetPlupp();
    }

    public void PurpleTurn()
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
}