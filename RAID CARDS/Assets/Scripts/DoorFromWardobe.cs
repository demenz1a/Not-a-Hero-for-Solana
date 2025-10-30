using Solana.Unity.Soar.Accounts;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorFromWardobe : MonoBehaviour
{
    [SerializeField] private KeyCode interactKey = KeyCode.Return;
    [SerializeField] private Transform TeleportPoint;
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject WardobeUI;
    [SerializeField] private CinemachineCamera cameraA;
    [SerializeField] private CinemachineCamera cameraB;

    private bool isCameraAActive = true;

    private bool isPlayerNear = false;

    private void Update()
    {
        if (isPlayerNear)
        {
            WardobeUI.SetActive(false);
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
