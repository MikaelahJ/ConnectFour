using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plupp : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public CircleCollider2D col;

    public Vector3 pos { get { return transform.position; } }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        if(transform.parent.name == "GreenHolder" || transform.parent.name == "PurpleHolder")
        {
            //transform.rotation = ;
        }
            transform.up = -rb.velocity;
    }

    public void Push(Vector3 force)
    {
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    public void ActivateRb()
    {
        rb.isKinematic = false;
    }
    public void DeactivateRb()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = 0f;
        rb.isKinematic = true;

    }
}
