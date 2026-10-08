using UnityEngine;

public class WorldScroller : MonoBehaviour
{   
    [SerializeField] private float scrollSpeed = 5f;
    public bool isScrolling = true;
    [SerializeField] private GameManager gameManager;

    void Update()
    {
        ScrollWorld();
    }

    public void ScrollWorld()
    {
        if (gameManager == null || gameManager.CurrentState != GameManager.GameState.Playing)
        {
            return;
        }
        transform.Translate(Vector3.left * scrollSpeed * Time.deltaTime);
    }
}
