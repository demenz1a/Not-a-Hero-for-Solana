using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("���������")]
    public int damage = 10;
    public float speed = 8f;
    public float lifeTime = 5f;

    private Rigidbody2D rb;
    private Vector2 moveDirection;

    public void SetDirection(Vector2 direction)
    {
        moveDirection = direction.normalized;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        rb.linearVelocity = moveDirection * speed;

        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }

        if (collision.CompareTag("Player"))
        {
            Debug.Log("KICK!");
            PlayerMovementForBossBattle.instance.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
