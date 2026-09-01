using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PoolManager : Singleton<PoolManager>
{
    [System.Serializable]
    public class Pool
    {
        [HideInInspector] public string poolName;
        [HideLabel] public GameObject objectToPool;
        [HorizontalGroup("Amount", Gap = 10)] [LabelText("Amount")] public int amountToPool;
        [HorizontalGroup("Amount")] [LabelText("Expand?")] public bool shouldExpand = true;
        [HideInInspector] public List<GameObject> pooledObjects;
    }

    [ListDrawerSettings(NumberOfItemsPerPage = 10, ShowItemCount = true)]  [Searchable]
    public List<Pool> pools;

    protected override void Awake()
    {
        base.Awake();
        
        InitializePools();
    }

    void InitializePools()
    {
        foreach (Pool pool in pools)
        {
            pool.poolName = pool.objectToPool.name;
            pool.pooledObjects = new List<GameObject>();
            for (int i = 0; i < pool.amountToPool; i++)
            {
                GameObject obj = Instantiate(pool.objectToPool, transform);
                obj.SetActive(false);
                pool.pooledObjects.Add(obj);
            }
        }
    }

    [Button("Load poolable objects")]
    void Load()
    {
        pools.Clear();
        foreach (GameObject prefab in Resources.LoadAll<GameObject>("Prefabs"))
        {
            IPoolable poolableComponent = prefab.GetComponent<IPoolable>();
            if (poolableComponent != null)
            {
                Pool newPool = new Pool
                {
                    poolName = poolableComponent.Name,
                    objectToPool = poolableComponent.Prefab,
                    amountToPool = poolableComponent.AmountToPool,
                    shouldExpand = poolableComponent.ShouldExpand
                };
                pools.Add(newPool);
            }
        }
    }

    public GameObject SpawnFromPool(string poolName, Vector3 position = default, Quaternion rotation = default)
    {
        Pool pool = pools.Find(p => p.poolName == poolName);
        if (pool == null)
        {
            Debug.LogWarning($"Pool with name {poolName} doesn't exist.");
            return null;
        }

        foreach (var obj in pool.pooledObjects)
        {
            if (!obj.activeInHierarchy)
            {
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                obj.SetActive(true);
                return obj;
            }
        }

        if (pool.shouldExpand)
        {
            GameObject obj = Instantiate(pool.objectToPool, position, rotation, transform);
            pool.pooledObjects.Add(obj);
            return obj;
        }

        return null; // No object available and pool cannot expand.
    }

    public void ReturnAll()
    {
        foreach (Pool pool in pools)
        {
            foreach(GameObject pooledObject in pool.pooledObjects)
            {
                pooledObject.SetActive(false);
            }
        }
    }

    // Method to return an object to the pool (simply deactivates it).
    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
    }
}
