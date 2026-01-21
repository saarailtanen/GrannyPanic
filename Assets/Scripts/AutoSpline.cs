using UnityEngine;
using UnityEngine.Splines;

public class AutoSpline : MonoBehaviour
{
    public SplineContainer spline;
    public float speed = 3f;

    [Range(0f, 1f)]
    public float startPercent = 0f; // 0 = start of spline, 1 = end

    private float distance;

    private void Start()
    {
        if (spline == null) return;

        float splineLength = spline.CalculateLength();
        distance = startPercent * splineLength; // set starting position
    }

    private void Update()
    {
        if (spline == null) return;

        float splineLength = spline.CalculateLength();

        // Move forward along the spline
        distance += speed * Time.deltaTime;

        // Loop back when reaching the end
        float t = (distance % splineLength) / splineLength;

        Vector3 pos = spline.EvaluatePosition(t);
        Vector3 tan = spline.EvaluateTangent(t);

        transform.position = pos;
        transform.rotation = Quaternion.LookRotation(tan);
    }
}
