using UnityEngine;

public class FallingObject : MonoBehaviour
{
    [SerializeField] private float fallSpeed = 3.5f;
    [SerializeField] private float destroyY = -6f; // batas bawah layar

    private void Update()
    {  
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);            

        if (transform.position.y < destroyY) 
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            BirdController birdController = other.GetComponent<BirdController>();
            if (birdController != null)
            {
                birdController.TakeDamage(); // Mengurangi health burung
                Debug.Log($"[FallingObject] Mengenai burung: {other.gameObject.name}");
            }

            Destroy(gameObject); // Hancurkan objek jatuh setelah menabrak burung
        }
    }
}