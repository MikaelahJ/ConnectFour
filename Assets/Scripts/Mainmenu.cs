using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System;

public class Mainmenu : MonoBehaviour
{
    [SerializeField] private TMP_InputField email;
    [SerializeField] private TMP_InputField password;
    [SerializeField] private TMP_InputField username;

    [SerializeField] private GameObject singedIn;
    [SerializeField] private GameObject singedOut;


    private void Start()
    {
        GetComponent<FirebaseSignIn>().LoadFromFirebase("users/" + FirebaseAuth.DefaultInstance.CurrentUser.UserId, UserLoaded);

        username.onValueChanged.AddListener(delegate { ValueChangeCheck(); });

    }



    public void UserLoaded(DataSnapshot snapshot)
    {
        Debug.Log("hej");
        var loadedUser = JsonUtility.FromJson<UserData>(snapshot.GetRawJsonValue());
        Debug.Log(loadedUser);
        GetComponent<FirebaseSignIn>().SignInFirebase(loadedUser.Email, loadedUser.Password);
        username.text = loadedUser.Name;
    }

    private void ValueChangeCheck()
    {
        throw new NotImplementedException();
    }

    public void LoadGame()
    {

    }

    public void OnSignInClick()
    {
        FirebaseSignIn.Instance.SignInFirebase(email.text, password.text);
    }

    public void SignedIn()
    {
        singedOut.SetActive(false);
        singedIn.SetActive(true);
    }

    public void SignedOut()
    {
        singedIn.SetActive(false);
        singedOut.SetActive(true);
    }

    public void OnGuestPlayClick()
    {
        FirebaseSignIn.Instance.GuestSignIn();
    }
}
