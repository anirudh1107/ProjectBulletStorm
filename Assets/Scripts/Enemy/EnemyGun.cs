using UnityEngine;
using System;
using System.Collections;

public class EnemyGun : MonoBehaviour
{
    [SerializeField]
    private GameObject _bulletPrefab;
    [SerializeField]
    private float _bulletSpeed = 20f;
    [SerializeField]
    private float patternInterval = 0.2f;

    public static EnemyBulletPool bulletPool;

    public static Action enemyOnShoot;
    public bool isShooting = false;
    private int initialPoolSize = 400;
    private int maxPoolSize = 800;

    private void Awake()
    {
        bulletPool = new EnemyBulletPool();
        bulletPool.InitializePool(_bulletPrefab, initialPoolSize, maxPoolSize); 
    }

    private void Shoot(Vector2 shootDirection, Vector3 enemyPosition)
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        GameObject enemyBullet = bulletPool.GetBullet(enemyPosition, Quaternion.identity);
        //bullet.transform.position = transform.position;
        //bullet.transform.rotation = Quaternion.identity;
        enemyBullet.GetComponent<EnemyBullet>().Initialize(shootDirection, _bulletSpeed);
        enemyOnShoot?.Invoke();
    }

    public Vector2[] GenerateDirections(int partitions)
    {
        Vector2[] directions = new Vector2[partitions];
        float angleStep = 360f / partitions;

        for (int i = 0; i < partitions; i++)
        {
            float angle = i * angleStep;
            float radians = angle * Mathf.Deg2Rad; // Convert to radians
            directions[i] = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
        }

        return directions;
    }

    private IEnumerator SpawnCircularPatterns(int partition, Vector3 enemyPosition)
    {
        Vector2[] shootDirections = GenerateDirections(partition);
        foreach (var direction in shootDirections)
        {
            Shoot(direction, enemyPosition);
            yield return new WaitForSeconds(patternInterval);
        }
    }

    public void ShootCircularPattern(int partition, Vector3 enemyPosition)
    {
        StartCoroutine(SpawnCircularPatterns(partition, enemyPosition));
    }
}
