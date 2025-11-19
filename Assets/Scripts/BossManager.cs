using UnityEngine;
using System.Collections;

public class BossManager : MonoBehaviour
{
    public static BossManager instance;

    [Header("Boss Settings")]
    public int currentHealth = 100;
    public string bossName;
    public BattlePhase[] phases;
    public int currentPhase;

    [Header("Boss Components")]
    public Animator bossAnim;
    public GameObject endExplosion;
    public Transform theBoss;

    [Header("Battle Settings")]
    public bool battleEnding;
    public float timeToExplosionEnd;
    public float waitToEndLevel;

    [Header("Enemy Spawn Settings")]
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject[] enemiesToSpawn;
    [SerializeField] private float spawnInterval = 2f;
    private bool isSpawningEnemies;

    void Awake()
    {

        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    void Start()
    {
        MusicController.instance.PlayBossMusic();

        if (UIManager.instance != null)
        {
            if (UIManager.instance.bossName != null)
                UIManager.instance.bossName.text = bossName;

            if (UIManager.instance.bossSlider != null)
            {
                UIManager.instance.bossSlider.maxValue = currentHealth;
                UIManager.instance.bossSlider.value = currentHealth;
                UIManager.instance.bossSlider.gameObject.SetActive(true);
            }
        }
    }

    void Update()
    {
        if (phases == null || currentPhase >= phases.Length || battleEnding) return;

        HandlePhaseShots();

        if (currentHealth <= phases[currentPhase].healthToEndPhase)
        {
            HandlePhaseTransition();
        }

        if (currentPhase == 0 && !isSpawningEnemies)
        {
            isSpawningEnemies = true;
            StartCoroutine(SpawnEnemies());
        }
    }

    private void HandlePhaseShots()
    {
        foreach (var shot in phases[currentPhase].phaseShots)
        {
            shot.shotCounter -= Time.deltaTime;
            if (shot.shotCounter <= 0)
            {
                shot.shotCounter = shot.timeBetwweenShots;

                if (shot.theShot != null && shot.firePoint != null)
                {
                    Instantiate(shot.theShot, shot.firePoint.position, shot.firePoint.rotation);
                }
            }
        }
    }

    private void HandlePhaseTransition()
    {
        phases[currentPhase].removeAtPhaseEnd?.SetActive(false);

        if (phases[currentPhase].addAtPhaseEnd != null && phases[currentPhase].newSpawnPoint != null)
        {
            Instantiate(phases[currentPhase].addAtPhaseEnd, phases[currentPhase].newSpawnPoint.position, phases[currentPhase].newSpawnPoint.rotation);
        }

        if (currentPhase + 1 < phases.Length)
        {
            currentPhase++;
            bossAnim.SetInteger("Phase", currentPhase + 1);

            if (currentPhase != 0)
            {
                isSpawningEnemies = false;
                StopCoroutine(SpawnEnemies());
            }
        }
    }
    private IEnumerator SpawnEnemies()
    {
        while (isSpawningEnemies)
        {
            if (enemiesToSpawn.Length > 0 && spawnPoints.Length > 0)
            {
                GameObject enemyToSpawn = enemiesToSpawn[Random.Range(0, enemiesToSpawn.Length)];

                Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

                Instantiate(enemyToSpawn, spawnPoint.position, spawnPoint.rotation);
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public void HurtBoss()
    {
        currentHealth = Mathf.Max(0, currentHealth - 1);

        if (UIManager.instance != null && UIManager.instance.bossSlider != null)
        {
            UIManager.instance.bossSlider.value = currentHealth;
        }

        if (currentHealth <= 0 && !battleEnding)
        {
            battleEnding = true;
            StartCoroutine(EndBattleCo());
        }
    }

    public IEnumerator EndBattleCo()
    {
        GameManager.instance.AddScore(10000000);

        if (UIManager.instance != null && UIManager.instance.bossSlider != null)
        {
            UIManager.instance.bossSlider.gameObject.SetActive(false);
        }

        Instantiate(endExplosion, theBoss.position, theBoss.rotation);
        bossAnim.enabled = false;

        yield return new WaitForSeconds(timeToExplosionEnd);

        theBoss.gameObject.SetActive(false);

        yield return new WaitForSeconds(waitToEndLevel);

        StartCoroutine(GameManager.instance.EndLevelCo());
    }
}

[System.Serializable]
public class BattleShot
{
    public GameObject theShot;
    public float timeBetwweenShots;
    public Transform firePoint;
    public float shotCounter;
}

[System.Serializable]
public class BattlePhase
{
    public BattleShot[] phaseShots;
    public int healthToEndPhase;
    public GameObject removeAtPhaseEnd;
    public GameObject addAtPhaseEnd;
    public Transform newSpawnPoint;
}