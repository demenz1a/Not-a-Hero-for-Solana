using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CoinProjectile : MonoBehaviour
{
    public int damage = 10;
    public float speed = 15f;
    public float lifeTime = 3f;

    private Vector2 moveDirection;
    private Rigidbody2D rb;

    public void Initialize(Vector2 direction)
    {
        moveDirection = direction.normalized;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.linearVelocity = moveDirection * speed;

        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            BossController.instance.TakeDamage(1);
            Debug.Log("Враг получил урон от монеты!");
            Destroy(gameObject);
        }

        if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}


