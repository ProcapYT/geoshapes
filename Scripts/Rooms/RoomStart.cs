using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class RoomStart : MonoBehaviour
{
    private GameObject[] enemy;
    public GameObject[] doors;
    private float enemySpawnTime = 0.01f;
    private float enemySpawnTimer = 2f;

    public SpriteRenderer[] wallsSprites;
    public Color clearedColor = new Color(214f / 255f, 214f / 255f, 214f / 255f, 1f);
    public Color notClearedColor = new Color(116f / 255f, 116f / 255f, 116f / 255f, 1f);

    private Color bossRoomColor = new Color(195f / 255f, 69f / 255f, 69f / 255f, 1f);
    private Color itemRoomColor = new Color(255f / 255f, 164f / 255f, 0, 1f);
    private Color shopColor = new Color(171f / 255f, 122f / 255f, 78f / 255f, 1f);
    private Color sacrificeColor = new Color(30f / 255f, 30f / 255f, 30f / 255f, 1f);
    private Color miniBossNotClear = new Color(84f / 255f, 84f / 255f, 255f / 255f, 1f);
    private Color miniBossClear = new Color(0f / 255f, 30f / 255f, 255f / 255f, 1f);

    private int totalEnemies = 8;
    private int enemiesSpawned = 0;
    private bool playerEntered = false;

    public bool isBossRoom = false;
    private bool bossSpawned = false;

    public bool isItemRoom = false;
    public bool isSacrificeRoom = false;

    public bool isShop = false;
    public int shopItemsQuant = 5;

    public bool isMiniBoss;
    public bool isCleared;

    private GameObject boss;

    private GameObject[] bosses;
    private GameObject[] miniBosses;

    public GameObject trapdoor;
    public GameObject trampoline;

    private GameObject[] items;

    private List<GameObject> enemies = new List<GameObject>();

    private PlayerScript player;
    private LogicScript logic;

    private bool miniBossSpawned;
    private GameObject instantiatedMiniBoss;

    public GameObject sacrificeSpikes;

    private void Start()
    {
        enemy = GameObject.Find("Enemy Templates").GetComponent<EnemyTemplates>().enemies;
        bosses = GameObject.Find("Enemy Templates").GetComponent<EnemyTemplates>().bosses;
        items = GameObject.Find("Item Templates").GetComponent<ItemTemplates>().shopItems;
        miniBosses = GameObject.Find("Enemy Templates").GetComponent<EnemyTemplates>().miniBosses;

        player = GameObject.Find("Player").GetComponent<PlayerScript>();
        logic = GameObject.Find("Logic Object").GetComponent<LogicScript>();

        totalEnemies = Random.Range(7, 10);

        for (int i = 0; i < wallsSprites.Length; i++)
        {
            wallsSprites[i].color = notClearedColor;
        }
    }

    private void Update()
    {
        if (!isBossRoom && !isItemRoom && !isShop && !isSacrificeRoom && !isMiniBoss && !isCleared)
        {
            if (playerEntered)
            {
                if (enemySpawnTimer < enemySpawnTime)
                {
                    enemySpawnTimer += Time.deltaTime;
                }
                else
                {
                    if (enemiesSpawned < totalEnemies)
                    {
                        float randomYPosition = Random.Range(transform.position.y + 7, transform.position.y - 7);
                        float randomXPosition = Random.Range(transform.position.x + 7, transform.position.x - 7);
                        Vector2 spawnPosition = new Vector2(randomXPosition, randomYPosition);

                        int spawnEnemyI = Random.Range(0, enemy.Length);

                        GameObject spawnedEnemy = Instantiate(enemy[spawnEnemyI], spawnPosition, Quaternion.identity);

                        enemies.Add(spawnedEnemy);
                        enemiesSpawned++;
                    }

                    enemySpawnTimer = 0;
                }
            }

            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i] == null || ReferenceEquals(enemies[i], null))
                {
                    enemies.RemoveAt(i);
                }
            }

            if (enemiesSpawned >= totalEnemies && enemies.Count <= 0)
            {
                for (int i = 0; i < doors.Length; i++)
                {
                    doors[i].SetActive(false);
                }

                for (int i = 0; i < wallsSprites.Length; i++)
                {
                    wallsSprites[i].color = clearedColor;
                }

                if (player.pickedUpHolyMantle)
                {
                    logic.hasHolyMantle = true;
                }

                if (player.currentActive != null)
                {
                    player.activeCurrentCharges++;
                }

                Destroy(gameObject);
            }
        }
        else if (isBossRoom)
        {
            for (int i = 0; i < wallsSprites.Length; i++)
            {
                wallsSprites[i].color = bossRoomColor;
            }

            if (playerEntered)
            {
                if (bossSpawned == false)
                {
                    int randomBossI = Random.Range(0, bosses.Length);

                    boss = Instantiate(bosses[randomBossI], new Vector2(transform.position.x, transform.position.y - 2f), Quaternion.identity);

                    for (int i = 0; i < doors.Length; i++)
                    {
                        doors[i].SetActive(true);
                    }

                    bossSpawned = true;
                }
                else
                {
                    if (boss == null || ReferenceEquals(boss, null))
                    {
                        for (int i = 0; i < doors.Length; i++)
                        {
                            doors[i].SetActive(false);
                        }

                        if (logic.elipsedTime <= 1800 && logic.currentFloor == 10)
                        {
                            Vector2 trapdoorPosition = new Vector2(transform.position.x - 2f, transform.position.y);
                            Vector2 trampolinePosition = new Vector2(transform.position.x + 2f, transform.position.y);

                            Instantiate(trampoline, trampolinePosition, Quaternion.identity);
                            Instantiate(trapdoor, trapdoorPosition, Quaternion.identity);
                        }
                        else if (player.goingUp)
                        {
                            Instantiate(trampoline, transform.position, Quaternion.identity);
                        }
                        else
                        {
                            Instantiate(trapdoor, transform.position, Quaternion.identity);
                        }

                        if (player.pickedUpHolyMantle)
                        {
                            logic.hasHolyMantle = true;
                        }

                        if (player.currentActive != null)
                        {
                            player.activeCurrentCharges++;
                        }

                        Destroy(gameObject);
                    }
                }
            }
        }
        else if (isItemRoom)
        {
            for (int i = 0; i < wallsSprites.Length; i++)
            {
                wallsSprites[i].color = itemRoomColor;
            }

            GameObject spawnedItem = Instantiate(items[(int)Random.Range(0, items.Length)], transform.position, Quaternion.identity);
            spawnedItem.GetComponent<ShopItem>().cost = 0;

            Destroy(gameObject);
        }
        else if (isShop)
        {
            for (int i = 0; i < wallsSprites.Length; i++)
            {
                wallsSprites[i].color = shopColor;
            }

            GameObject[] itemsToSpawn = SelectItems(items, shopItemsQuant);

            for (int i = 0; i < itemsToSpawn.Length; i++)
            {
                float t = i / (float)shopItemsQuant;
                float posX = transform.position.x - 5f + t * 10f;

                Instantiate(itemsToSpawn[i], new Vector2(posX, transform.position.y), Quaternion.identity);
            }

            Destroy(gameObject);
        }
        else if (isSacrificeRoom)
        {
            for (int i = 0; i < wallsSprites.Length; i++)
            {
                wallsSprites[i].color = sacrificeColor;
            }

            Instantiate(sacrificeSpikes, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
        else if (isMiniBoss)
        {
            for (int i = 0; i < wallsSprites.Length; i++)
            {
                wallsSprites[i].color = miniBossNotClear;
            }

            if (playerEntered)
            {
                if (!miniBossSpawned)
                {
                    instantiatedMiniBoss = Instantiate(miniBosses[Random.Range(0, miniBosses.Length)], transform.position, Quaternion.identity);
                    miniBossSpawned = true;
                }

                if (instantiatedMiniBoss == null || ReferenceEquals(instantiatedMiniBoss, null))
                {
                    for (int i = 0; i < wallsSprites.Length; i++)
                    {
                        wallsSprites[i].color = miniBossClear;
                    }

                    for (int i = 0; i < doors.Length; i++)
                    {
                        doors[i].SetActive(false);
                    }

                    if (player.pickedUpHolyMantle)
                    {
                        logic.hasHolyMantle = true;
                    }

                    if (player.currentActive != null)
                    {
                        player.activeCurrentCharges++;
                    }

                    Destroy(gameObject);
                }
            }
        }
        else if (isCleared)
        {
            for (int i = 0; i < wallsSprites.Length; i++)
            {
                wallsSprites[i].color = clearedColor;
            }

            Destroy(gameObject);
        }
    }

    GameObject[] SelectItems(GameObject[] options, int amount)
    {
        if (amount > options.Length)
        {
            return null;
        }

        List<GameObject> selectedItems = new List<GameObject>(amount);
        List<int> availableIndices = new List<int>(options.Length);

        for (int i = 0; i < options.Length; i++)
        {
            availableIndices.Add(i);
        }

        for (int i = 0; i < amount; i++)
        {
            int selectedIndice = Random.Range(0, availableIndices.Count);
            int realIndice = availableIndices[selectedIndice];

            selectedItems.Add(options[realIndice]);
            availableIndices.RemoveAt(selectedIndice);
        }

        return selectedItems.ToArray();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            for (int i = 0; i < doors.Length; i++)
            {
                doors[i].SetActive(true);
            }

            playerEntered = true;
        }
    }
}
