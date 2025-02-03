using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailSpawner : MonoBehaviour
{
    [SerializeField]
    private Transform[] spawnPoints; // Array of spawn points

    [SerializeField]
    private float spawnInterval = 3f; // Time between spawns

    private void Start()
    {
        StartCoroutine(SpawnSnails());
    }

    private IEnumerator SpawnSnails()
    {
        var loopCount = 0;
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (spawnPoints.Length == 0) yield break;

            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            GameObject snail = SnailEnemyPool.Instance.GetFromPool();
            snail.transform.position = spawnPoint.position;

            if (spawnInterval > 1f)
            {
                loopCount++;
            }

            // after several time, reduce the spawn interval
            if (spawnInterval > 1f && loopCount % 5 == 0)
            {
                spawnInterval -= 0.1f;
            }
            else if (spawnInterval <= 1f)
            {
                spawnInterval = 1f;
            }
        }
    }
}
