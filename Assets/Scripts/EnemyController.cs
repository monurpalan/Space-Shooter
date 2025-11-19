using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector2 startDirection;
    [SerializeField] private bool shouldChangeDirection;
    [SerializeField] private float changeDirectionXPoint;
    [SerializeField] private Vector2 changedDirection;

    [Header("Shooting Settings")]
    [SerializeField] private GameObject shotFire;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform firePoint2;
    [SerializeField] private float minTimeBetweenShots = 1f;
    [SerializeField] private float maxTimeBetweenShots = 3f;

    private float shotCounter1;
    private float shotCounter2;
    private bool allowShooting;

    [Header("Health & Death Settings")]
    [SerializeField] private int currentHealth;
    [SerializeField] private GameObject deathEffect;
    [SerializeField] private Vector3 bulletSize = Vector3.one;
    [SerializeField] private int scoreValue = 100;

    [Header("Power-Up Settings")]
    [SerializeField] private GameObject[] powerUps;
    [SerializeField] private int dropSuccessRate = 1;

    void Start()
    {
        shouldChangeDirection = Random.value > 0.5f;
        SetRandomTimeBetweenShots();
    }

    void Update()
    {
        MoveEnemy();
        if (allowShooting) HandleShooting();
    }

    private void MoveEnemy()
    {
        Vector2 enemyMove = transform.position;
        Vector2 direction = shouldChangeDirection && enemyMove.x <= changeDirectionXPoint
            ? changedDirection
            : startDirection;

        enemyMove += direction * moveSpeed * Time.deltaTime;
        transform.position = enemyMove;
    }

    private void HandleShooting()
    {
        shotCounter1 -= Time.deltaTime;
        if (shotCounter1 <= 0)
        {
            Shoot(firePoint);
            shotCounter1 = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }

        if (firePoint2 != null)
        {
            shotCounter2 -= Time.deltaTime;
            if (shotCounter2 <= 0)
            {
                Shoot(firePoint2);
                shotCounter2 = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
            }
        }
    }

    private void Shoot(Transform firePoint)
    {
        if (firePoint == null || shotFire == null) return;

        GameObject newBullet = Instantiate(shotFire, firePoint.position, firePoint.rotation);
        newBullet.transform.localScale = bulletSize;
    }

    private void SetRandomTimeBetweenShots()
    {
        shotCounter1 = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        shotCounter2 = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    public void HurtEnemy()
    {
        currentHealth--;
        if (currentHealth > 0) return;

        TryDropPowerUp();
        GameManager.instance.AddScore(scoreValue);
        Instantiate(deathEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    private void TryDropPowerUp()
    {
        int randomChance = Random.Range(0, 100);
        if (randomChance >= dropSuccessRate) return;

        int randomPick = Random.Range(0, 100);
        int index = randomPick < 50 ? 0 : randomPick < 80 ? 1 : 2;
        Instantiate(powerUps[index], transform.position, transform.rotation);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnBecameVisible()
    {
        allowShooting = true;
    }
}