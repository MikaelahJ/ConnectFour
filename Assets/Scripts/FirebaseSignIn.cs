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

    private void Start()
    {

    }

    public void SignInFirebase(string email, string password)
    {
        auth = FirebaseManager.Instance.GetAuth;
        db = FirebaseManager.Instance.db;
    
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
                            FirebaseManager.Instance.LoadFromFirebase("users/" + auth.CurrentUser.UserId);

                            FirebaseManager.Instance.LoadFromFirebase("users/" + FirebaseAuth.DefaultInstance.CurrentUser.UserId, GetComponent<Mainmenu>().SetUsername);
                        }
                    });
                }
                else
                {
                    Debug.LogWarning(task.Exception);
                }
            }
            else
            {   //create new user
                FirebaseUser newUser = task.Result;
                Debug.LogFormat("User Registered: {0} ({1})", newUser.DisplayName, newUser.UserId);

                UserData userData = new UserData();
                userData.Email = email;
                userData.Password = password;
                userData.Wins = 0;

                string json = JsonUtility.ToJson(userData);
                FirebaseManager.Instance.SaveToFirebase(json);
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
        var loadedUser = JsonUtility.FromJson<UserData>(snapshot.GetRawJsonValue());

        UserData userData = new UserData();
        userData.Name = username.text;
        userData.Email = loadedUser.Email;
        userData.Password = loadedUser.Password;
        userData.Wins = loadedUser.Wins;

        string json = JsonUtility.ToJson(userData);
        FirebaseManager.Instance.SaveToFirebase(json);

    }
}
