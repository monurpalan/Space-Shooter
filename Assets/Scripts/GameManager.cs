using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Player Settings")]
    public int currentLives = 3;
    [SerializeField] private float respawnTime = 2f;

    [Header("Game Settings")]
    public static int currentScore;
    public bool levelEnding;
    private bool canPause;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        canPause = true;
        UpdateUI();
    }

    void Update()
    {
        if (levelEnding)
        {
            MovePlayerToEnd();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && canPause)
        {
            TogglePause();
        }
    }

    public void KillPlayer()
    {
        currentLives--;
        UIManager.instance.livesText.text = "X " + currentLives;

        if (currentLives > 0)
        {
            StartCoroutine(RespawnCo());
        }
        else
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        UIManager.instance.gameOverScreen.SetActive(true);
        WavesManager.instance.canSpawnWaves = false;
        MusicController.instance.PlayGameOverMusic();
        canPause = false;
    }

    public IEnumerator RespawnCo()
    {
        yield return new WaitForSeconds(respawnTime);
        HealthManager.instance.Respawn();
        WavesManager.instance.ContinueSpawning();
    }

    public void AddScore(int scoreToAdd)
    {
        currentScore += scoreToAdd;
        UIManager.instance.scoreText.text = "Score: " + currentScore;
    }

    public IEnumerator EndLevelCo()
    {
        canPause = false;
        MusicController.instance.PlayVictoryMusic();
        levelEnding = true;
        PlayerController.instance.stopMovement = true;
        UIManager.instance.levelCompleteScreen.SetActive(true);

        yield return new WaitForSeconds(5f);

        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogWarning("Bir sonraki sahne mevcut değil.");
        }
    }

    public void TogglePause()
    {
        if (UIManager.instance.pauseScreen.activeInHierarchy)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    private void PauseGame()
    {
        UIManager.instance.pauseScreen.SetActive(true);
        Time.timeScale = 0f;
        PlayerController.instance.stopMovement = true;
    }

    private void ResumeGame()
    {
        UIManager.instance.pauseScreen.SetActive(false);
        Time.timeScale = 1f;
        PlayerController.instance.stopMovement = false;
    }

    private void MovePlayerToEnd()
    {
        PlayerController.instance.transform.position += new Vector3(PlayerController.instance.boostSpeed * Time.deltaTime, 0f, 0f);
    }

    private void UpdateUI()
    {
        UIManager.instance.livesText.text = "X " + currentLives;
        UIManager.instance.scoreText.text = "Score: " + currentScore;
    }
}