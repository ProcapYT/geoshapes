using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastBoss : MonoBehaviour
{
    public GameObject fastEnemy;
    public float spawnSpeed = 2f;
    private float spawnTimer = -2f;
    private Vector2 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.identity;
        transform.position = startPosition;

        if (spawnTimer < spawnSpeed)
        {
            spawnTimer += Time.deltaTime;
        } else
        {
            float heightOffset = Random.Range(transform.position.y - 5, transform.position.y + 5);
            float widthOffset = Random.Range(transform.position.x - 5, transform.position.x + 5);

            Instantiate(fastEnemy, new Vector2(widthOffset, heightOffset), Quaternion.identity);

            spawnTimer = 0;
        }
    }
}
