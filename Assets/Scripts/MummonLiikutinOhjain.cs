using System;
using UnityEngine;

public class MummonLiikutinOhjain : MonoBehaviour
{
    public WheelCollider[] leftWheelColliders;
    public WheelCollider[] rightWheelColliders;
    public Transform[] leftWheelMeshes;
    public Transform[] rightWheelMeshes;
    [SerializeField] private float torque = 0.5f;

    // kutsutaan espdatat luokan oliota
    public EspOhjain espOhjain;
    public EspOhjainVasen espOhjainVasen;


    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();

    }

private void FixedUpdate()
{
    // Get wheel values from your ESP data classes
    float leftInput = espOhjainVasen.espData1.vasenAnturi1;
    float rightInput = espOhjain.espData.oikeaAnturi1;

    // 0 = backwards, 1 = forwards
    int vipu = espOhjain.espData.kytkinE1;

    // Determine the direction multiplier
    int direction = (vipu == 1) ? 1 : -1;

    float leftTorque = leftInput * torque * direction;
    float rightTorque = rightInput * torque * direction;

    foreach (var wheel in leftWheelColliders)
    {
        wheel.motorTorque = leftTorque;
    }
    foreach (var wheel in rightWheelColliders)
    {
        wheel.motorTorque = rightTorque;
    }

    UpdateWheelMeshes(leftWheelColliders, leftWheelMeshes);
    UpdateWheelMeshes(rightWheelColliders, rightWheelMeshes);

    animator.SetBool("forward", direction == 1 && leftInput > 0 && rightInput > 0);
    animator.SetBool("right", direction == 1 && leftInput < rightInput);
    animator.SetBool("left", direction == 1 && leftInput > rightInput);
    animator.SetBool("backward", direction == -1 && leftInput > 0 && rightInput > 0);
    // Add more animator states as needed
}


    public void UpdateWheelMeshes(WheelCollider[] colliders, Transform[] meshes)
    {
        for (int i = 0; i < colliders.Length; i++)
        {
            Vector3 pos;
            Quaternion rot;
            colliders[i].GetWorldPose(out pos, out rot);
            meshes[i].rotation = rot;
        }
    }

    public float CalculateDistance(Transform goal)
    {
        return Vector3.Distance(transform.position, goal.position);
    }
}
