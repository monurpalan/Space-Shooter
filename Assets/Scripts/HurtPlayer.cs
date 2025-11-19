using UnityEngine;

public class HurtPlayer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            HealthManager.instance?.HurtPlayer();
        }
    }
}