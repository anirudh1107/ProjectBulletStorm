using NUnit.Framework;
using UnityEngine;

public class EnemyBulletPool : IBulletPool
{
    private int initialPoolSize = 1000;
    private int maxPoolSize = 10000;
    public override void InitializePool(GameObject bulletPrefab, int initialPoolSize, int maxPoolSize)
    {
        base.InitializePool(bulletPrefab, initialPoolSize, maxPoolSize);
    }

    public override GameObject GetBullet(Vector3 spawnPosition, Quaternion spawnRotation)
    {
        GameObject bullet = base.GetBullet(spawnPosition, spawnRotation);
        bullet.transform.position = spawnPosition;
        bullet.transform.rotation = spawnRotation;
        return bullet;
    }

    public override void ReturnBullet(GameObject bullet)
    {
        bullet.transform.position = Vector3.zero;
        bullet.transform.rotation = Quaternion.identity;
        base.ReturnBullet(bullet);
    }

    public GameObject[] GetBulletInBatch(int batchSize, Vector3 spawnPosition, Quaternion spawnRotation)
    {
        GameObject[] bulletBatch = new GameObject[batchSize];
        if (batchSize <= base.bulletPool.CountInactive)
        {
            for (int i = 0; i < batchSize; i++)
            {
                bulletBatch[i] = GetBullet(spawnPosition, spawnRotation);
            } 
            return bulletBatch;
        }
        else
        {
            Debug.LogError("Not enough bullets in the pool. Please increase the pool size.");
            return bulletBatch;
        }
       
    }
}
