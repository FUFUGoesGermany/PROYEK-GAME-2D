using UnityEngine;

public class HeartsHUD : MonoBehaviour
{
    [SerializeField] private BirdController bird;
    [SerializeField] private GameObject[] heartIcons;

    void Update()
    {
        if (bird == null || heartIcons == null || heartIcons.Length == 0) return;
        for (int i = 0; i < heartIcons.Length; i++)
        {
            heartIcons[i].SetActive(i < bird.CurrentHearts);
        }
    }
        
}   