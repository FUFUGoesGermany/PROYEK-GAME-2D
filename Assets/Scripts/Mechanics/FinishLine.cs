using UnityEngine;

public class FinishLine : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<BirdController>() == null) return;
        gameManager.ReachFinish();
    }
}