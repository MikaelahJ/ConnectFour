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
    public Dictionary<string, string> players = new Dictionary<string, string>();
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
                if (playerID == matchmakingQueue.Keys.First())
                {
                    List<string> playersInQueue = matchmakingQueue.Keys.ToList();
                    StartGame(playersInQueue);
                }

                text.text = "Match Found";
                gameFound = true;
            }
        }
    }

    private void StartGame(List<string> playersInQueue)
    {
        string gameID = Guid.NewGuid().ToString();
        GameData gameData = new GameData();
        gameData.gameID = gameID;

        foreach (string player in playersInQueue)
        {
            RemoveFromQueue(player);

            gameData.players.Add(player, gameID);
        }

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
