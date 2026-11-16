using UnityEngine;

public enum CollectibleType { Star, Food }

public class Collectible : MonoBehaviour
{
    [SerializeField] private CollectibleType type = CollectibleType.Star;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<BirdController>() == null) return;
        if (CollectibleManager.Instance == null) return;

        if (type == CollectibleType.Star) CollectibleManager.Instance.AddStar();
        else CollectibleManager.Instance.AddFood();

        Destroy(gameObject);
    }
}