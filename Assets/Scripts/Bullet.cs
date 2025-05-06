using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float initialBulletSpeed = 5f;
    [SerializeField] private float lifeTime = 2f;
    [SerializeField] private GameObject hitEffectPrefab;

    private Rigidbody2D rb;
    private float activationTime;
    private Action _onHit;

    private void OnEnable()
    {
        _onHit += ReturnBullet;
    }

    private void OnDisable()
    {
        _onHit -= ReturnBullet;
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Initialize(Vector2 direction, float bulletSpeed)
    {
        activationTime = Time.time;
        rb.linearVelocity = direction * bulletSpeed;
    }

    private void Update()
    {
        if (Time.time - activationTime > lifeTime)
        {
            ReturnBullet();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            // Handle collision with enemy
            // For example, you can apply damage to the enemy here
            Debug.Log("Hit Enemy");
        }
        ReturnBullet();
    }

    private void ReturnBullet()
    {      
        rb.linearVelocity = Vector2.zero;
        Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
        PlayerGun.bulletPool.ReturnBullet(this.gameObject);
    }


}
