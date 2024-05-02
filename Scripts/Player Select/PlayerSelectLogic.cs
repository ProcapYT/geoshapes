using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectLogic : MonoBehaviour
{
    public GameObject itemUnlock;
    public GameObject activateButton;
    public GameObject itemDescription;

    public GameObject[] shopItems;

    public Crossfade crossfade;

    public Color itemSelectedColor = new Color(32f / 255f, 32f / 255f, 32f / 255f, 1f);
    public Color itemNonSelectedColor = new Color(0, 0, 0, 1f);

    void Awake()
    {
        PlayerPrefs.SetString("Circle unlocked", "True");
    }

    public void SelectShopItem(string name)
    {
        itemUnlock.SetActive(true);
        activateButton.SetActive(true);
        itemDescription.SetActive(true);

        activateButton.GetComponent<Button>().interactable = false;
        activateButton.GetComponent<Button>().onClick.RemoveAllListeners();

        switch (name)
        {
            case "Circle":
                SelectItem(
                    name,
                    "Normal Stats",
                    "Normal Stats",
                    "",
                    ""
                );

                break;

            case "Player square":
                SelectItem(
                    name,
                    "Start with spectral bullets.",
                    "Start with spectral bullets.",
                    "Unlock by reaching floor 20 in a run.",
                    ""
                );

                break;

            case "Player triangle":
                SelectItem(
                    name,
                    "Start with 0.25s shot speed.",
                    "Start with 0.25s shot speed.",
                    "Unlock by getting 20,000 score in a run.",
                    ""
                );

                break;

            case "Holy player":
                SelectItem(
                    name,
                    "Start with holy mantle.",
                    "Start with holy mantle.",
                    "Unlock by not reciving any damage for a full floor.",
                    ""
                );

                break;

            case "Devil player":
                SelectItem(
                    name,
                    "??? ???",
                    "You feel holy.",
                    "???",
                    ""
                );

                break;

            case "Circumference":
                SelectItem(
                    name,
                    "???",
                    "???",
                    "Unlock by killing 1K enemies in total.",
                    ""
                );

                break;
        }
    }

    void SelectItem(string name, string itemDescriptionText, string unlockedItemDescriptionText, string itemUnlockText, string unlockedItemUnlockText)
    {
        itemUnlock.GetComponent<Text>().text = itemUnlockText;
        itemDescription.GetComponent<Text>().text = itemDescriptionText;

        if (PlayerPrefs.GetString($"{name} unlocked", "False") == "True")
        {
            activateButton.GetComponent<Button>().interactable = true;

            activateButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                ActivateItem(name);
                crossfade.LoadScene("Game");
            });

            itemUnlock.GetComponent<Text>().text = unlockedItemUnlockText;
            itemDescription.GetComponent<Text>().text = unlockedItemDescriptionText;
        }

        for (int i = 0; i < shopItems.Length; i++)
        {
            Image background = shopItems[i].transform.Find("Background").gameObject.GetComponent<Image>();

            background.color = (shopItems[i].name == name) ? itemSelectedColor : itemNonSelectedColor;
        }
    }

    void ActivateItem(string name)
    {
        for (int i = 0; i < shopItems.Length; i++)
        {
            if (name != shopItems[i].name) PlayerPrefs.SetString(shopItems[i].name, "False");
            else PlayerPrefs.SetString(name, "True");
        }
    }
}
