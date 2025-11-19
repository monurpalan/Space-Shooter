using UnityEngine;

public class MusicController : MonoBehaviour
{
    public static MusicController instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource levelMusic;
    [SerializeField] private AudioSource bossMusic;
    [SerializeField] private AudioSource victoryMusic;
    [SerializeField] private AudioSource gameOverMusic;

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
        PlayLevelMusic();
    }

    private void StopAllMusic()
    {
        levelMusic.Stop();
        bossMusic.Stop();
        victoryMusic.Stop();
        gameOverMusic.Stop();
    }

    public void PlayLevelMusic()
    {
        StopAllMusic();
        levelMusic.Play();
    }

    public void PlayBossMusic()
    {
        StopAllMusic();
        bossMusic.Play();
    }

    public void PlayVictoryMusic()
    {
        StopAllMusic();
        victoryMusic.Play();
    }

    public void PlayGameOverMusic()
    {
        StopAllMusic();
        gameOverMusic.Play();
    }
}