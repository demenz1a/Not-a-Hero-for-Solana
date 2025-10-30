using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AnimatorScript : MonoBehaviour
{
    [SerializeField] private Animator animator;   
    [SerializeField] private float moveThreshold = 0.05f; 

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (animator == null) return;

        bool isMoving = rb.linearVelocity.magnitude > moveThreshold;
        animator.SetBool("Moving", isMoving);
    }
}

