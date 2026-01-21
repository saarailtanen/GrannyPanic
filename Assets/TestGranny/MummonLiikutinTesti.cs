using UnityEngine;

public class MummonLiikutinTesti : MonoBehaviour
{
    Rigidbody rb;
    Animator animator;

    bool wButton = false;
    bool aButton = false;
    bool sButton = false;
    bool dButton = false;
    bool bButton = false;
    bool nButton = false;

    [SerializeField] private float moveForce = 30f;
    [SerializeField] private float torqueForce = 0.25f;

    void Start()
    {
        rb = GetComponent<Rigidbody> ();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        wButton = Input.GetKey("w");
        aButton = Input.GetKey("a");
        sButton = Input.GetKey("s");
        dButton = Input.GetKey("d");
        bButton = Input.GetKey("b");
        nButton = Input.GetKey("n");

        if(wButton){
            rb.AddRelativeForce(Vector3.forward * moveForce);
            animator.SetBool("forward", true);
        }
        else{
            animator.SetBool("forward", false);
        }

        if(sButton){
            rb.AddRelativeForce(Vector3.back * moveForce);
            animator.SetBool("backward", true);
        }
        else {
            animator.SetBool("backward", false);
        }

        if(aButton){
            rb.AddTorque(Vector3.up * torqueForce, ForceMode.Impulse);
            animator.SetBool("left", true);
        }
        else{
            animator.SetBool("left", false);
        }

        if(dButton){
            rb.AddTorque(Vector3.up * -torqueForce, ForceMode.Impulse);
            animator.SetBool("right", true);
        }
        else{
            animator.SetBool("right", false);
        }


        if(bButton){
            rb.AddTorque(Vector3.up * torqueForce, ForceMode.Impulse);
            animator.SetBool("backLeft", true);
        }
        else{
            animator.SetBool("backLeft", false);
        }

        if(nButton){
            rb.AddTorque(Vector3.up * -torqueForce, ForceMode.Impulse);
            animator.SetBool("backRight", true);
        }
        else{
            animator.SetBool("backRight", false);
        }
    }
}
