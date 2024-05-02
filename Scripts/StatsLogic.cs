using TMPro;
using UnityEngine;

public class StatsLogic : MonoBehaviour
{
    public TextMeshProUGUI enemiesKilledText;
    public TextMeshProUGUI highscoreText;
    public TextMeshProUGUI itemsPickedUpText;

    private int enemiesKilled;
    private int highscore;
    private int itemsPickedUp;

    // Start is called before the first frame update
    void Start()
    {
        enemiesKilled = PlayerPrefs.GetInt("Enemies killed", 0);
        highscore = (int)PlayerPrefs.GetFloat("Highscore", 0);
        itemsPickedUp = PlayerPrefs.GetInt("Items picked up", 0);

        enemiesKilledText.text = FormatNumber(enemiesKilled);
        highscoreText.text = FormatNumber(highscore);
        itemsPickedUpText.text = FormatNumber(itemsPickedUp);
    }

    string FormatNumber(int number)
    {
        if (number >= 1000000000)
            return (number / 1000000000).ToString("N1") + "B";

        if (number >= 1000000)
            return (number / 1000000).ToString("N1") + "M";

        if (number >= 1000)
            return (number / 1000).ToString("N1") + "k";
        
        return number.ToString();
    }
}
