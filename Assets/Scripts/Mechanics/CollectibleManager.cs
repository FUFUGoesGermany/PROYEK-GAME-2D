using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
    public static CollectibleManager Instance;

    private int starCount;
    private int foodTotal;

    void Awake()
    {
        Instance = this;
        starCount = 0;
        foodTotal = PlayerPrefs.GetInt("food", 0);
    }

    public int StarCount => starCount;
    public int FoodTotal => foodTotal;

    public void AddStar()
    {
        starCount++;
        Debug.Log($"[Collectible] Bintang: {starCount}/3");
    }

    public void AddFood()
    {
        foodTotal++;
        PlayerPrefs.SetInt("food", foodTotal);
        PlayerPrefs.Save();
        Debug.Log($"[Collectible] Makanan: {foodTotal}");
    }
}