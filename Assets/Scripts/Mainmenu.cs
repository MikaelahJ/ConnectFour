using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Mainmenu : MonoBehaviour
{
    [SerializeField] private TMP_InputField username;
    [SerializeField] private TMP_InputField password;

    public void LoadGame()
    {

    }
    public void OnSignInClick()
    {
        FirebaseManager.Instance.OnSignIn(username.text, password.text);
    }

    public void OnGuestPlayClick()
    {
        FirebaseManager.Instance.GuestUser();
    }
}
