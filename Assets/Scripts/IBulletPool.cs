using UnityEngine;
using UnityEngine.Pool;

public abstract class IBulletPool 
{
    public int initialPoolSize = 1000;
    public int maxPoolSize = 10000;

    private ObjectPool<GameObject> bulletPool;
    

    public virtual void InitializePool(GameObject bulletPrefab)
    {
        bulletPool = new ObjectPool<GameObject>(
            () => { return Object.Instantiate(bulletPrefab, Vector3.zero, Quaternion.identity); },
            x =>  x.SetActive(true),
            y =>  y.SetActive(false),
            z => Object.Destroy(z),
            false,
            initialPoolSize,
            maxPoolSize
        );
    }

    public virtual GameObject GetBullet(Vector3 spawnPosition, Quaternion spawnRotation)
    {
        if (bulletPool == null)
        {
            Debug.LogError("Bullet pool is not initialized");
        }

        return bulletPool.Get();
    }

    public virtual void ReturnBullet(GameObject bullet)
    {
        if (bulletPool == null)
        {
            Debug.LogError("Bullet pool is not initialized and bullet is being destroyed." + bullet.ToString());
        }

        bulletPool.Release(bullet);
    }

}
