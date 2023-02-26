using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plupp : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public CircleCollider2D col;
    [HideInInspector] public Animator animator;

    public List<float> xPositions = new List<float>();
    public List<float> yPositions = new List<float>();
    float timer = 0;


    public bool IsInCell;

    public Vector3 Pos { get { return transform.position; } }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();
        animator = GetComponent<Animator>();

        animator.SetBool("IsStill", true);
        rb.freezeRotation = true;
    }

    private void Update()
    {
        if (!transform.parent.gameObject.CompareTag("Holder") && !IsInCell)
        {
            transform.up = -rb.velocity;

            timer += Time.deltaTime;
            if (timer >= 0.2f)
            {
                xPositions.Add(transform.position.x);
                yPositions.Add(transform.position.y);
                timer = 0;
            }
        }
    }

    public void Push(Vector3 force)
    {
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    public void ActivateRb()
    {
        rb.freezeRotation = false;
        transform.up = -rb.velocity;
        animator.SetBool("IsStill", false);
        rb.isKinematic = false;
    }

    public void DeactivateRb()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = 0f;
        rb.isKinematic = true;

        animator.SetBool("IsStill", true);
    }

    public void PushGhost(Vector3 lerpForce)
    {
        rb.AddForce(lerpForce, ForceMode2D.Impulse);
    }
}
