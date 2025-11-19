using UnityEngine;

public class EnemyWave : MonoBehaviour
{
    void Start()
    {
        transform.DetachChildren();

        Destroy(gameObject);
    }
}