using UnityEngine;
using UnityEngine.Splines;

public class TrainCarFollow : MonoBehaviour
{
    public SplineContainer spline;  // Assign your spline in Inspector
    public float speed = 15f;        // Movement speed
    public float spacing = 21f;      // Distance between cars
    [Tooltip("0 = front car, 1 = next car, etc.")]
    public int order = 0;           // Front/back order number

    private float distance;         // Current distance along spline

    private void Start()
    {
        if (spline == null) return;

        // Assign distance based solely on order
        distance = order * spacing;
    }

    private void Update()
    {
        if (spline == null) return;

        // Move along spline
        distance += speed * Time.deltaTime;

        float splineLength = spline.CalculateLength();
        float t = (distance % splineLength) / splineLength;

        transform.position = spline.EvaluatePosition(t);
        transform.rotation = Quaternion.LookRotation(spline.EvaluateTangent(t), Vector3.up);
    }
}
