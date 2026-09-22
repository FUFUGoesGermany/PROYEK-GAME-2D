using UnityEngine;
using System.Collections;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private BirdController bird;
    [SerializeField] private WorldScroller worldScroller;

    [Header("Countdown")]
    [SerializeField] private int countdownSeconds = 3;

    public enum GameState
    {
        Countdown,
        Playing,
        Dead
    }

    public GameState CurrentState { get; private set; }

    private void Awake()
    {
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
    }

    private void SetState(GameState newState)
    {
        CurrentState = newState;

        switch (newState)
        {
            case GameState.Countdown:
                SetWorldActive(false);
                SetCountdownText(string.Empty);
                break;
            case GameState.Playing:
                SetWorldActive(true);
                SetCountdownText(string.Empty);
                break;
            case GameState.Dead:
                SetWorldActive(false);
                SetCountdownText("GAME OVER");
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
}
