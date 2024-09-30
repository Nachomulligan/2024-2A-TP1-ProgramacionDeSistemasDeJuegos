using Enemies;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public static ObjectPooler Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDict;
    private Vector3 vectorOnNavMesh = new Vector3(0, 21, 10);
    private void Start()
    {
        // initialize the pool dictionary and create queues for each pool
        poolDict = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, vectorOnNavMesh, Quaternion.identity);
                obj.SetActive(false);
                
                objectPool.Enqueue(obj);
            }
            // add the queue to the dictionary with its tags
            poolDict.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDict.ContainsKey(tag))
        {
            // check if the tag exists in the pool dictionary
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return null;
        }
        // dequeue the first object from the pool, activate it, and set its position/rotation
        GameObject objectToSpawn = poolDict[tag].Dequeue();
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        poolDict[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}