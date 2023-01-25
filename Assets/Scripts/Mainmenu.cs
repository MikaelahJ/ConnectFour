using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Mainmenu : MonoBehaviour
{
    [SerializeField] private TMP_InputField email;
    [SerializeField] private TMP_InputField password;
    [SerializeField] private TMP_InputField username;


    private void Start()
    {
        if (PlayerDataManager.Instance.GetEmail() != "No email found")
            email.text = PlayerDataManager.Instance.GetEmail();
    }
    public void LoadGame()
    {

    }
    public void OnSignInClick()
    {
        FirebaseManager.Instance.SignInFirebase(email.text, password.text);
    }

    public void OnGuestPlayClick()
    {
        FirebaseManager.Instance.GuestUser();
    }
}
