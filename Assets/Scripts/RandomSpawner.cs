using UnityEngine;
using System.Collections;

public class RandomSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject[] spritesToSpawn;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float minY = -5f;
    [SerializeField] private float maxY = 5f;
    [SerializeField] private float minSpawnInterval = 10f;
    [SerializeField] private float maxSpawnInterval = 20f;

    private void Start()
    {
        StartCoroutine(SpawnRandomSpriteCoroutine());
    }

    private IEnumerator SpawnRandomSpriteCoroutine()
    {
        while (true)
        {
            SpawnRandomSprite();

            float randomInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(randomInterval);
        }
    }

    private void SpawnRandomSprite()
    {
        if (spritesToSpawn.Length == 0 || spawnPoints.Length == 0)
        {
            Debug.LogWarning("Sprites or spawn points are not assigned!");
            return;
        }

        GameObject spriteToSpawn = spritesToSpawn[Random.Range(0, spritesToSpawn.Length)];
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        float randomY = Random.Range(minY, maxY);
        Vector3 spawnPosition = new Vector3(spawnPoint.position.x, randomY, spawnPoint.position.z);

        Instantiate(spriteToSpawn, spawnPosition, Quaternion.identity);
    }
}