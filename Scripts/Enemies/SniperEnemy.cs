using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperEnemy : MonoBehaviour
{
    public float shotSpeed = 2f;
    private float shotTimer = -2f;
    public GameObject sniperBullet;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (shotTimer < shotSpeed)
        {
            shotTimer += Time.deltaTime;
        } else
        {
            Instantiate(sniperBullet, transform.position, transform.rotation);

            shotTimer = 0;
        }
    }
}
