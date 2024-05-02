using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileBoss : MonoBehaviour
{
    public GameObject missile;
    public float spawnTime = 2f;
    private float spawnTimer = 0;

    private Vector2 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        transform.position = startPosition;

        if (spawnTimer < spawnTime)
        {
            spawnTimer += Time.deltaTime;
        } else
        {
            float randomHeight = Random.Range(transform.position.y - 3f, transform.position.y + 3f);
            float randomWidth = Random.Range(transform.position.x - 3f, transform.position.x + 3f);

            Vector2 spawnPosition = new Vector2(randomWidth, randomHeight);

            Instantiate(missile, spawnPosition, transform.rotation);
            spawnTimer = 0;
        }
    }
}
