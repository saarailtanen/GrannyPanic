using System;
using ithappy.Animals_FREE;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class cat : MonoBehaviour
{
    //referoidaan spline, haluttu nopeus ja rangen avulla saadaan määritettyä
    //mistä kohtaa splineä aloitetaan
    [SerializeField] private SplineContainer spline;
    [SerializeField] private float speed = 5f;
    [Range(0f, 1f)] [SerializeField] private float startT = 0f;

    private float distance;
    //refedoifaan catMover skriptiin jossa animaatio koodattu
    private catMover creatureMover;

    private void Start()
    {
        //otetaan animaatio skripti käyttöön
        creatureMover = GetComponent<catMover>();
        if (spline == null) return;

        //splinen pituus
        float splineLength = spline.CalculateLength();
        distance = startT * splineLength;

        //ottaa position ja suunnan ja laittaa kissan haluttuun paikkaan
        Vector3 pos = spline.EvaluatePosition(startT);
        Vector3 tangent = spline.EvaluateTangent(startT);
        transform.position = pos;
        //lookrotation saa kissan sinne suuntaan mihin spline menee
        transform.rotation = Quaternion.LookRotation(tangent);
    }

    private void Update()
    {
        if (spline == null) return;

        distance += speed * Time.deltaTime;

        float splineLength = spline.CalculateLength();
        //% avulla kissa saadaan loopattua splineen
        float t = (distance % splineLength) / splineLength;
        

        //tässä käytetään unity.mathematics
        //jotta saadaan 3d positio ja tangentti (direction of movement)
        float3 posF3 = spline.EvaluatePosition(t);
        float3 tangentF3 = spline.EvaluateTangent(t);

        //kääntää float3 vector3 jotta unity animator voi käyttää arvoja
        Vector3 pos = new Vector3(posF3.x, posF3.y, posF3.z);
        Vector3 tangent = new Vector3(tangentF3.x, tangentF3.y, tangentF3.z);

        //liikuttaa kissaa
        transform.position = pos;
        transform.rotation = Quaternion.LookRotation(tangent);

        //luo 2d movement axis animaattorille jotta tietää liikkuvuuden suunnan
        Vector3 forward = transform.forward;
        Vector3 moveDir = tangent.normalized;
        Vector2 axis = new Vector2(Vector3.Dot(transform.right, moveDir), Vector3.Dot(transform.forward, moveDir));

        //aktivoi kävely animaation kissalle (false ja false eli ei juokse eikä hypi
        //jos laittaa true ja false niin juoksee
        creatureMover.SetInput(axis, pos + forward, true, false);

    }
}

