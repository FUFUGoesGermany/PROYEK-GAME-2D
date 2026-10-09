using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.AI;
using UnityEditor.Rendering;


public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private BirdController bird;
    [SerializeField] private WorldScroller worldScroller;

    [Header("Countdown")]
    [SerializeField] private int countdownSeconds = 3;

    [Header("Game Over UI")]
    [SerializeField] private GameObject gameOverPanel;

    [Header("Pause UI")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private TextMeshProUGUI pauseTitleText;
    private PauseReason pauseReason;

    public enum GameState
    {
        Countdown,
        Playing,
        Dead,
        Paused
    }

    public enum PauseReason
    {
        Manual,
        TrackingLost
    }

    public GameState CurrentState { get; private set; }

    private void Awake()
    {
        Time.timeScale = 1f;
        
        if (bird == null)
        {
            bird = FindObjectOfType<BirdController>();
        }

        if (worldScroller == null)
        {
            worldScroller = FindObjectOfType<WorldScroller>();
        }

        CurrentState = GameState.Countdown;
    }

    private void Start()
    {
        SetState(GameState.Countdown);
        StartCoroutine(CountdownRoutine());
    }

    private void Update()
    {
        if (CurrentState == GameState.Playing && bird != null && bird.IsDead)
        {
            SetState(GameState.Dead);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (CurrentState == GameState.Paused) ResumeGame();
            else PauseGame(PauseReason.Manual);
        }
    }

    private void SetState(GameState newState)
    {
        CurrentState = newState;

        switch (newState)
        {
            case GameState.Countdown:
                SetWorldActive(false);
                SetCountdownText(string.Empty);
                SetGameOverPanel(false);
                SetPausePanel(false);
                break;
            case GameState.Playing:
                SetWorldActive(true);
                SetCountdownText(string.Empty);
                SetGameOverPanel(false);
                SetPausePanel(false);
                break;
            case GameState.Dead:
                SetWorldActive(false);
                SetCountdownText("GAME OVER");
                SetGameOverPanel(true);
                SetPausePanel(false);
                break;
            case GameState.Paused:
                SetWorldActive(false);
                SetPausePanel(true);
                break;

                
        }
    }

    private IEnumerator CountdownRoutine()
    {
        int secondsRemaining = Mathf.Max(0, countdownSeconds);

        while (secondsRemaining > 0)
        {
            SetCountdownText(secondsRemaining.ToString());
            yield return new WaitForSecondsRealtime(1f);
            secondsRemaining = secondsRemaining - 1;
        }

        SetCountdownText("GO!");
        yield return new WaitForSecondsRealtime(0.5f);
        SetState(GameState.Playing);
    }

    private void SetWorldActive(bool isActive)
    {
        if (bird != null)
        {
            bird.SetBirdKinematic(!isActive);
        }

        if (worldScroller != null)
        {
            worldScroller.isScrolling = isActive;
        }
    }

    private void SetCountdownText(string text)
    {
        if (countdownText != null)
        {
            countdownText.text = text;
        }
    }

    private void SetGameOverPanel(bool isActive)
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(isActive);
        }
    }

    private void SetPausePanel(bool isActive)
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(isActive);
        }

        if (isActive && pauseTitleText != null)
        {
            pauseTitleText.text = (pauseReason == PauseReason.TrackingLost)
            ? "Tracking Lost!"
            : "Paused";
        }
    }

    public void RetryGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void PauseGame(PauseReason reason)
    {
        if (CurrentState == GameState.Dead || CurrentState == GameState.Paused) return;

        pauseReason = reason;
        Time.timeScale = 0f;
        SetState(GameState.Paused);
    }

    public void ResumeGame()
    {
        if (CurrentState != GameState.Paused) return;

        Time.timeScale = 1f;
        SetState(GameState.Playing);
    }
}
