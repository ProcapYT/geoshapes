using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileScript : MonoBehaviour
{
    public float missileStartSpeed = 0;
    public float missileMaxSpeed = 10f;
    public int missileHealth = 1;
    public int missileDamage = 10;
    public GameObject missileExplosion;

    public float rotateSpeed = 200f;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        GameObject player = GameObject.Find("Player");

        missileStartSpeed += Time.deltaTime * 10;

        if (missileStartSpeed > missileMaxSpeed)
        {
            missileStartSpeed = missileMaxSpeed;
        }

        if (player != null)
        {
            Vector2 direction = (Vector2)player.transform.position - rb.position;

            direction.Normalize();

            float rotateAmount = Vector3.Cross(direction, transform.up).z;

            rb.angularVelocity = -rotateAmount * rotateSpeed;

            rb.velocity = transform.up * (missileStartSpeed * 50) * Time.deltaTime;
        }

        if (missileHealth <= 0)
        {
            Instantiate(missileExplosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Instantiate(missileExplosion, transform.position, Quaternion.identity);
            GameObject.Find("Logic Object").GetComponent<LogicScript>().RemovePlayerHP(10);
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Wall")
        {
            Instantiate(missileExplosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
