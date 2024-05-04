using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public float playerForce = 5f;
    public Rigidbody2D rb;
    private GameObject mainCamera;

    public float shotSpeed = 0.5f;
    private float shotTimer = 0;
    public float playerDamage = 1f;

    public GameObject bullet;
    public GameObject playerExplosion;
    public GameObject redExplosion;

    private LogicScript logic;
    private ItemTemplates itemTemplates;
    private EnemyTemplates enemyTemplates;

    public RoomTemplates roomTemplates;
    public GameObject entryRoom;
    public GameObject currentEntryRoom;

    public Transform minimapCamera;

    public GameObject trapdoor;

    public bool spectralBullets;

    public bool pickedUpHolyMantle = false;

    public int coins = 0;

    public bool goingUp = false;

    public GameObject activeItemObject;
    private Image activeItemImage;

    public string currentActive;
    public int activeCharges;
    public int activeCurrentCharges;

    public Animator itemPropertiesAnim;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDescription;

    private bool spawnActiveLeft = true;

    private delegate void FunctionDelegate();

    private int totalItemsPickedUp;
    public int enemiesKilled;

    public GameObject playerMissile;

    public float bulletSize = 1;
    public float bulletSpeed = 20;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.Find("Main Camera");
        rb = GetComponent<Rigidbody2D>();
        logic = GameObject.Find("Logic Object").GetComponent<LogicScript>();
        itemTemplates = GameObject.Find("Item Templates").GetComponent<ItemTemplates>();
        enemyTemplates = GameObject.Find("Enemy Templates").GetComponent<EnemyTemplates>();

        activeItemImage = activeItemObject.GetComponent<Image>();

        totalItemsPickedUp = PlayerPrefs.GetInt("Items picked up", 0);
        enemiesKilled = PlayerPrefs.GetInt("Enemies killed", 0);
    }

    void FixedUpdate()
    {
        Vector3 screenMousePosition = Input.mousePosition;

        screenMousePosition.z = transform.position.z - Camera.main.transform.position.z;

        Vector3 worldMausePosition = Camera.main.ScreenToWorldPoint(screenMousePosition);

        float angle = Mathf.Atan2(worldMausePosition.y - transform.position.y, worldMausePosition.x - transform.position.x);

        transform.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg - 90f);

        if (shotTimer < shotSpeed)
        {
            shotTimer += Time.fixedDeltaTime;
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                Quaternion bulletRotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg - 90f);

                GameObject instantiatedBullet = Instantiate(bullet, transform.position, bulletRotation);
                instantiatedBullet.transform.localScale *= bulletSize;
                instantiatedBullet.GetComponent<BulletMovement>().bulletSpeed = bulletSpeed;
                instantiatedBullet.GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;

                shotTimer = 0;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        PlayerPrefs.SetInt("Items picked up", totalItemsPickedUp);
        PlayerPrefs.SetInt("Enemies killed", enemiesKilled);

        minimapCamera.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 100f);

        if (currentActive != null)
        {
            Slider activeSlider = activeItemObject.transform.Find("Slider").gameObject.GetComponent<Slider>();
            activeSlider.maxValue = activeCharges;
            activeSlider.value = activeCurrentCharges;
        }

        // movement controlls
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontalInput, verticalInput, 0);
        rb.velocity = movement * playerForce;

        mainCamera.transform.position = new Vector3(transform.position.x, transform.position.y, -10);



        // destroy the player if the hp is <= 0
        if (logic.playerHP <= 0)
        {
            if (PlayerPrefs.GetString("Devil player", "False") == "True")
            {
                Instantiate(redExplosion, transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(playerExplosion, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }

        // get when the player hits the space bar to use the active item
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // if the player has an active use it
            if (currentActive != null && activeCurrentCharges >= activeCharges)
            {
                // actives logic
                switch (currentActive)
                {
                    case "Hp active":
                        logic.AddPlayerHP(10);
                        break;

                    case "Coin active":
                        coins += 10;
                        break;

                    case "Shovel active":
                        Instantiate(trapdoor, transform.position, Quaternion.identity);
                        break;

                    case "R key active":
                        logic.recivedDamage = true;
                        ResetFloor();
                        logic.currentFloor = 1;
                        logic.elipsedTime = 0;
                        currentActive = null;
                        activeItemObject.SetActive(false);
                        goingUp = false;
                        break;

                    case "Potassium active":
                        logic.AddPlayerHP(logic.maxPlayerHp);
                        break;

                    case "Strontium active":
                        logic.hasHolyMantle = true;
                        break;

                    case "Lawrencium active":
                        shotSpeed *= 4;
                        playerDamage += 5;
                        break;

                    case "Semicolon active":
                        int itemBehaviour = Random.Range(0, 2);

                        switch (itemBehaviour)
                        {
                            case 0:
                                bool add = Random.value > 0.5;
                                playerDamage += add ? 0.5f : -0.5f;

                                break;

                            case 1:
                                bool divide = Random.value > 0.5;
                                shotSpeed /= divide ? 2f : 0.5f;

                                break;

                            default:
                                Debug.LogError($"Error, itemBehaviour ({itemBehaviour}) is not programed.");

                                break;
                        }

                        activeCharges = Random.Range(3, 6);

                        break;

                    case "Missile active":
                        Instantiate(playerMissile, transform.position, Quaternion.identity);

                        break;
                }

                activeCurrentCharges = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // floor logic
        if (collision.gameObject.tag == "Trapdoor")
        {
            Destroy(collision.gameObject);

            ResetFloor();

            logic.currentFloor += 1;
        }

        if (collision.gameObject.tag == "Trampoline")
        {
            Destroy(collision.gameObject);

            ResetFloor();

            logic.currentFloor -= 1;

            goingUp = true;
        }

        // items ----------------------------------------------------------------

        switch (collision.gameObject.tag)
        {
            case "Heart item":
                logic.AddPlayerHP(5);
                break;

            case "Double heart item":
                logic.AddPlayerHP(10);
                break;

            case "Coin item":
                coins += 5;
                break;

            case "Batery item":
                if (currentActive != null)
                {
                    activeCurrentCharges++;
                }
                break;

            case "Atack item":
                PickUpItem(() =>
                {
                    shotSpeed /= 2f;
                    if (shotSpeed < 0.01f) shotSpeed = 0.01f;

                    bulletSize /= 1.3f;
                    if (bulletSize < 0.1f) bulletSize = 0.1f;
                }, collision.gameObject);
                break;

            case "Shoe item":
                PickUpItem(() =>
                {
                    playerForce += 1f;
                }, collision.gameObject);
                break;

            case "Damage item":
                PickUpItem(() =>
                {
                    playerDamage += 0.5f;
                    bulletSize += 0.25f;
                }, collision.gameObject);
                break;

            case "Hp item":
                PickUpItem(() =>
                {
                    logic.maxPlayerHp += 10;
                    logic.AddPlayerHP(10);
                }, collision.gameObject);
                break;

            case "Spectral item":
                PickUpItem(() =>
                {
                    spectralBullets = true;
                    playerDamage += 0.5f;
                    bulletSize += 0.25f;
                }, collision.gameObject);
                break;

            case "Holy mantle":
                PickUpItem(() =>
                {
                    pickedUpHolyMantle = true;
                    logic.hasHolyMantle = true;
                    logic.AddPlayerHP(10);
                }, collision.gameObject);
                break;

            case "Bullet Size":
                PickUpItem(() =>
                {
                    bulletSize += 0.5f;
                }, collision.gameObject);
                break;

            case "Less Bullet Size":
                PickUpItem(() =>
                {
                    bulletSize -= 0.25f;
                    if (bulletSize < 0.1f) bulletSize = 0.1f;
                }, collision.gameObject);
                break;

            case "Bullet Speed":
                PickUpItem(() => 
                { 
                    bulletSpeed += 2.5f;
                }, collision.gameObject);
                break;

            case "Hp active":
                PickUpActive("Hp active", 2, collision.gameObject);
                break;

            case "Coin active":
                PickUpActive("Coin active", 4, collision.gameObject);
                break;

            case "Shovel active":
                PickUpActive("Shovel active", 7, collision.gameObject);
                break;

            case "Potassium active":
                PickUpActive("Potassium active", 4, collision.gameObject);
                break;

            case "Strontium active":
                PickUpActive("Strontium active", 3, collision.gameObject);
                break;

            case "Lawrencium active":
                PickUpActive("Lawrencium active", 5, collision.gameObject);
                break;

            case "R key active":
                PickUpActive("R key active", 0, collision.gameObject);
                break;

            case "Semicolon active":
                PickUpActive("Semicolon active", (int)Random.Range(3, 6), collision.gameObject);
                break;

            case "Missile active":
                PickUpActive("Missile active", 2, collision.gameObject);
                break;
        }

        void PickUpActive(string name, int charges, GameObject itemGameObject)
        {
            PickUpItem(() =>
            {
                Sprite sprite = itemGameObject.GetComponent<SpriteRenderer>().sprite;

                if (currentActive != null)
                {
                    for (int i = 0; i < itemTemplates.shopItems.Length; i++)
                    {
                        if (itemTemplates.shopItems[i].tag == currentActive)
                        {
                            float horizontalInput = Input.GetAxis("Horizontal");

                            float posX = transform.position.x + (spawnActiveLeft ? -2f : 2f);
                            float posY = transform.position.y;

                            Vector2 spawnPosition = new Vector2(posX, posY);
                            Quaternion spawnRotation = Quaternion.identity;

                            GameObject spawnedActive = Instantiate(itemTemplates.shopItems[i], spawnPosition, spawnRotation);

                            spawnedActive.GetComponent<ShopItem>().cost = 0;
                            spawnedActive.GetComponent<ShopItem>().currentCharges = activeCurrentCharges;

                            spawnActiveLeft = spawnActiveLeft ? false : true;

                            break;
                        }
                    }

                    currentActive = null;
                }

                activeCurrentCharges = itemGameObject.GetComponent<ShopItem>().currentCharges;

                if (charges == 0)
                {
                    activeCharges = 1;
                    activeCurrentCharges = 1;
                }
                else
                {
                    activeCharges = charges;
                }

                currentActive = name;
                activeItemImage.sprite = sprite;

                activeItemObject.SetActive(true);
            }, itemGameObject, true);
        }

        void PickUpItem(FunctionDelegate itemLogic, GameObject itemCollision, bool isActive = false)
        {
            if (itemCollision.GetComponent<ShopItem>().cost <= coins)
            {
                itemLogic();
                showItemProperties(itemCollision.GetComponent<ShopItem>().itemName, itemCollision.GetComponent<ShopItem>().itemDescription);

                coins -= itemCollision.GetComponent<ShopItem>().cost;

                itemCollision.GetComponent<ShopItem>().apliedEffects = true;

                totalItemsPickedUp++;

                if (!isActive) logic.AddItemHUD(itemCollision.GetComponent<SpriteRenderer>().sprite);
            }
        }

        void showItemProperties(string name, string description)
        {
            itemPropertiesAnim.SetTrigger("Apear");

            itemName.text = name;
            itemDescription.text = description;
        }
    }

    void ResetFloor()
    {
        for (int i = 0; i < roomTemplates.rooms.Count; i++)
        {
            Destroy(roomTemplates.rooms[i]);
        }

        for (int i = 0; i < roomTemplates.closedRooms.Count; i++)
        {
            Destroy(roomTemplates.closedRooms[i]);
        }

        for (int i = 0; i < itemTemplates.spawnedItems.Count; i++)
        {
            Destroy(itemTemplates.spawnedItems[i]);
        }

        for (int i = 0; i < enemyTemplates.spawnedEnemies.Count; i++)
        {
            Destroy(enemyTemplates.spawnedEnemies[i]);
        }

        GameObject spawnedTrapdoor = GameObject.FindGameObjectWithTag("Trapdoor");
        GameObject spawnedTrampoline = GameObject.FindGameObjectWithTag("Trampoline");
        GameObject spawnedSpikes = GameObject.FindGameObjectWithTag("Spikes");

        if (spawnedTrapdoor != null)
            Destroy(spawnedTrapdoor);

        if (spawnedTrampoline != null)
            Destroy(spawnedTrampoline);

        if (spawnedSpikes != null)
            Destroy(spawnedSpikes);

        enemyTemplates.spawnedEnemies.Clear();
        itemTemplates.spawnedItems.Clear();
        roomTemplates.rooms.Clear();
        roomTemplates.closedRooms.Clear();

        transform.position = new Vector2(0, 0);

        Destroy(currentEntryRoom);

        currentEntryRoom = Instantiate(entryRoom, new Vector2(-12.5f, 12.5f), Quaternion.identity);

        roomTemplates.waitTime = 2f;
        roomTemplates.setedSpecialRooms = false;

        if (pickedUpHolyMantle)
        {
            logic.hasHolyMantle = true;
        }

        if (logic.recivedDamage == false)
        {
            logic.UnlockObject("Holy player", "Unlocked holy player!");
        }

        logic.recivedDamage = false;
    }
}
