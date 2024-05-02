using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    private ParticleSystem itemParticles;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;

    public int cost;
    public float destroyTime = 1f;
    private float destroyTimer = 0;

    private ItemTemplates itemTemplates;

    public bool apliedEffects = false;
    private bool exploded = false;

    public string itemName;
    public string itemDescription;

    public int currentCharges;
    private void Start()
    {
        transform.Find("Price").gameObject.GetComponent<TextMeshPro>().text = "$" + cost.ToString();

        itemTemplates = GameObject.Find("Item Templates").GetComponent<ItemTemplates>();

        itemTemplates.spawnedItems.Add(gameObject);

        itemParticles = GetComponent<ParticleSystem>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();

        itemParticles.Stop();
        itemParticles.Clear();
    }

    private void Update()
    {
        if (cost == 0)
        {
            transform.Find("Price").gameObject.SetActive(false);
        }

        if (apliedEffects)
        {
            if (destroyTimer < destroyTime)
            {
                destroyTimer += Time.deltaTime;
            }
            else
            {
                Destroy(gameObject);
            }

            if (exploded == false)
            {
                itemParticles.Play();
                spriteRenderer.enabled = false;
                boxCollider.enabled = false;

                transform.Find("Price").gameObject.SetActive(false);

                exploded = true;
            }
        }
    }
}
