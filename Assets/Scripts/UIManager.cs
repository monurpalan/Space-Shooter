using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("UI Elements")]
    public GameObject gameOverScreen;
    public GameObject levelCompleteScreen;
    public GameObject pauseScreen;

    [Header("Player UI")]
    public Text livesText;
    public Slider healthBar;
    public Slider shieldBar;
    public Text scoreText;

    [Header("Boss UI")]
    public Slider bossSlider;
    public Text bossName;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        GameManager.currentScore = 0;
        SceneManager.LoadScene("Level_1");
    }

    public void Resume()
    {
        GameManager.instance.TogglePause();
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        GameManager.currentScore = 0;
        SceneManager.LoadScene("MainMenu");
    }
}