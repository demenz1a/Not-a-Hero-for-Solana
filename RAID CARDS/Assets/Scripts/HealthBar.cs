using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image bar;
    [SerializeField] private PlayerMovementForBossBattle player;
    [SerializeField] private BossController boss;

    private void Update()
    {
        if (player != null)
        {
            bar.fillAmount = (float)player.currentHealth / player.health;
        }

        else
        {
            bar.fillAmount = (float)boss.currentHealth / boss.health;
        }
    }
}