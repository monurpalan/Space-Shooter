using UnityEngine;

public class PowerUp : MonoBehaviour
{

    [Header("Power-Up Types")]
    public bool isShield;
    public bool isBoost;
    public bool isDoubleShot;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ApplyPowerUp();
            Destroy(gameObject);
        }
    }

    private void ApplyPowerUp()
    {
        if (isShield)
        {
            HealthManager.instance?.ActivateShield();
        }

        if (isBoost)
        {
            PlayerController.instance?.ActivateSpeedBoost();
        }

        if (isDoubleShot)
        {
            PlayerController.instance.doubleShotActive = true;
        }
    }
}