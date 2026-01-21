using UnityEngine;

public class RagdollController : MonoBehaviour
{
    [SerializeField] Animator animator;
    Rigidbody[] ragdollBodies;
    Collider[] ragdollColliders;

    void Start()
    {
        ragdollBodies = GetComponentsInChildren<Rigidbody>();
        ragdollColliders = GetComponentsInChildren<Collider>();
        SetRagdollActive(false); // start disabled
    }

    void Update()
    {

    }

    public void SetRagdollActive(bool active)
    {
        foreach (var body in ragdollBodies)
        {
            if (body.CompareTag("Ragdoll"))
                body.isKinematic = !active;
        }

        foreach (var col in ragdollColliders)
        {
            if (col.CompareTag("Ragdoll"))
                col.enabled = active;
        }

        animator.enabled = !active;
    }

    void OnCollisionEnter(Collision collision)
{
    if (collision.gameObject.CompareTag("Juna") || collision.gameObject.CompareTag("Junavaunu"))
    {
        SetRagdollActive(true);
    }
}

}
