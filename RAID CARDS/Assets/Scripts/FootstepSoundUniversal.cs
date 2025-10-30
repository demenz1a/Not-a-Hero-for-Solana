using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]
public class FootstepSoundUniversal : MonoBehaviour
{
    [SerializeField] private float moveThreshold = 0.1f; 
    [SerializeField] private AudioSource footstepAudio;  
    [SerializeField] private LayerMask groundLayer;      
    [SerializeField] private Transform groundCheck;    

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (footstepAudio == null)
            footstepAudio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        bool isGrounded = IsGrounded();
        bool isMoving = rb.linearVelocity.magnitude > moveThreshold; 

        if (isGrounded && isMoving)
        {
            if (!footstepAudio.isPlaying)
                footstepAudio.Play();
        }
        else
        {
            if (footstepAudio.isPlaying)
                footstepAudio.Stop();
        }
    }

    private bool IsGrounded()
    {
        if (groundCheck == null) return true; 
        return Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
    }
}

