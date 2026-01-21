using UnityEngine;
using UnityEngine.Splines;

public class TrainFollowSpline : MonoBehaviour
{
    public SplineContainer spline;
    public float speed = 5f;
    private float distance;

    private void Update()
    {
        //tarkistaa onko spline ettei tule virhettä
        if (spline == null) return;
        //liikku splinessa kiinni ajan ja nopeuden mukaan
        distance += speed * Time.deltaTime;

        //lasketaan radan pituus
        float splineLength = spline.CalculateLength();
        //% aloittaa alusta kun spline loppuu
        float t = distance % splineLength / splineLength;

        //koordinaatistoa
        var splineEval = spline.EvaluatePosition(t);
        var tangent = spline.EvaluateTangent(t);

        //siirtää junan splinen pisteeseen radalla
        transform.position = splineEval;
        //kääntää junaa splinen mukaan
        transform.rotation = Quaternion.LookRotation(tangent);
    }
}
