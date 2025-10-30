using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovementForBossBattle : MonoBehaviour
{
    public static PlayerMovementForBossBattle instance;

    private float horizontal;
    public int health = 100;
    public int currentHealth;
    [SerializeField] private float speed = 8f;
    [SerializeField] private float jumpingPower = 25f;
    private bool isFacingRight = true;

    [Header("Компоненты")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private string SceneName;

    [Header("Монеты и стрельба")]
    public GameObject coinPrefab;
    public float shootForce = 15f;
    public Transform firePoint;

    [Header("Перезарядка стрельбы")]
    [SerializeField] private float shootCooldown = 1.5f;
    private bool canShoot = true;
    private float cooldownTimer = 0f;

    [Header("Аниматоры по типу")]
    [SerializeField] private Animator animator;
    [SerializeField] private RuntimeAnimatorController medicalController;
    [SerializeField] private RuntimeAnimatorController weaponController;
    [SerializeField] private RuntimeAnimatorController magicController;

    private Camera cam;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        cam = Camera.main;
        currentHealth = health;

        if (RoomManager.instance != null)
        {
            switch (RoomManager.instance.playerSkinType)
            {
                case NeedType.Medical:
                    animator.runtimeAnimatorController = medicalController;
                    break;
                case NeedType.Weapon:
                    animator.runtimeAnimatorController = weaponController;
                    break;
                case NeedType.Magic:
                    animator.runtimeAnimatorController = magicController;
                    break;
                default:
                    Debug.LogWarning("Неизвестный тип скина у игрока!");
                    break;
            }
        }
    }

    private void Update()
    {
        HandleShootingCooldown();

        if (Input.GetMouseButtonDown(0))
        {
            TryShootCoin();
        }

        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
        }

        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }

        ItIsDead();
        Flip();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private void TryShootCoin()
    {
        if (!canShoot)
        {
            Debug.Log("⏳ Перезарядка... Подожди немного!");
            return;
        }

        if (!RoomManager.instance.SpendCoin())
        {
            Debug.Log("Нет монет!");
            return;
        }

        ShootCoin();
        canShoot = false;
        cooldownTimer = shootCooldown;
    }

    private void ShootCoin()
    {
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - firePoint.position).normalized;

        GameObject coin = Instantiate(coinPrefab, firePoint.position, Quaternion.identity);

        CoinProjectile proj = coin.GetComponent<CoinProjectile>();
        if (proj != null)
            proj.Initialize(direction);

        Debug.Log("💰 Выстрел монетой!");
    }

    private void HandleShootingCooldown()
    {
        if (!canShoot)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                canShoot = true;
                Debug.Log("✅ Перезарядка завершена!");
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
    }

    private void ItIsDead()
    {
        if (currentHealth < 0)
        {
            SceneManager.LoadScene(SceneName);
        }
    }
}

