using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    private Camera cam;

    [SerializeField] public TrajectoryLine trajectory;
    [SerializeField] public float pushForce = 4f;

    [SerializeField] private GameObject pluppPrefab;
    [SerializeField] private GameObject used;
    [SerializeField] private GameObject shootPos;
    [SerializeField] private GameObject pluppHolder;
    private Plupp plupp;
    private Animator animator;

    private Vector2 startPoint;
    private Vector2 endPoint;
    private Vector2 direction;
    private Vector2 force;
    private float distance;

    private bool isDragging;
    private bool isGhostPlupp;

    private float[] xPos, yPos;
    private int cellToTake;
    int i = 0;

    void Start()
    {
        cam = Camera.main;
        animator = GetComponent<Animator>();
        //GetPlupp();
    }
    public void GetPlupp()
    {
        if (pluppHolder.transform.childCount == 0)
            SpawnPluppar();

        plupp = pluppHolder.GetComponentInChildren<Plupp>();
        plupp.transform.position = shootPos.transform.position;
    }

    private void SpawnPluppar()
    {
        //gör en object pool
        for (int i = 0; i <= 5; i++)
        {
            Vector2 spawnPos = new Vector2(pluppHolder.transform.position.x + UnityEngine.Random.Range(-1f, 1f), pluppHolder.transform.position.y + UnityEngine.Random.Range(-1f, 1f));
            Instantiate(pluppPrefab, spawnPos, Quaternion.identity, pluppHolder.transform);
        }
    }

    void Update()
    {
        if (plupp != null && !isGhostPlupp)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isDragging = true;
                OnDragStart();
            }
            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
                OnDragEnd();
            }
            if (isDragging)
            {
                OnDrag();
            }
        }
        if (plupp != null && isGhostPlupp)
        {
            plupp.ActivateRb();
            Vector2 currentPos = new Vector2(xPos[i], yPos[i]);
            Vector2 nextPos = new Vector2(xPos[i + 1], yPos[i + 1]);

            plupp.PushGhost(Vector3.Lerp(currentPos, nextPos, 0.3f));
            i++;
        }

    }

    void OnDragStart()
    {
        plupp.DeactivateRb();
        startPoint = cam.ScreenToWorldPoint(Input.mousePosition);

        trajectory.ShowDots();
    }

    void OnDrag()
    {
        endPoint = cam.ScreenToWorldPoint(Input.mousePosition);
        distance = Vector2.Distance(startPoint, endPoint);
        direction = (startPoint - endPoint).normalized;
        force = direction * distance * pushForce;

        Debug.DrawLine(startPoint, endPoint);

        trajectory.UpdateDots(plupp.Pos, force);
    }

    void OnDragEnd()
    {
        animator.SetTrigger("Shoot");
        plupp.transform.parent = used.transform;
        plupp.ActivateRb();
        plupp.Push(force);

        trajectory.HideDots();

        plupp = null;
    }

    public void ShowGhostPlupp(float[] _xPos, float[] _yPos, int _cell)
    {
        xPos = _xPos;
        yPos = _yPos;
        cellToTake = _cell;

        isGhostPlupp = true;
        GetPlupp();

    }
}
