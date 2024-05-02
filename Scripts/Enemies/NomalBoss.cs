using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NomalBoss : MonoBehaviour
{
    public float dashSpeed = 20f;
    public float dashTime = 2f;

    private GameObject target;

    private Rigidbody2D rb;
    private float dashTimer = -2f;
    private Vector2 dashDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.Find("Player");

        dashDirection = (Vector2)target.transform.position - rb.position;
    }

    void Update()
    {
        if (dashTimer < dashTime)
        {
            dashTimer += Time.deltaTime;
        } else 
        {
            if (target != null)
            {
                rb.velocity = transform.up * dashSpeed;

                transform.LookAt(dashDirection);

                dashDirection = (Vector2)target.transform.position - rb.position;
            }

            dashTimer = 0;
        }
    }
}
