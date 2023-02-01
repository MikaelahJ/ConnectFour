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

public class GameData
{
    public string gameID;
    public List<PlayerGameInfo> players;
    public int maxPlayers = 2;
    public string displayName;
}

public class PlayerGameInfo
{
}

public class FindMatch : MonoBehaviour
{
    [SerializeField] private GameObject matchPrefab;
    [SerializeField] private GameObject matchHolder;

    FirebaseDatabase db;
    private DatabaseReference matchmakingRef;

    private string username;

    private void Start()
    {
        FirebaseManager.Instance.LoadFromFirebase("users/" + FirebaseAuth.DefaultInstance.CurrentUser.UserId, UserLoaded);

        db = FirebaseManager.Instance.db;

     
        Debug.Log(matchmakingRef);
    }

    private void UserLoaded(DataSnapshot snapshot)
    {
        var loadedUser = JsonUtility.FromJson<UserData>(snapshot.GetRawJsonValue());
        username = loadedUser.Name;
    }

    public void JoinQueue(string playerID)
    {
        Debug.Log(playerID);
        matchmakingRef.Child(playerID).SetValueAsync("placeholder")
            .ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.Log(task.Exception);
                }
                else
                {
                    Debug.Log("Successfully joined the queue");
                }
            });

    }
}
