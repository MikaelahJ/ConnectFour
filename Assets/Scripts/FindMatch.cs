using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    public List<PlayerGameInfo> players;
    public int maxPlayers = 2;
    public string displayName;
    public string playersTurn;
}

public class PlayerGameInfo
{
    public string username;
    public string ID;
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
        db.RootReference.Child("matchmaking").ValueChanged += matchValueChanged;
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

    private void matchValueChanged(object sender, ValueChangedEventArgs e)
    {
        if (!gameFound)
        {
            Dictionary<string, object> matchmakingQueue = (Dictionary<string, object>)e.Snapshot.Value;
            if (matchmakingQueue != null && matchmakingQueue.Count >= 2)
            {
                List<string> playersInQueue = matchmakingQueue.Keys.ToList();

                text.text = "Match Found";
                StartGame(playersInQueue);
                gameFound = true;
            }
        }
    }

    private void StartGame(List<string> playersInQueue)
    {//if this user = första i listan skapa game
        string gameID = Guid.NewGuid().ToString();
        GameData gameData = new GameData();
        gameData.gameID = gameID;
        gameData.players = new List<PlayerGameInfo>();

        for (int i = 0; i < 2; i++)
        {
            if (playersInQueue[0] != FirebaseManager.Instance.GetAuth.CurrentUser.UserId)
                return;
            RemoveFromQueue(playerID);
            PlayerGameInfo playerGameInfo = new PlayerGameInfo();
            playerGameInfo.username = username;
            playerGameInfo.ID = playerID;
            gameData.players.Add(playerGameInfo);

            db.RootReference.Child("games").Child(playerID).GetValueAsync().ContinueWith(task =>
            {
                if (task.Exception != null)
                {
                    Debug.Log(task.Exception);
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    if (snapshot.Exists)
                    {
                        // PlayerID exists in the "games" node
                        Debug.Log("hej");
                        
                    }
                    else
                    {
                        Debug.Log("hej2");

                        // PlayerID does not exist in the "games" node
                    }
                }
            });
        }

        Debug.Log(gameData.players[0].ID);

        string json = JsonUtility.ToJson(gameData);
        FirebaseManager.Instance.CreateNewMatch(json, gameID);
    }

    private void RemoveFromQueue(string playerID)
    {
        db.RootReference.Child("matchmaking").Child(playerID).RemoveValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
            {
                Debug.Log(task.Exception);
            }
            else
            {
                Debug.Log("removed from queue");
            }
        });
    }
}
