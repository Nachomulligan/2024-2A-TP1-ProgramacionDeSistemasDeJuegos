using Enemies;
using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject characterPrefab;
    [SerializeField] private int spawnsPerPeriod = 10;
    [SerializeField] private float frequency = 30;
    [SerializeField] private float period = 0;

    private ObjectPooler objectPooler;

    private void OnEnable()
    {
        // on enable, calculates the period based on the frequency
        if (frequency > 0) period = 1 / frequency;
    }

    private IEnumerator Start()
    {
        // coroutine to spawn characters at a specified frequency, spawns enemy from pool and initializes his state
        yield return new WaitForSeconds(1f);

        objectPooler = ObjectPooler.Instance;

        while (true)
        {
            for (int i = 0; i < spawnsPerPeriod; i++)
            {
                Vector3 spawnPosition = transform.position;
                GameObject enemyObj = objectPooler.SpawnFromPool("Enemy", spawnPosition, Quaternion.identity);
                Enemy enemy = enemyObj.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.OnSpawnFromPool(spawnPosition);
                }

            }

            yield return new WaitForSeconds(period);
        }
    }
}