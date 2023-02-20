using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CameraMover : MonoBehaviour
{
    [HideInInspector]
    public bool isGreenTurn = true;
    private float camMoveSpeed = 4f;

    [SerializeField] private GameObject leftWall;
    [SerializeField] private GameObject rightWall;
    [SerializeField] private BoxCollider gridWalls;

    [SerializeField] private TextMeshProUGUI turnText;

    private float xMin, xMax;

    private void Start()
    {
        xMin = gridWalls.bounds.min.x;
        xMax = gridWalls.bounds.max.x;

    }

    void FixedUpdate()
    {
        if (isGreenTurn)
        {
            Vector3 newCamPos = new Vector3(xMin, 0, -10);
            transform.position = Vector3.Lerp(transform.position, newCamPos, camMoveSpeed * Time.deltaTime);

            turnText.text = "Green Turn";
            turnText.color = new Color(0.254902f, 0.8784314f, 0.2f);
        }
        else if (!isGreenTurn)
        {
            Vector3 newCamPos = new Vector3(xMax, 0, -10);
            transform.position = Vector3.Lerp(transform.position, newCamPos, camMoveSpeed * Time.deltaTime);

            turnText.text = "Purple Turn";
            turnText.color = new Color(0.7176471f, 0.1294118f, 0.7960784f);
        }
    }
}
