using UnityEngine;

public class FallingSpawner : MonoBehaviour
{
    [SerializeField] private float minSpawnInterval = 15f;
    [SerializeField] private float maxSpawnInterval = 30f;
    [SerializeField] private float spawnXMin = 5f;
    [SerializeField] private float spawnXMax = 10f;
    [SerializeField] private float spawnY = 6f;
    [SerializeField] private GameObject fallingObjectPrefab;
    [SerializeField] private Transform worldParent;
    [SerializeField] private GameManager gameManager;
    
    private float timer;
    private float nextSpawnTime;

    void Start()
    {
        nextSpawnTime = Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    void Update()
    {
        if (gameManager != null && gameManager.CurrentState != GameManager.GameState.Playing)
        {
            return; // Hentikan spawn jika game tidak dalam keadaan bermain
        }

        timer += Time.deltaTime;

        if (timer >= nextSpawnTime)
        {
            SpawnFallingObject();
            timer = 0f;
            nextSpawnTime = Random.Range(minSpawnInterval, maxSpawnInterval);
        }
    }

    private void SpawnFallingObject()
    {
        float randomX = Random.Range(spawnXMin, spawnXMax);
        Vector3 spawnPosition = new Vector3(randomX, spawnY, 0f);
        Instantiate(fallingObjectPrefab, spawnPosition, Quaternion.identity, worldParent);
        Debug.Log("[Spawner] Barang jatuh telah muncul!");
    }

}