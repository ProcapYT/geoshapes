using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{
    private GameObject player;
    public float enemySpeed = 4f;
    public float enemyHealth = 2;
    public GameObject enemyExplosion;
    public float spawnTime = 2f;
    private float spawnTimer = 0;
    public float damage = 10f;
    public ParticleSystem spawnParticles;
    public SpriteRenderer sprite;
    public CircleCollider2D circleCollider;
    private LogicScript logic;
    private EnemyTemplates enemyTemplates;
    private float damageTime = 1f;
    private float damageTimer = 0;
    public float score;
    public float itemDropProbab = 10f;
    private GameObject[] enemyItems;
    public bool isBoss = false;
    public Slider bossHpBar;
    public int itemN;

    private GameObject[] items;
    private Rigidbody2D rb;
    private Slider hpBar;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false;

        player = GameObject.Find("Player");
        logic = GameObject.Find("Logic Object").GetComponent<LogicScript>();
        enemyItems = GameObject.Find("Item Templates").GetComponent<ItemTemplates>().normalItems;
        items = GameObject.Find("Item Templates").GetComponent<ItemTemplates>().shopItems;

        enemyTemplates = GameObject.Find("Enemy Templates").GetComponent<EnemyTemplates>();

        enemyTemplates.spawnedEnemies.Add(gameObject);

        rb = GetComponent<Rigidbody2D>();

        if (isBoss)
        {
            enemyHealth += Mathf.Floor(logic.elipsedTime / 60f) * 10f;
        } else
        {
            enemyHealth += Mathf.Floor(logic.elipsedTime / 60f);
        }

        if (isBoss == false)
        {
            hpBar = transform.Find("Canvas").Find("Health bar").GetComponent<Slider>();
            hpBar.maxValue = enemyHealth;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isBoss == false)
        {
            hpBar.value = enemyHealth;
        }

        if (spawnTimer < spawnTime)
        {
            spawnTimer += Time.deltaTime;
        } else
        {
            spawnParticles.Clear();
            sprite.enabled = true;
            circleCollider.enabled = true;

            if (player != null)
            {
                Vector3 direction = player.transform.position - transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                Quaternion rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);

                transform.rotation = rotation;

                Vector3 movement = transform.up;

                if (enemySpeed != 0)
                {
                    rb.velocity = movement * enemySpeed;
                }
            }
        }

        if (damageTimer < damageTime)
        {
            damageTimer += Time.deltaTime;
        }

        if (enemyHealth <= 0)
        {
            Instantiate(enemyExplosion, transform.position, Quaternion.identity);
            logic.AddScore(score);

            if (isBoss)
            {
                bossHpBar.gameObject.SetActive(false);

                for (int i = 0; i < itemN; i++) {
                    float t = (i + 1) / (float)itemN; 
                    float posX = transform.position.x - 1.25f + t * 2.5f;
                    Vector2 spawnPosition = new Vector2(posX, transform.position.y);

                    GameObject spawnedItem = Instantiate(items[Random.Range(0, items.Length)], spawnPosition, Quaternion.identity);
                    spawnedItem.GetComponent<ShopItem>().cost = 0;
                }
                
            } else
            {
                float dropProbab = Random.Range(0, 1f);

                if (dropProbab <= itemDropProbab / 100)
                {
                    int itemDropI = Random.Range(0, enemyItems.Length);

                    Instantiate(enemyItems[itemDropI], transform.position, Quaternion.identity);
                }
            }

            player.GetComponent<PlayerScript>().enemiesKilled++;
            Destroy(gameObject);
        }

        if (isBoss == false)
        {
            hpBar.transform.rotation = Quaternion.identity;
            hpBar.transform.position = new Vector2(transform.position.x, transform.position.y + 1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            enemyHealth -= player.GetComponent<PlayerScript>().playerDamage;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            logic.RemovePlayerHP(damage);
            damageTimer = 0;
        }

        if (collision.gameObject.tag == "Missile")
        {
            enemyHealth -= player.GetComponent<PlayerScript>().playerDamage * 25;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (damageTimer >= damageTime)
            {
                logic.RemovePlayerHP(damage);
                damageTimer = 0;
            }
        }
    }
}
