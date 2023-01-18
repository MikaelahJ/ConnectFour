using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [HideInInspector]
    public bool isGreenTurn = true;
    private float camMoveSpeed = 4f;

    void Update()
    {
        if (isGreenTurn)
        {
            Vector3 newCamPos = new Vector3(0, 0, -10);
            transform.position = Vector3.Lerp(transform.position, newCamPos, camMoveSpeed * Time.deltaTime);
        }
        else if (!isGreenTurn)
        {
            Vector3 newCamPos = new Vector3(10, 0, -10);
            transform.position = Vector3.Lerp(transform.position, newCamPos, camMoveSpeed * Time.deltaTime);
        }
    }
}
