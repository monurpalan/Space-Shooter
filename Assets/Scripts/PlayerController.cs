using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private Rigidbody2D playerRB;
    [SerializeField] private float padding = 1f;

    [Header("Shooting Settings")]
    [SerializeField] private Transform shotPoint;
    [SerializeField] private GameObject shot;
    [SerializeField] private float timeBetweenShots;
    [SerializeField] private float doubleShotOffset;
    public bool doubleShotActive;

    [Header("Boost Settings")]
    [SerializeField] public float boostSpeed;
    [SerializeField] private float boostLength;
    private float boostCounter;
    private float normalSpeed;

    [Header("Bounds Settings")]
    private Camera mainCamera;
    private Vector2 minBounds;
    private Vector2 maxBounds;

    [Header("Player State")]
    public bool stopMovement;

    private float shootCounter;
    private bool isShooting;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        normalSpeed = moveSpeed;
        mainCamera = Camera.main;
        UpdateBounds();
    }

    private void Update()
    {
        Debug.Log($"stopMovement: {stopMovement}");

        if (!stopMovement)
        {
            HandleMovement();
            HandleShooting();
            HandleBoost();
        }
        else
        {
            playerRB.velocity = Vector2.zero;
        }
    }

    private void HandleMovement()
    {
        Vector2 keyboardInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        Vector2 joystickInput = Joystick.instance != null ? Joystick.instance.InputVector : Vector2.zero;

        Vector2 input = keyboardInput + joystickInput;
        input = input.magnitude > 1 ? input.normalized : input;
        Debug.Log($"Input: {input}");

        playerRB.velocity = input * moveSpeed;

        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minBounds.x + padding, maxBounds.x - padding);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minBounds.y + padding, maxBounds.y - padding);
        transform.position = clampedPosition;
    }
    private void HandleShooting()
    {
        if (stopMovement) return;
        // Klavye ile ateş etme
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Shoot button pressed!");
            Shoot();
            shootCounter = timeBetweenShots;
        }

        if (Input.GetKey(KeyCode.Space) || isShooting)
        {
            shootCounter -= Time.deltaTime;
            if (shootCounter <= 0)
            {
                Debug.Log("Shooting continuously...");
                Shoot();
                shootCounter = timeBetweenShots;
            }
        }
    }

    public void Shoot()
    {
        if (stopMovement) return;
        if (!isShooting) return;

        if (!doubleShotActive)
        {
            Instantiate(shot, shotPoint.position, shotPoint.rotation);
        }
        else
        {
            Instantiate(shot, shotPoint.position + new Vector3(0f, doubleShotOffset, 0f), shotPoint.rotation);
            Instantiate(shot, shotPoint.position - new Vector3(0f, doubleShotOffset, 0f), shotPoint.rotation);
        }
    }

    private void HandleBoost()
    {
        if (boostCounter > 0)
        {
            boostCounter -= Time.deltaTime;
            if (boostCounter <= 0)
            {
                moveSpeed = normalSpeed;
            }
        }
    }

    private void UpdateBounds()
    {
        minBounds = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0));
        maxBounds = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, 0));
    }

    public void ActivateSpeedBoost()
    {
        boostCounter = boostLength;
        moveSpeed = boostSpeed;
    }


    public void StartShooting()
    {
        if (!stopMovement)
        {
            isShooting = true;
        }
    }

    public void StopShooting()
    {
        isShooting = false;
    }
}