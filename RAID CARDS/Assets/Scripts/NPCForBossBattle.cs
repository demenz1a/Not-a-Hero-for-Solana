using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class BossController : MonoBehaviour
{
    public static BossController instance;

    public float health = 100f;
    public GameObject perehodPrefab;
    public float currentHealth;


    [Header("Настройки")]
    public float moveSpeed = 10f;
    public float attackCooldown = 3f;
    [SerializeField] private float projectileSpeed = 8f;

    [Header("Ссылки")]
    public Transform player;
    public Transform projectileSpawnPoint;
    public GameObject projectilePrefab;
    public GameObject swordPrefab;
    public Animator animator;
    [SerializeField] private Transform sword;

    private Rigidbody2D rb;
    private bool isAttacking = false;
    private int currentAttack = 0;
    private bool isFacingRight = true;

    public bool stopSpin = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        StartCoroutine(BossRoutine());
        currentHealth = health;
    }

    private void Update()
    {
        ItIsDead();
    }

    private IEnumerator BossRoutine()
    {
        while (true)
        {
            if (!isAttacking)
            {
                isAttacking = true;

                if (currentAttack == 0)
                    yield return StartCoroutine(Attack1_Projectile());
                else
                    yield return StartCoroutine(Attack2_SpinMove());

                currentAttack = (currentAttack + 1) % 2; 
                yield return new WaitForSeconds(attackCooldown);
                isAttacking = false;
            }

            yield return null;
        }
    }

    private IEnumerator Attack1_Projectile()
    {
        FacePlayer();

        animator.SetTrigger("Attack1");
        yield return new WaitForSeconds(0.5f); 

        if (projectilePrefab != null && projectileSpawnPoint != null && player != null)
        {
            Vector2 direction = (player.position - projectileSpawnPoint.position).normalized;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            sword.rotation = Quaternion.Euler(0, 0, angle);

            GameObject proj = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);

            Projectile bp = proj.GetComponent<Projectile>();
            if (bp != null)
                bp.SetDirection(direction);

            animator.SetTrigger("Idle");
        }

        yield return new WaitForSeconds(2f);
        sword.localRotation = Quaternion.identity;
    }

    private IEnumerator Attack2_SpinMove()
    {
        animator.SetTrigger("Attack2");
        stopSpin = false;

        while (!stopSpin)
        {
            float dir = isFacingRight ? 1f : -1f;
            rb.linearVelocity = new Vector2(dir * moveSpeed, rb.linearVelocity.y);
            yield return null;
        }

        rb.linearVelocity = Vector2.zero;
        animator.SetTrigger("Idle");

        Flip();

        yield return new WaitForSeconds(0.5f);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1f;
        transform.localScale = scale;
    }

    private void FacePlayer()
    {
        if (player == null) return;

        if (player.position.x > transform.position.x && !isFacingRight)
            Flip();

        else if (player.position.x < transform.position.x && isFacingRight)
            Flip();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
    }

    private void ItIsDead()
    {
        if (currentHealth < 0)
        {
            Instantiate(perehodPrefab);
            animator.SetBool("Death", true);
            swordPrefab.SetActive(false);
        }
    }
}