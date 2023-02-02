using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [HideInInspector]
    public bool isGreenTurn = true;
    private float camMoveSpeed = 4f;

    [SerializeField] private GameObject leftWall;
    [SerializeField] private GameObject rightWall;
    [SerializeField] private BoxCollider gridWalls;

    private Camera cam;
    private float camSize;
    private float camRatio;
    private float camX;
    private float xMin, xMax;

    private void Start()
    {
        cam = GetComponent<Camera>();
        camSize = cam.orthographicSize;
        camRatio = (xMax + camSize) / 2.0f;

        xMin = gridWalls.bounds.min.x;
        xMax = gridWalls.bounds.max.x;
    }
    void FixedUpdate()
    {

        if (isGreenTurn)
        {
            Vector3 newCamPos = new Vector3(xMin, 0, -10);
            transform.position = Vector3.Lerp(transform.position, newCamPos, camMoveSpeed * Time.deltaTime);
        }
        else if (!isGreenTurn)
        {
            Vector3 newCamPos = new Vector3(xMax, 0, -10);
            transform.position = Vector3.Lerp(transform.position, newCamPos, camMoveSpeed * Time.deltaTime);
        }
    }
}
