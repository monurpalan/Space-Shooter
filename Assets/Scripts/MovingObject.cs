using UnityEngine;

public class MovingObject : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    void Update()
    {
        MoveObject();
    }

    private void MoveObject()
    {
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}