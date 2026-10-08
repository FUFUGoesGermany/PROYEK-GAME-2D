using System;
using UnityEngine;

public class ObstacleCleanup : MonoBehaviour
{
    [SerializeField] private float destroyXPosition = -12f;
    void Update()
    {
        float posX = transform.position.x;

        if (destroyXPosition > posX)
        {
            Destroy(gameObject);    
            Debug.Log("Pipa dihancurkan");
        }
    }
}


