using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [Header("Background Settings")]
    [SerializeField] private Transform background1;
    [SerializeField] private Transform background2;
    [SerializeField] private float scrollSpeed = 5f;

    private float backgroundWidth;

    void Start()
    {
        InitializeBackgroundWidth();
    }

    void Update()
    {
        ScrollBackground(background1);
        ScrollBackground(background2);
    }

    private void InitializeBackgroundWidth()
    {
        if (background1 != null && background1.TryGetComponent(out SpriteRenderer spriteRenderer))
        {
            backgroundWidth = spriteRenderer.sprite.bounds.size.x;
        }
        else
        {
            Debug.LogError("Background1 veya SpriteRenderer eksik.");
        }
    }

    private void ScrollBackground(Transform background)
    {
        if (background == null) return;

        background.position -= Vector3.right * scrollSpeed * Time.deltaTime;

        if (background.position.x < -backgroundWidth)
        {
            background.position += Vector3.right * backgroundWidth * 2f;
        }
    }
}