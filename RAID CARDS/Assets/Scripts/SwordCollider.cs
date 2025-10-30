using UnityEngine;

public class SwordCollider : MonoBehaviour
{
    public AudioSource audioSource;
    [SerializeField] private AudioClip swordClip;
    public int damage = 10;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("KICK!");
            PlayerMovementForBossBattle.instance.TakeDamage(damage);
        }
    }

    private void Start()
    {
        audioSource.playOnAwake = false;
    }

    public void PlaySword()
    {
        audioSource.PlayOneShot(swordClip);
    }
}
