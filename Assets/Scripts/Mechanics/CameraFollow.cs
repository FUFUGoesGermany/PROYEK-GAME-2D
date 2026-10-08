using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Camera cam;

    [Header("Camera Boundaries")]
    [SerializeField] private float minY = -4.5f;
    [SerializeField] private float maxY = 4.5f;
    [SerializeField] private Transform target;
    [SerializeField] private float smoothSpeed = 5;


    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (target == null) return;
        
        float ortho = cam.orthographicSize;

        float yMin = minY + ortho;
        float yMax = maxY - ortho;

        float yTujuan = Mathf.Clamp(target.position.y, yMin, yMax);

        Vector3 targetPosition = new Vector3(transform.position.x, yTujuan, transform.position
.z);    
        float t = smoothSpeed * Time.deltaTime;
        transform.position = Vector3.Lerp(transform.position, targetPosition, t);
    }
}