using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroyer : MonoBehaviour
{
    public float destroyTime = 1f;
    private float destroyTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (destroyTimer < destroyTime)
        {
            destroyTimer += Time.deltaTime;
        } else
        {
            Destroy(gameObject);
        }
    }
}
