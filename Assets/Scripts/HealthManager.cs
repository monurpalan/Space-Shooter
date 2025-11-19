using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public static HealthManager instance;

    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private int currentHealth;
    [SerializeField] private GameObject deathEffect;

    [Header("Invincibility Settings")]
    [SerializeField] private float invincibleLength = 2f;
    private float invincCounter;
    [SerializeField] private SpriteRenderer theSr;

    [Header("Shield Settings")]
    public int shieldPower = 0;
    public int shieldMaxPower = 2;
    [SerializeField] private GameObject theShield;

    void Awake()
    {
        instance = this;

        UIManager.instance.shieldBar.maxValue = shieldMaxPower;
        UIManager.instance.healthBar.maxValue = maxHealth;
        UIManager.instance.healthBar.value = currentHealth;
    }

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        HandleInvincibility();
    }

    private void HandleInvincibility()
    {
        if (invincCounter > 0)
        {
            invincCounter -= Time.deltaTime;
            if (invincCounter <= 0)
            {
                theSr.color = new Color(theSr.color.r, theSr.color.g, theSr.color.b, 1f);
            }
        }
    }

    public void HurtPlayer()
    {
        if (invincCounter > 0) return;

        if (theShield.activeInHierarchy)
        {
            HandleShieldDamage();
        }
        else
        {
            HandleHealthDamage();
        }
    }

    private void HandleShieldDamage()
    {
        shieldPower--;
        UIManager.instance.shieldBar.value = shieldPower;

        if (shieldPower <= 0)
        {
            theShield.SetActive(false);
        }
    }

    private void HandleHealthDamage()
    {
        currentHealth--;
        UIManager.instance.healthBar.value = currentHealth;
        PlayerController.instance.doubleShotActive = false;

        if (currentHealth <= 0)
        {
            Instantiate(deathEffect, transform.position, transform.rotation);
            gameObject.SetActive(false);
            GameManager.instance.KillPlayer();
            WavesManager.instance.canSpawnWaves = false;
        }
    }

    public void Respawn()
    {
        gameObject.SetActive(true);
        currentHealth = maxHealth;
        UIManager.instance.healthBar.value = maxHealth;

        invincCounter = invincibleLength;
        theSr.color = new Color(theSr.color.r, theSr.color.g, theSr.color.b, 0.5f);

        WavesManager.instance.canSpawnWaves = true;
    }

    public void ActivateShield()
    {
        theShield.SetActive(true);
        shieldPower = shieldMaxPower;
        UIManager.instance.shieldBar.value = shieldPower;
    }
}