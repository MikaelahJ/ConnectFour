using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;

public class FirebaseManager : MonoBehaviour
{


    public FirebaseAuth auth;
    public FirebaseDatabase db;

    public delegate void OnLoadedDelegate(DataSnapshot snapshot);
    public FirebaseAuth GetAuth { get { return auth; } }


    public static FirebaseManager Instance = null;

    private void Awake()
    {
        #region singleton
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        #endregion

        db = FirebaseDatabase.DefaultInstance;
        db.SetPersistenceEnabled(false);

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
                Debug.LogError(task.Exception);

            auth = FirebaseAuth.DefaultInstance;
        });
    }

    public void SaveToFirebase(string data)
    {
        //puts the json data in the "users/userId" part of the database.
        Debug.Log("Trying to write data...");
        db.RootReference.Child("users").Child(auth.CurrentUser.UserId).SetRawJsonValueAsync(data).ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
                Debug.LogWarning(task.Exception);
            else
            {
                Debug.Log("DataWrite: Complete");
                LoadFromFirebase("users/" + auth.CurrentUser.UserId);
            }
        });
    }

    public void LoadFromFirebase(string path, OnLoadedDelegate onLoadedDelegate = null)
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

    public void LoadGames(string path, OnLoadedDelegate onLoadedDelegate)
    {

    }
}
