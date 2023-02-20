using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Firebase.Auth;
using Firebase.Database;
using Firebase;
using Firebase.Extensions;
using System;
using System.Linq;

public class GameData
{
    public string gameID;
    public string displayName;
    public bool greenTurn;
    public string playerOneID;
    public string playerTwoID;
}

public class FindMatch : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    FirebaseDatabase db;

    public string playerID;
    public string username;

    private bool gameFound;

    private void Start()
    {
        db = FirebaseManager.Instance.db;

        JoinQueue(playerID);
        db.RootReference.Child("matchmaking").ValueChanged += MatchValueChanged;
    }

    public void JoinQueue(string playerID)
    {
        db.RootReference.Child("matchmaking/").Child(playerID).SetValueAsync("placeholder")
            .ContinueWithOnMainThread(task =>
            {
                if (task.Exception != null)
                {
                    Debug.Log(task.Exception);
                }
                else
                {
                    Debug.Log("Successfully joined the queue");
                }
            });
    }

    private void MatchValueChanged(object sender, ValueChangedEventArgs e)
    {
        string firstPlayer = "";
        if (!gameFound)
        {
            Dictionary<string, object> matchmakingQueue = (Dictionary<string, object>)e.Snapshot.Value;

            if (matchmakingQueue != null && matchmakingQueue.Count >= 2)
            {
                firstPlayer = matchmakingQueue.Keys.First();
                if (playerID == matchmakingQueue.Keys.First())
                {
                    List<string> playersInQueue = matchmakingQueue.Keys.ToList();
                    StartGame(playersInQueue);
                }

                text.text = "Match Found";
                gameFound = true;
            }
        }
        if (gameFound)
        {
            Debug.Log(FirebaseManager.Instance.currentGameID);
            Debug.Log(firstPlayer);

            if (FirebaseManager.Instance.currentGameID == null)
                FirebaseManager.Instance.LoadGameData("games/" + firstPlayer, LoadCurrentGame);
            else
                SceneManager.LoadScene(2);
        }
    }

    private void StartGame(List<string> playersInQueue)
    {
        string gameID = playersInQueue[0];
        GameData gameData = new GameData();
        gameData.gameID = gameID;
        gameData.greenTurn = true;

        int i = 0;

        foreach (string player in playersInQueue)
        {
            if (i == 0)
                gameData.playerOneID = player;
            if (i == 1)
                gameData.playerTwoID = player;
            i++;
        }

        string json = JsonUtility.ToJson(gameData);
        FirebaseManager.Instance.CreateNewMatch(json, gameID, playersInQueue);
    }

    private void LoadCurrentGame(DataSnapshot snap)
    {
        var loadedGame = JsonUtility.FromJson<GameData>(snap.GetRawJsonValue());
        FirebaseManager.Instance.currentGameID = loadedGame.gameID;

        SceneManager.LoadScene(2);
    }
}
