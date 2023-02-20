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
    private FirebaseSignIn firebaseSignIn;

    [SerializeField] private TMP_InputField email;
    [SerializeField] private TMP_InputField password;
    [SerializeField] private TMP_InputField username;

    [SerializeField] private Button playButton;
    [SerializeField] private Button guestButton;

    [SerializeField] private GameObject singedIn;
    [SerializeField] private GameObject singedOut;

    [SerializeField] private Canvas findMatchCanvas;

    private void Start()
    {
        firebaseSignIn = GetComponent<FirebaseSignIn>();
        //sätt tillbaka när klar med testing
        //FirebaseManager.Instance.LoadUserFromFirebase("users/" + FirebaseAuth.DefaultInstance.CurrentUser.UserId, UserLoaded);

        username.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }

    public void UserLoaded(DataSnapshot snapshot)
    {
        var loadedUser = JsonUtility.FromJson<UserData>(snapshot.GetRawJsonValue());
        firebaseSignIn.SignInFirebase(loadedUser.Email, loadedUser.Password);
        SetUsername(snapshot);
    }

    public void SetUsername(DataSnapshot snapshot)
    {
        var loadedUser = JsonUtility.FromJson<UserData>(snapshot.GetRawJsonValue());
        if (loadedUser.Name != " ")
        {
            username.text = loadedUser.Name;
            FirebaseManager.Instance.thisUsername = username.text;
        }
    }

    private void ValueChangeCheck()
    {
        if (string.IsNullOrWhiteSpace(username.text) || username.text.Contains(" "))
        {
            playButton.interactable = false;
            playButton.GetComponent<Image>().color = new Color(0.6f, 0.6f, 0.6f, 0.66f);
        }
        else
        {
            playButton.interactable = true;
            playButton.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }
    }

    public void ShowFindMatch()
    {
        FirebaseManager.Instance.LoadUserFromFirebase("users/" + FirebaseAuth.DefaultInstance.CurrentUser.UserId, firebaseSignIn.SaveUsername);
        findMatchCanvas.gameObject.SetActive(true);
        findMatchCanvas.GetComponent<FindMatch>().playerID = FirebaseManager.Instance.GetAuth.CurrentUser.UserId;
        findMatchCanvas.GetComponent<FindMatch>().username = username.text;
        gameObject.SetActive(false);
    }

    public void OnSignInClick()
    {
        firebaseSignIn.SignInFirebase(email.text, password.text);
    }

    public void OnGuestPlayClick()
    {
        firebaseSignIn.GuestSignIn();
        gameObject.SetActive(false);
        findMatchCanvas.gameObject.SetActive(true);
    }

    public void OnTjotjo()
    {
        firebaseSignIn.SignInFirebase("tjo@tjo.tjo", "tjotjo");
    }
    public void OnHejhej()
    {
        firebaseSignIn.SignInFirebase("hej@hej.hej", "hejhej");
    }

    public void SignedIn()
    {
        singedOut.SetActive(false);
        singedIn.SetActive(true);
        guestButton.gameObject.SetActive(false);
    }

    public void LogoutClick()
    {
        FirebaseManager.Instance.GetAuth.SignOut();
        singedIn.SetActive(false);
        singedOut.SetActive(true);
        guestButton.gameObject.SetActive(true);
        findMatchCanvas.gameObject.SetActive(false);
        gameObject.SetActive(true);

        email.text = string.Empty;
        password.text = string.Empty;
        username.text = "";

    }
}
