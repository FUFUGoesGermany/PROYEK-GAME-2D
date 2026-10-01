using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public BirdController bird;
    public enum EnemyPattern
    {
        Straight,
        SineWave,
    }   

    [SerializeField] public EnemyPattern Straightpattern = EnemyPattern.Straight;
    [SerializeField] public EnemyPattern SineWavepattern = EnemyPattern.SineWave;

    [Header("Movement Parameters")]
    [Tooltip("Kecepatan horizontal musuh")]
    [SerializeField] public float speed = 2f;
    [SerializeField] public float amplitude = 2f;
    [SerializeField] public float frequency = 3f;
    private float baseY;
    private float timeAlive;

    void Start()
    {
        baseY = transform.position.y;        
    }

    void Update()
    {
        if (bird == null || bird.IsDead || bird.IsInvincible)
        {
            return;
        }

        // timer maju
        timeAlive += Time.deltaTime;
        
        // fungsi bergerak musuh ke kiri
        if (Straightpattern == EnemyPattern.Straight || SineWavepattern == EnemyPattern.SineWave)
        {
            MoveEnemy();
        }
        // fungsi gerak musuh dengan pola SineWave
        else if (SineWavepattern == EnemyPattern.SineWave)
        {
            float offsetY = Mathf.Sin(timeAlive * frequency) * amplitude;

            Vector3 pos = transform.position;
            pos.y = baseY + offsetY;
            transform.position = pos;
        }
    }
    public void MoveEnemy()
    {
        transform.position = new Vector2(transform.position.x - speed * Time.deltaTime, transform.position.y);

    }          
}
