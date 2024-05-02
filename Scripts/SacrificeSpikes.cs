using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioactiveBarrel : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            float normalItemSpawnProbab = Random.Range(0, 1f);

            if (normalItemSpawnProbab <= 0.5f)
            {
                GameObject[] items = GameObject.Find("Item Templates").GetComponent<ItemTemplates>().normalItems;
                GameObject itemToSpawn = items[Random.Range(0, items.Length)];

                float posX = transform.position.x;
                float posY = transform.position.y - 2.5f;

                Instantiate(itemToSpawn, new Vector2(posX, posY), Quaternion.identity);
            } else
            {
                float itemSpawnProbab = Random.Range(0, 1f);

                if (itemSpawnProbab <= 0.1f)
                {
                    GameObject[] items = GameObject.Find("Item Templates").GetComponent<ItemTemplates>().shopItems;
                    GameObject itemToSpawn = items[Random.Range(0, items.Length)];

                    float posX = transform.position.x;
                    float posY = transform.position.y + 2.5f;

                    GameObject spawnedItem = Instantiate(itemToSpawn, new Vector2(posX, posY), Quaternion.identity);

                    spawnedItem.GetComponent<ShopItem>().cost = 0;
                }
            }

            GameObject.Find("Logic Object").GetComponent<LogicScript>().RemovePlayerHP(10);
        }
    }
}
