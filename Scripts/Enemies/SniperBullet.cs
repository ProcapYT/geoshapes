using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperBullet : MonoBehaviour
{
    public float bulletSpeed = 20f;
    public GameObject bulletExplosion;
    private LogicScript logic;
    public int damageToPlayer = 10;

    private void Start()
    {
        logic = GameObject.Find("Logic Object").GetComponent<LogicScript>();
    }

    private void Update()
    {
        transform.position += transform.up * bulletSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            logic.RemovePlayerHP(damageToPlayer);
        }

        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Wall") 
        {
            Instantiate(bulletExplosion, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}
