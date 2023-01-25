using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;

public class FirebaseManager : MonoBehaviour
{
    #region singleton
    public static FirebaseManager Instance = null;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }
    #endregion

    FirebaseAuth auth;

    void Start()
    {
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

                //playButton.interactable = true;

            }
        });
    }

private void RegisterNewUser(string email, string password)
{

}

public void GuestUser()
{
    FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
    {
        if (task.Exception != null)
            Debug.LogError(task.Exception);

        auth = FirebaseAuth.DefaultInstance;


        AnonymousSignIn();
    });
}
private void AnonymousSignIn()
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
}
