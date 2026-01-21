using UnityEngine;

public class SmoothFollowCamera : MonoBehaviour
{
    [Header("mummo Settings")]
    public Transform mummo; 
    public Transform lookAtPoint;
    
    [Header("Camera Position")]
    public Vector3 offset = new Vector3(0f, 1.5f, -3f); 
    
    [Header("Smoothing")]
    public float positionSmoothTime = 0.1f;
    public float rotationSpeed = 5f;
    
    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (mummo == null)
        {
            Debug.LogWarning("Camera mummo is not assigned!");
            return;
        }
  
        Vector3 desiredPosition = mummo.position + Quaternion.Euler(0, mummo.eulerAngles.y, 0) * offset;
        transform.position = Vector3.SmoothDamp(
            transform.position, 
            desiredPosition, 
            ref velocity, 
            positionSmoothTime
        );
        
        Vector3 lookmummo = lookAtPoint != null ? lookAtPoint.position : mummo.position + Vector3.up * 1.5f;
        Vector3 direction = lookmummo - transform.position;
        if (direction != Vector3.zero)
        {
            Quaternion mummoRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, mummoRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
