using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossScript : MonoBehaviour
{
    private Slider bossHpBar;
    public float bossHp;
    public EnemyScript enemyScript;
    public bool isTankBoss;
    public string bossName;

    private TextMeshProUGUI bossHpText;
    // Start is called before the first frame update
    void Start()
    {
        bossHpBar = FindAnyObjectByType<Canvas>().transform.Find(isTankBoss ? "Large Boss HP bar" : "Boss HP bar").GetComponent<Slider>();
        bossHpText = FindAnyObjectByType<Canvas>().transform.Find(isTankBoss ? "Large Boss HP bar" : "Boss HP bar").Find("Boss HP").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI bossNameText = FindAnyObjectByType<Canvas>().transform.Find(isTankBoss ? "Large Boss HP bar" : "Boss HP bar").Find("Boss Name").GetComponent<TextMeshProUGUI>();

        bossNameText.text = bossName;

        bossHpBar.maxValue = enemyScript.enemyHealth;

        bossHpBar.gameObject.SetActive(true);

        enemyScript.isBoss = true;
        enemyScript.bossHpBar = bossHpBar;
    }

    // Update is called once per frame
    void Update()
    {
        bossHp = enemyScript.enemyHealth;

        bossHpBar.value = bossHp;
        bossHpText.text = bossHp.ToString();
    }
}
