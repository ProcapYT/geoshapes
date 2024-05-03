using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    public float bulletSpeed = 20f;

    public GameObject bulletExplosion;
    public GameObject redParticles;

    public Color spectralBulletsColor;

    private PlayerScript player;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerScript>();

        if (player.spectralBullets)
        {
            SetBulletColor(spectralBulletsColor);
        }

        if (PlayerPrefs.GetString("Devil player", "False") == "True")
        {
            SetBulletColor(new Color(255f / 255f, 0, 0, 1f));
        }

        if (PlayerPrefs.GetString("Devil player", "False") == "True" && player.spectralBullets)
        {
            SetBulletColor(new Color(255f / 255f, 170f / 255f, 170f / 255f, 1f));
        }
    }

    void SetBulletColor(Color color)
    {
        GetComponent<SpriteRenderer>().color = color;
        GetComponent<TrailRenderer>().startColor = color;
        GetComponent<TrailRenderer>().endColor = color;
    }

    void FixedUpdate()
    {
        Vector3 direction = transform.up;

        rb.velocity = direction * bulletSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Enemy")
        {
            if (PlayerPrefs.GetString("Devil player", "False") == "True")
            {
                Instantiate(redParticles, transform.position, Quaternion.identity);
            } else
            {
                Instantiate(bulletExplosion, transform.position, Quaternion.identity);
            }

            if (collision.gameObject.tag == "Enemy" && player.spectralBullets)
            {
                return;
            }

            Destroy(gameObject);
        }
    }
}
