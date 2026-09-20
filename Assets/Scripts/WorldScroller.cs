using UnityEngine;

public class WorldScroller : MonoBehaviour
{
    [SerializeField] private BirdController bird;
    [SerializeField] private float scrollSpeed = 5f;
    public bool isScrolling = true;

    void Start()
    {
        
    }

    void Update()
    {
        ScrollWorld();
    }

    public void ScrollWorld()
    {
        if (!isScrolling == true) return; 

        if (bird == null || bird.IsDead || bird.IsInvincible)
        {
            //  Bird is dead or invincible, stopping world scrolling;
            return;
        }

        transform.Translate(Vector3.left * scrollSpeed * Time.deltaTime);

    }
}
