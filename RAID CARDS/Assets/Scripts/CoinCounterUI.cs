using TMPro;
using UnityEngine;

public class CoinCounterUI : MonoBehaviour
{
    [Header("���������� UI")]
    [SerializeField] private TextMeshProUGUI coinText;   

    private void Start()
    {
        if (RoomManager.instance == null)
        {
            Debug.LogWarning("RoomManager �� ������!");
            return;
        }

        UpdateCoinText();
    }

    private void Update()
    {
        UpdateCoinText();
    }

    private void UpdateCoinText()
    {
        if (RoomManager.instance != null && coinText != null)
        {
            coinText.text = RoomManager.instance.coins.ToString();
        }
    }
}
