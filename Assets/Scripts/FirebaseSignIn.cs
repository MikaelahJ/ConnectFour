using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using UnityEngine.UI;

class UserData
{
    public string Name;
    public string Email;
    public string Password;
    public int Wins;
}

public class FirebaseSignIn : MonoBehaviour
{
    FirebaseAuth auth;
    FirebaseDatabase db;

    [SerializeField] private TMP_InputField username;

    public delegate void OnLoadedDelegate(DataSnapshot snapshot);
    public FirebaseAuth GetAuth { get { return auth; } }

    private void Awake()
    {
        db = FirebaseDatabase.DefaultInstance;
        db.SetPersistenceEnabled(false);

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
                {
                    if (task.Exception != null)
                        Debug.LogError(task.Exception);

                    auth = FirebaseAuth.DefaultInstance;
                });
    }

    public void SignInFirebase(string email, string password)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
            {
                if (task.Exception.Message.Contains("The email address is already in use by another account."))
                {
                    auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task2 =>
                    {
                        if (task2.Exception != null)
                        {
                            Debug.LogWarning(task2.Exception);
                        }
                        else
                        {
                            Debug.Log("Email Exists, Logging in");

                            FirebaseUser newUser = task2.Result;
                            Debug.LogFormat("User signed in successfully: {0} ({1})",
                              newUser.DisplayName, newUser.UserId);
                            //status.text = newUser.Email + "is signed in";

                            //playButton.interactable = true;
                            PlayerDataManager.Instance.SavePlayerInlog(email, password);
                            GetComponent<Mainmenu>().SignedIn();
                            LoadFromFirebase("users/" + auth.CurrentUser.UserId);

                            LoadFromFirebase("users/" + FirebaseAuth.DefaultInstance.CurrentUser.UserId,GetComponent<Mainmenu>().SetUsername);
                        }
                    });
                }
                else
                {
                    Debug.LogWarning(task.Exception);
                }
            }
            else
            {
                FirebaseUser newUser = task.Result;
                Debug.LogFormat("User Registered: {0} ({1})",
                      newUser.DisplayName, newUser.UserId);

                UserData userData = new UserData();
                userData.Email = email;
                userData.Password = password;
                userData.Wins = 0;

                string json = JsonUtility.ToJson(userData);
                SaveToFirebase(json);
            }
        });
    }

    public void GuestSignIn()
    {
        auth.SignInAnonymouslyAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
            {
                Debug.LogWarning(task.Exception);
            }
            else
            {
                FirebaseUser newUser = task.Result;
                Debug.LogFormat("User signed in successfully: {0} ({1})",
                    newUser.DisplayName, newUser.UserId);
            }
        });
    }

    public void SaveUsername(DataSnapshot snapshot)
    {
        Debug.Log("hejhej");
        var loadedUser = JsonUtility.FromJson<UserData>(snapshot.GetRawJsonValue());

        UserData userData = new UserData();
        userData.Name = username.text;
        userData.Email = loadedUser.Email;
        userData.Password = loadedUser.Password;
        userData.Wins = loadedUser.Wins;

        string json = JsonUtility.ToJson(userData);
        SaveToFirebase(json);

    }
    public void test(DataSnapshot snapshot)
    {
        var loadedUser = JsonUtility.FromJson<UserData>(snapshot.GetRawJsonValue());
        Debug.Log(loadedUser.Name);
    }

    private void SaveToFirebase(string data)
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
}
