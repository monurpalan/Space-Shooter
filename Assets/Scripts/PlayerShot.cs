using UnityEngine;

public class PlayerShot : MonoBehaviour
{
    [SerializeField] private float playerShotSpeed = 7f;
    [SerializeField] private GameObject impactEffect;
    [SerializeField] private GameObject objectExplosion;

    void Update()
    {
        MoveShot();
    }

    private void MoveShot()
    {
        transform.position += Vector3.right * playerShotSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Space_Object"))
        {
            HandleSpaceObjectCollision(other);
        }
        else if (other.CompareTag("Enemy"))
        {
            HandleEnemyCollision(other);
        }
        else if (other.CompareTag("Boss"))
        {
            HandleBossCollision();
        }

        Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    private void HandleSpaceObjectCollision(Collider2D other)
    {
        Instantiate(objectExplosion, other.transform.position, other.transform.rotation);
        Destroy(other.gameObject);
        GameManager.instance.AddScore(50);
    }

    private void HandleEnemyCollision(Collider2D other)
    {
        other.GetComponent<EnemyController>()?.HurtEnemy();
    }

    private void HandleBossCollision()
    {
        BossManager.instance?.HurtBoss();
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}