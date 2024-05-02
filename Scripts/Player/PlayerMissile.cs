using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMissile : MonoBehaviour
{
    public float missileStartSpeed = 0;
    public float missileMaxSpeed = 10f;
    public GameObject missileExplosion;

    public float rotateSpeed = 200f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.color = GameObject.Find("Player").GetComponent<SpriteRenderer>().color;
    }

    private void Update()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject enemy = enemies.Length == 0 ? null : enemies[Random.Range(0, enemies.Length)];

        missileStartSpeed += Time.deltaTime * 10;

        if (missileStartSpeed > missileMaxSpeed)
        {
            missileStartSpeed = missileMaxSpeed;
        }

        if (enemy != null)
        {
            Vector2 direction = (Vector2)enemy.transform.position - rb.position;

            direction.Normalize();

            float rotateAmount = Vector3.Cross(direction, transform.up).z;

            rb.angularVelocity = -rotateAmount * rotateSpeed;

            rb.velocity = transform.up * (missileStartSpeed * 100) * Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Wall")
        {
            Instantiate(missileExplosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
