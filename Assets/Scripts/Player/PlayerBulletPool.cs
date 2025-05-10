using UnityEngine;
using UnityEngine.Pool;

public class PlayerBulletPool
{
    protected ObjectPool<GameObject> bulletPool;
    private int initialPoolSize = 1000;
    private int maxPoolSize = 10000;

    public void InitializePool(GameObject bulletPrefab)
    {
        bulletPool = new ObjectPool<GameObject>(
            () => { return Object.Instantiate(bulletPrefab, Vector3.zero, Quaternion.identity); },
            x => x.SetActive(true),
            y => y.SetActive(false),
            z => Object.Destroy(z),
            false,
            initialPoolSize,
            maxPoolSize
        );
    }

    private GameObject GetBullet(Vector3 spawnPosition, Quaternion spawnRotation)
    {
        if (bulletPool == null)
        {
            Debug.LogError("Bullet pool is not initialized");
        }

        return bulletPool.Get();
    }

    private void ReturnBullet(GameObject bullet)
    {
        if (bulletPool == null)
        {
            Debug.LogError("Bullet pool is not initialized and bullet is being destroyed." + bullet.ToString());
        }

        bulletPool.Release(bullet);
    }

    public GameObject GetPlayerBullet(Vector3 spawnPosition, Quaternion spawnRotation)
    {
        GameObject bullet = GetBullet(spawnPosition, spawnRotation);
        bullet.transform.position = spawnPosition;
        bullet.transform.rotation = spawnRotation;
        return bullet;
    }

    public void ReturnPlayerBullet(GameObject bullet)
    {
        bullet.transform.position = Vector3.zero;
        bullet.transform.rotation = Quaternion.identity;
        ReturnBullet(bullet);
    }
}
