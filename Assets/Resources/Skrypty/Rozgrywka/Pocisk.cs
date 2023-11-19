using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    
    public GameObject bulletPrefab;

    
    public float bulletSpeed = 5f;

   
    public float shootInterval = 10f;

    
    public int bulletCount = 8;

    
    private float angleStep;

    
    private float shootTimer;

    
    void Start()
    {
        
        angleStep = 360f / bulletCount;

        
        shootTimer = 0f;
    }

    
    void Update()
    {
        
        shootTimer += Time.deltaTime;

        
        if (shootTimer >= shootInterval)
        {
            
            Shoot();

            shootTimer = 0f;
        }
    }

    void Shoot()
    {
        float startAngle = 0f;

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = startAngle + angleStep * i;

            Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

            bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
        }
    }
}
