using UnityEngine;

public class PlaySound : MonoBehaviour
{
    public AudioSource audioSource;
    [SerializeField] private AudioClip swordClip;

    private void Start()
    {
        audioSource.playOnAwake = false;
    }

    public void PlaySword()
    {
        audioSource.PlayOneShot(swordClip);
    }
}