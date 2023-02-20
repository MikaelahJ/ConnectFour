using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine.SceneManagement;
using UnityEditor;
using System;

class PlayerMove
{
    public float[] xPos;
    public float[] yPos;
    public int cell;
}

public class FirebaseManager : MonoBehaviour
{
    public FirebaseAuth auth;
    public FirebaseDatabase db;

    public delegate void OnLoadedDelegate(DataSnapshot snapshot);
    public FirebaseAuth GetAuth { get { return auth; } }

    public static FirebaseManager Instance = null;

    public string currentGameID = null;


    private void Awake()
    {
        Instance = this;

        db = FirebaseDatabase.DefaultInstance;
        db.SetPersistenceEnabled(false);

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
                Debug.LogError(task.Exception);

            auth = FirebaseAuth.DefaultInstance;
        });
        SceneManager.LoadScene(1);


    }

    public void SaveUserToFirebase(string data)
    {
        //puts the json data in the "users/userId" part of the database.
        db.RootReference.Child("users").Child(auth.CurrentUser.UserId).SetRawJsonValueAsync(data).ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
                Debug.LogWarning(task.Exception);
            else
            {
                Debug.Log("DataWrite: Complete");
                LoadUserFromFirebase("users/" + auth.CurrentUser.UserId);
            }
        });
    }

    public void LoadUserFromFirebase(string path, OnLoadedDelegate onLoadedDelegate = null)
    {
        db.RootReference.Child("users");
        db.RootReference.Child(path).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
                Debug.LogWarning(task.Exception);

            DataSnapshot snap = task.Result;

            onLoadedDelegate(snap);
        });
    }

    public void CreateNewMatch(string data, string gameID, List<string> playersInQueue)
    {
        db.RootReference.Child("games").Child(gameID).SetRawJsonValueAsync(data).ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
                Debug.LogWarning(task.Exception);
            else
            {
                Debug.Log("game Created");
                currentGameID = gameID;
            }

            foreach (string player in playersInQueue)
            {
                RemoveFromQueue(player);
            }
        });
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

    public void LoadGameData(string path, OnLoadedDelegate onLoadedDelegate)
    {
        db.RootReference.Child(path).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
                Debug.LogWarning(task.Exception);

            DataSnapshot snap = task.Result;
            onLoadedDelegate(snap);
        });
    }

    public void SaveBallPath(float[] xPos, float[] yPos, int cellTaken)
    {
        PlayerMove playerMove = new PlayerMove();
        playerMove.xPos = xPos;
        playerMove.yPos = yPos;
        playerMove.cell = cellTaken;
        string json = JsonUtility.ToJson(playerMove);

        db.RootReference.Child("games").Child(currentGameID).Child("ballPath").SetRawJsonValueAsync(json).ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
            {
                Debug.Log(task.Exception);
            }
        });
    }

    public void ChangeTurn()
    {
        Debug.Log(GameManager.Instance.greenTurn);

        GameManager.Instance.greenTurn = !GameManager.Instance.greenTurn;

        Debug.Log(GameManager.Instance.greenTurn);

        db.RootReference.Child("games").Child(currentGameID).Child("greenTurn").SetValueAsync(GameManager.Instance.greenTurn).ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
                Debug.Log(task.Exception);
        });
    }
    public void AddPointToUser(string playerID)
    {
        db.RootReference.Child("users").Child(playerID).Child("wins").GetValueAsync().ContinueWith(task =>
        {
            if (task.Exception != null)
            {
                Debug.Log(task.Exception);
            }

            var user = JsonUtility.FromJson<UserData>(task.Result.GetRawJsonValue());

            int wins = user.Wins + 1;
            Debug.Log(task.Result);
            Debug.Log(wins);

            db.RootReference.Child("users").Child(playerID).Child("wins").SetValueAsync(wins).ContinueWithOnMainThread(task2 =>
            {
                if (task2.Exception != null)
                {
                    Debug.Log(task2.Exception);
                }
            });

        });
    }

    private void OnApplicationQuit()
    {
        Debug.Log("Quit");
        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            db.RootReference.Child("games").Child(currentGameID).RemoveValueAsync();
            Debug.Log("deletedGame");
        }
        else if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            RemoveFromQueue(GetAuth.CurrentUser.UserId);
            Debug.Log("removed from queue");
        }
    }
}

