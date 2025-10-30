using Unity.Cinemachine;
using UnityEngine;

public class DoorFromBulid : MonoBehaviour
{
    [SerializeField] private KeyCode interactKey = KeyCode.Return;
    [SerializeField] private Transform TeleportPoint;
    [SerializeField] private GameObject Player;

    private bool isPlayerNear = false;

    private void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(interactKey))
        { 
            Player.transform.position = TeleportPoint.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) isPlayerNear = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) isPlayerNear = false;
    }
}
