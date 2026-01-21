using System;
using UnityEngine;

public class WheelchairController : MonoBehaviour
{
    public WheelCollider[] leftWheelColliders;
    public WheelCollider[] rightWheelColliders;
    public Transform[] leftWheelMeshes;
    public Transform[] rightWheelMeshes;
    [SerializeField] private float torque = 0.5f;

    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
/*
    void FixedUpdate()
    {
        float leftInput = 0f;
        float rightInput = 0f;
        float leftAngle = 0f;
        float rightAngle = 0f;

        if (Input.GetKey("w") && Input.GetKey("i")) 
        {
            leftInput = 100f;
            rightInput = 100f;
        }
        if (Input.GetKey("i") && !Input.GetKey("w"))
        {
            leftInput = 100f;
            rightInput = 100f;
            rightAngle = -45f;
        }
        if (!Input.GetKey("i") && Input.GetKey("w"))
        {
            leftInput = 100f;
            rightInput = 100f;
            leftAngle = 45;
        }

        foreach (var wheel in leftWheelColliders)
        {
            wheel.motorTorque = leftInput * torque;
            wheel.steerAngle = rightAngle; // Käännetään vain sisärengasta --> kääntyy paremmin
        }

        foreach (var wheel in rightWheelColliders)
        {
            wheel.motorTorque = rightInput * torque;
            wheel.steerAngle = leftAngle;
        }

        UpdateWheelMeshes(leftWheelColliders, leftWheelMeshes);
        UpdateWheelMeshes(rightWheelColliders, rightWheelMeshes);

        if (Input.GetKey("w") && Input.GetKey("i"))
        {
            animator.SetBool("forward", true);
        }
        else animator.SetBool("forward", false);
        if (!Input.GetKey("w") && Input.GetKey("i"))
        {
            animator.SetBool("right", true);
        }
        else animator.SetBool("right", false);
        if (Input.GetKey("w") && !Input.GetKey("i"))
        {
            animator.SetBool("left", true);
        }
        else animator.SetBool("left", false);
    }*/

      //VANHA TOIMIVA LIIKKUMINEN ILMAN KÄÄNTÖKULMAA
        private void FixedUpdate()
        {
            float leftInput = 0f;
            float rightInput = 0f;

            if (Input.GetKey("w")) leftInput = 100f;
            if (Input.GetKey("s")) leftInput = -100f;
            if (Input.GetKey("i")) rightInput = 100f;
            if (Input.GetKey("k")) rightInput = -100f;

            foreach (var wheel in leftWheelColliders)
            {
                wheel.motorTorque = leftInput * torque;
            }

            foreach (var wheel in rightWheelColliders)
            {
                wheel.motorTorque = rightInput * torque;
            }

            UpdateWheelMeshes(leftWheelColliders, leftWheelMeshes);
            UpdateWheelMeshes(rightWheelColliders, rightWheelMeshes);

            if (Input.GetKey("w") && Input.GetKey("i"))
            {
                animator.SetBool("forward", true);
            }
            else animator.SetBool("forward", false);
            if (!Input.GetKey("w") && Input.GetKey("i"))
            {
                animator.SetBool("right", true);
            }
            else animator.SetBool("right", false);
            if (Input.GetKey("w") && !Input.GetKey("i"))
            {
                animator.SetBool("left", true);
            }
            else animator.SetBool("left", false);
        }
        

    public void UpdateWheelMeshes(WheelCollider[] colliders, Transform[] meshes)
    {
        for (int i = 0; i < colliders.Length; i++)
        {
            Vector3 pos;
            Quaternion rot;
            colliders[i].GetWorldPose(out pos, out rot);
            Quaternion localRot = Quaternion.Inverse(meshes[i].parent.rotation) * rot;
            Vector3 euler = localRot.eulerAngles;
            euler.y = 0f;
            euler.z = 0f;
            meshes[i].rotation = meshes[i].parent.rotation * Quaternion.Euler(euler);
        }
    }

}
