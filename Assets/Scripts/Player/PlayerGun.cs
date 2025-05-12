using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGun : MonoBehaviour
{
    [SerializeField]
    private GameObject _bulletPrefab;
    [SerializeField]
    private float _bulletCoolDown = 0.1f;
    [SerializeField]
    private float _bulletSpeed = 20f;

    public PlayerBulletPool bulletPool;

    public static Action onShoot; 
    public bool isShooting = false;
    private int initialPoolSize = 1000;
    private int maxPoolSize = 10000;

    public static PlayerGun Instance { get; private set; }

    private void OnEnable()
    {
        onShoot += StartShooting;
    }

    private void OnDisable()
    {
        onShoot -= StartShooting;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        if (bulletPool == null)
        {
            bulletPool = new PlayerBulletPool();
            bulletPool.InitializePool(_bulletPrefab); // Should Also Initialize once
        }
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            if (!isShooting)
            {
                onShoot?.Invoke();
            }
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

        GameObject bullet = bulletPool.GetPlayerBullet(transform.position, Quaternion.identity);
        //bullet.transform.position = transform.position;
        //bullet.transform.rotation = Quaternion.identity;
        bullet.GetComponent<Bullet>().Initialize(shootDirection, _bulletSpeed);
    }

    private void StartShooting()
    {
        isShooting = true;
        StartCoroutine(ShootWithCoolDown());
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
