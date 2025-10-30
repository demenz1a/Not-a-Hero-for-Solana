using UnityEngine;

public class Enter2D : MonoBehaviour
{
    public float bounceForce = 30f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.IsChildOf(transform))
            return;

        if (collision.CompareTag("SpinEnd"))
        {
            BossController.instance.stopSpin = true;
        }

        if (collision.CompareTag("Coin"))
        {
            //BossController.instance.TakeDamage(1);
        }

        if (collision.CompareTag("Player"))
        {
            BossController.instance.TakeDamage(3);

            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);

                rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
            }
        }
    }
}
