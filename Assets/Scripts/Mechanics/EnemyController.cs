using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public BirdController bird;
        
    [Header("Movement Parameters")]
    [Tooltip("Kecepatan horizontal musuh")]
    [SerializeField] public float speed = 2f;

    void Start()
    {
                
    }

    void Update()
    {
        MoveEnemy();
    }
    public void MoveEnemy()
    {
        transform.position = new Vector2(transform.position.x - speed * Time.deltaTime, transform.position.y);
    }          
}
