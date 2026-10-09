using UnityEngine;

public class TrackingMonitor : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    private KeyCode simulateLostKey = KeyCode.T;

    private bool isLost;

    void Update()
    {
        bool trackingAktif = !Input.GetKey(simulateLostKey);

        if (!trackingAktif && !isLost)
        {
            isLost = true;
            gameManager.PauseGame(GameManager.PauseReason.TrackingLost);
        }
        else if (trackingAktif && isLost)
        {
            isLost = false;
            gameManager.ResumeGame();
        }
    }
}