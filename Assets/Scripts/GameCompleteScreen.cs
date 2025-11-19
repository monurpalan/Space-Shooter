using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GameCompleteScreen : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float timeBetweenTexts = 1f;

    [Header("UI Elements")]
    [SerializeField] private Text congratulationsText;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text pressKeyText;

    private bool canExit;

    void Start()
    {
        StartCoroutine(ShowTextSequence());
    }

    void Update()
    {
        if (canExit && Input.anyKeyDown)
        {
            SceneManager.LoadScene(0);
        }
    }

    private IEnumerator ShowTextSequence()
    {
        yield return new WaitForSeconds(timeBetweenTexts);
        congratulationsText.gameObject.SetActive(true);

        yield return new WaitForSeconds(timeBetweenTexts);
        scoreText.text = "Final Score: " + GameManager.currentScore.ToString();
        scoreText.gameObject.SetActive(true);

        yield return new WaitForSeconds(timeBetweenTexts);
        pressKeyText.gameObject.SetActive(true);

        canExit = true;
    }
}