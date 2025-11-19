using UnityEngine;

public class WavesManager : MonoBehaviour
{
    public static WavesManager instance;

    [Header("Wave Settings")]
    [SerializeField] private WaveObject[] waves;
    [SerializeField] private int currentWave;
    [SerializeField] private float timeToNextWave;
    [SerializeField] private Transform spawnPoint;
    public bool canSpawnWaves;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (waves.Length > 0)
        {
            timeToNextWave = waves[0].timeToSpawn;
        }
        else
        {
            Debug.LogWarning("No waves configured in WavesManager!");
        }
    }

    private void Update()
    {
        if (canSpawnWaves)
        {
            HandleWaveSpawning();
        }
    }

    private void HandleWaveSpawning()
    {
        timeToNextWave -= Time.deltaTime;

        if (timeToNextWave <= 0)
        {
            SpawnWave();

            if (currentWave < waves.Length - 1)
            {
                currentWave++;
                timeToNextWave = waves[currentWave].timeToSpawn;
            }
            else
            {
                canSpawnWaves = false;
            }
        }
    }

    private void SpawnWave()
    {
        if (waves[currentWave].theWave != null)
        {
            Instantiate(waves[currentWave].theWave, spawnPoint.position, spawnPoint.rotation);
        }
        else
        {
            Debug.LogWarning($"Wave {currentWave} has no assigned prefab!");
        }
    }

    public void ContinueSpawning()
    {
        if (currentWave < waves.Length - 1 && timeToNextWave < 0)
        {
            canSpawnWaves = true;
        }
    }
}

[System.Serializable]
public class WaveObject
{
    public float timeToSpawn;
    public EnemyWave theWave;
}