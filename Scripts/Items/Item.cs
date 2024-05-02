using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public float destroyTime = 1f;
    private float destroyTimer = 0;
    private bool pickedUp = false;
    private ParticleSystem itemParticles;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;

    private ItemTemplates itemTemplates;

    public string itemName;

    private void Start()
    {
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
        if (pickedUp)
        {
            if (destroyTimer < destroyTime)
            {
                destroyTimer += Time.deltaTime;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            pickedUp = true;
            itemParticles.Play();
            boxCollider.enabled = false;
            spriteRenderer.enabled = false;
        }
    }
}
