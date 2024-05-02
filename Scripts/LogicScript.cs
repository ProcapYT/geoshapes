using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LogicScript : MonoBehaviour
{
    public Slider hpSlider;
    public Text hpText;
    public float playerHP = 100f;
    public float maxPlayerHp = 100f;
    public GameObject gameOverScreen;
    public float score = 0;
    public Text scoreText;
    public Text gameOverScoreText;
    public Text highscoreText;
    public Animator scoreAnim;
    public Text pauseText;
    public GameObject gamePausedText;
    public PlayerScript player;

    public Sprite circle;
    public Sprite square;
    public Sprite triangle;
    public Sprite crucifix;
    public Sprite inversedCrucifix;
    public Sprite circumference;

    public Text shotSpeedText;
    public Text speedText;
    public Text damageText;
    public TextMeshPro floorText;
    public int currentFloor = 1;

    public Crossfade crossfade;

    public float highscore = 0;

    public float slowMoTimeScale = 0.5f;

    public float elipsedTime;

    public Text timeText;
    public Text coinsText;

    public bool hasHolyMantle = false;
    public GameObject holyMantleImage;

    public bool recivedDamage = false;

    public Animator notificationAnim;
    public Text notificationName;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;

        crossfade.gameObject.SetActive(true);

        highscore = PlayerPrefs.GetFloat("Highscore", 0);

        player.transform.localScale = Vector3.one;

        if (PlayerPrefs.GetString("Player square", "False") == "True")
        {
            player.gameObject.GetComponent<SpriteRenderer>().sprite = square;

            player.spectralBullets = true;
            maxPlayerHp = 110;
            playerHP = 110;
        } else
        {
            PlayerPrefs.SetString("Player square", "False");
            player.gameObject.GetComponent<SpriteRenderer>().sprite = circle;
        }

        if (PlayerPrefs.GetString("Player triangle", "False") == "True")
        {
            player.gameObject.GetComponent<SpriteRenderer>().sprite = triangle;

            player.shotSpeed = 0.25f;
        }
        
        if (PlayerPrefs.GetString("Holy player", "False") == "True")
        {
            player.gameObject.GetComponent<SpriteRenderer>().sprite = crucifix;

            player.pickedUpHolyMantle = true;
            hasHolyMantle = true;
        }

        if (PlayerPrefs.GetString("Devil player", "False") == "True")
        {
            player.gameObject.GetComponent<SpriteRenderer>().sprite = inversedCrucifix;
            player.gameObject.GetComponent<SpriteRenderer>().color = new Color(255f / 255f, 0, 0, 1f);

            player.pickedUpHolyMantle = true;
            hasHolyMantle = true;

            player.spectralBullets = true;
        }

        if (PlayerPrefs.GetString("Circumference", "False") == "True")
        {
            player.gameObject.GetComponent<SpriteRenderer>().sprite = circumference;

            player.pickedUpHolyMantle = true;
            hasHolyMantle = true;

            player.spectralBullets = true;

            player.shotSpeed /= 2f;

            player.playerDamage = 2;

            maxPlayerHp = 20;
            playerHP = 20;
        }   
    }

    // Update is called once per frame
    void Update()
    {
        elipsedTime += Time.deltaTime;

        hpSlider.maxValue = maxPlayerHp;
        hpSlider.value = playerHP;
        hpText.text = $"{playerHP.ToString()}/{maxPlayerHp.ToString()}";
        scoreText.text = score.ToString("#,0.##");
        damageText.text = player.playerDamage.ToString("#,0.##");

        shotSpeedText.text = $"{player.shotSpeed.ToString("0.##")}s";
        speedText.text = player.playerForce.ToString("0.##");
        floorText.text = $"Floor {currentFloor}";

        coinsText.text = player.coins.ToString("#,0.##");

        timeText.text = FormatTime(elipsedTime);

        if (hasHolyMantle)
        {
            holyMantleImage.SetActive(true);
        } else
        {
            holyMantleImage.SetActive(false);
        }

        if (playerHP <= 0)
        {
            if (score > highscore)
            {
                PlayerPrefs.SetFloat("Highscore", score);
                highscore = score;
            }

            gameOverScoreText.text = $"Score: {score.ToString("#,0.##")}";
            highscoreText.text = $"Highscore: {highscore.ToString("#,0.##")}";
            gameOverScreen.SetActive(true);
        } else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePause();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            crossfade.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (currentFloor >= 20)
        {
            UnlockObject("Player square", "Unlocked square player!");
        }

        if (score >= 20000)
        {
            UnlockObject("Player triangle", "Unlocked triangle player!");
        }

        if (currentFloor >= 5)
        {
            UnlockObject("Player yellow", "Unlocked yellow player!");
        }


        if (currentFloor <= -5)
        {
            UnlockObject("Devil player", "Unlocked devil player!");
        }

        if (score >= 100000)
        {
            UnlockObject("Player hat", "Unlocked player hat!");
        }

        if (PlayerPrefs.GetInt("Enemies killed", 0) >= 1000)
        {
            UnlockObject("Circumference", "Unlocked circumference!");
        }
    }

    public void UnlockObject(string name, string notificationText)
    {
        if (PlayerPrefs.GetString($"{name} unlocked", "False") == "False")
        {
            PlayerPrefs.SetString($"{name} unlocked", "True");
            notificationAnim.SetTrigger("Apear");
            notificationName.text = notificationText;
        }
    }

    public void RemovePlayerHP(float hp)
    {
        if (hasHolyMantle)
        {
            hasHolyMantle = false;
        }
        else
        {
            playerHP -= hp;

            if (playerHP < 0)
            {
                playerHP = 0;
            }

            recivedDamage = true;
        }
    }

    public void AddPlayerHP(float hp)
    {
        playerHP += hp;

        playerHP = Mathf.Min(playerHP, maxPlayerHp);
    }

    public void AddScore(float scoreToAdd)
    {
        scoreAnim.SetTrigger("Gained Score");
        score += scoreToAdd;
    }

    public void TogglePause()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            pauseText.text = "Pause";
            gamePausedText.SetActive(false);
        } else
        {
            Time.timeScale = 0;
            pauseText.text = "Play";
            gamePausedText.SetActive(true);
        }
    }

    string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);

        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
