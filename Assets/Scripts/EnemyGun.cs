using UnityEngine;
using System;
using System.Collections;

public class EnemyGun : MonoBehaviour
{
    [SerializeField]
    private GameObject _bulletPrefab;
    [SerializeField]
    private float _bulletCoolDown = 0.1f;
    [SerializeField]
    private float _bulletSpeed = 20f;

    public static PlayerBulletPool bulletPool;

    public static Action onShoot;
    public bool isShooting = false;
    private int initialPoolSize = 1000;
    private int maxPoolSize = 10000;

    private void Awake()
    {
        if (bulletPool == null)
        {
            bulletPool = new PlayerBulletPool();
            bulletPool.InitializePool(_bulletPrefab, initialPoolSize, maxPoolSize); 
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isShooting)
        {
            StartCoroutine(ShootWithCoolDown());
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            isShooting = true;
        }
        else
        {
            isShooting = false;
        }
    }

    private void Shoot()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 shootDirection = (mousePosition - transform.position).normalized;

        GameObject bullet = bulletPool.GetBullet(transform.position, Quaternion.identity);
        //bullet.transform.position = transform.position;
        //bullet.transform.rotation = Quaternion.identity;
        bullet.GetComponent<Bullet>().Initialize(shootDirection, _bulletSpeed);
        onShoot?.Invoke();
    }

    private IEnumerator ShootWithCoolDown()
    {
        while (isShooting)
        {
            Shoot();
            yield return new WaitForSeconds(_bulletCoolDown);
        }

    }
}
