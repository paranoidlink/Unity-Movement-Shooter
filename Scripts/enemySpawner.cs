using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawner : MonoBehaviour
{
public GameObject enemyPrefab;

public float spawnRate = 1f;
public float spawnRateIncreasePerSecond = 0.1f;
private float spawnTimer = 0f;
private float gameTime = 0f;

public Transform spawnAreaTopLeft;
public Transform spawnAreaBottomRight;

private void Update()
    {
        gameTime += Time.deltaTime;

        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            SpawnEnemy();
            spawnTimer = 1f / (spawnRate + spawnRateIncreasePerSecond * gameTime);
        }
    }

    private void SpawnEnemy()
    {
        //Generate a random position within the spawn area
        float randomX = Random.Range(spawnAreaTopLeft.position.x, spawnAreaBottomRight.position.x);
        float randomZ = Random.Range(spawnAreaTopLeft.position.z, spawnAreaBottomRight.position.z);

        Vector3 spawnPos = new Vector3(randomX, spawnAreaTopLeft.position.y, randomZ);

        // Instantiate the enemy at the randomly generated position
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

    }
}
