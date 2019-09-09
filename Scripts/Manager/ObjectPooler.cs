using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectToPool
{
    public GameObject GetObject
    {
        get
        {
            for (int i = 0; i < amount; i++)
                if (!pool[i].activeInHierarchy)
                    return pool[i];

            Debug.LogError("Object pool is full");
            return null;
        }
    }

    public int amount;
    public GameObject gameObject;
    [System.NonSerialized]
    public List<GameObject> pool;
}


public class ObjectPooler : PumpFramework.Common.Singleton<ObjectPooler>
{
    [SerializeField]
    private Transform objectPool;

    public ObjectToPool enemyBullet;
    public ObjectToPool playerBullet;

    private void Start()
    {
        Initialize(enemyBullet);
        Initialize(playerBullet);
    }

    private void Initialize(ObjectToPool poolObject)
    {
        poolObject.pool = new List<GameObject>();
        for (int i = 0; i < poolObject.amount; i++)
        {
            GameObject newObj = Instantiate(poolObject.gameObject, objectPool);
            newObj.SetActive(false);
            poolObject.pool.Add(newObj);
        }
    }
}
