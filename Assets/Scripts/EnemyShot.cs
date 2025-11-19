using UnityEngine;

public class EnemyShot : MonoBehaviour
{
    [SerializeField] private float shotSpeed = 7f;
    [SerializeField] private GameObject impactEffect;

    void Update()
    {
        MoveShot();
    }

    private void MoveShot()
    {
        transform.position += Vector3.left * shotSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (impactEffect != null)
        {
            Instantiate(impactEffect, transform.position, transform.rotation);
        }

        if (other.CompareTag("Player"))
        {
            HealthManager.instance?.HurtPlayer();
        }

        Destroy(gameObject);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}