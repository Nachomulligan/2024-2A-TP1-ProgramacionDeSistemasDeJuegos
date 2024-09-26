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
        if (frequency > 0) period = 1 / frequency;
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);

        objectPooler = ObjectPooler.Instance;

        while (true) 
        {
            for (int i = 0; i < spawnsPerPeriod; i++)
            {
                GameObject enemyObj = objectPooler.SpawnFromPool("Enemy", transform.position, Quaternion.identity);

                Enemy enemy = enemyObj.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.OnSpawnFromPool();  
                }
           
            }

            yield return new WaitForSeconds(period);
        }
    }
}
