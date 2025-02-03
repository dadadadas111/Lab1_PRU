using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float rollSpeed = 5f;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

    public GameOverMenu gameOverMenu;

    private Rigidbody2D rb;
    private Animator animator;
    private HealthManager healthManager;
    private bool isGrounded = false;
    public bool isRolling = false;
    private float xVelocity;

    // Attack variables
    private bool isAttacking = false;
    private int attackCombo = 0;
    private float comboResetTime = 1f;
    private float lastAttackTime = 0f;
    private float lastImageXPos;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        healthManager = GetComponent<HealthManager>();
    }

    void Update()
    {
        if (healthManager.currentHealth <= 0)
        {
            healthManager.Die();
            //animator.Play("Die", 0, 0f);
            return;
        }

        // --- Horizontal Movement ---
        // Only allow movement if NOT attacking
        if (!isAttacking && !isRolling)
        {
            xVelocity = Input.GetAxis("Horizontal") * moveSpeed;
        }
        else if (isRolling)
        {
            xVelocity = transform.localScale.x * rollSpeed;
            if (Mathf.Abs(transform.position.x - lastImageXPos) > 0.5f)
            {
                PlayerAfterImagePool.Instance.GetFromPool();
                lastImageXPos = transform.position.x;
            }
        }
        else
        {
            xVelocity = 0f; // Freeze movement during attack
        }

        // --- Flip Sprite ---
        // Only flip if NOT attacking
        if (!isAttacking)
        {
            if (xVelocity > 0.1f)
                transform.localScale = new Vector3(1, 1, 1);
            else if (xVelocity < -0.1f)
                transform.localScale = new Vector3(-1, 1, 1);
        }

        // --- Jumping ---
        if (Input.GetKeyDown(KeyCode.K) && isGrounded && !isAttacking && !isRolling) // Disable jump during attack
        {
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Objects"), true);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }

        // --- Rolling ---
        if (Input.GetKeyDown(KeyCode.L) && isGrounded && !isRolling && !isAttacking) // Press 'L' to roll
        {
            Roll();
        }

        // --- Attacking ---
        if (Input.GetKeyDown(KeyCode.J) && !isAttacking && isGrounded)
        {
            Attack();
        }
    }

    private void FixedUpdate()
    {
        if (healthManager.currentHealth <= 0)
        {
            return;
        }

        // Freeze X velocity during attack
        if (!isAttacking)
        {
            rb.velocity = new Vector2(xVelocity, rb.velocity.y);
        }
        else
        {
            // move forward during attack
            rb.velocity = new Vector2(transform.localScale.x * 1f, rb.velocity.y);
        }

        animator.SetFloat("xVelocity", Mathf.Abs(xVelocity));
        animator.SetFloat("yVelocity", rb.velocity.y);
        animator.SetBool("isJumping", !isGrounded);
        animator.SetBool("isFalling", !isGrounded && rb.velocity.y < 0.1);

        if (isGrounded && !isRolling)
        {
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Objects"), false);
        }

        // Combo reset logic
        var timePassedFromLastAttack = Time.time - lastAttackTime;
        if (timePassedFromLastAttack > comboResetTime)
        {
            attackCombo = 0;
            animator.SetBool("CanCombo", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (rb.velocity.y <= 0.1)
        {
            isGrounded = true;
            animator.SetBool("isJumping", !isGrounded);
            animator.SetBool("isFalling", false);
        }
        //isGrounded = true;
        //animator.SetBool("isJumping", !isGrounded);
        //animator.SetBool("isFalling", false);
    }

    public void OnAttackEnd()
    {
        isAttacking = false;
        animator.SetBool("IsAttackFinished", true);
        if (attackCombo >= 2)
        {
            attackCombo = 0;
            animator.SetBool("CanCombo", false);
        }
        else
        {
            animator.SetBool("CanCombo", true);
        }
    }

    private void Roll()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Objects"), true);
        animator.SetBool("IsRolling", true);
        isRolling = true;

        PlayerAfterImagePool.Instance.GetFromPool();
        lastImageXPos = transform.position.x;
    }

    private void Attack()
    {
        animator.SetBool("IsAttackFinished", false);
        if (Time.time - lastAttackTime > comboResetTime)
        {
            attackCombo = 0;
        }

        attackCombo++;
        lastAttackTime = Time.time;

        if (attackCombo == 1)
        {
            animator.SetTrigger("Attack");
        }
        else if (attackCombo == 2)
        {
            animator.SetTrigger("Attack");
        }

        isAttacking = true;

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<HealthManager>().TakeDamage(10);
            //Debug.Log("Hit: " + enemy.name);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public void OnRollingEnd()
    {
        // Roll finished, reset to normal movement
        isRolling = false;
        animator.SetBool("IsRolling", false);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Objects"), false);
    }

    public void OnDeathAnimationEnd()
    {
        // log the death animation end
        Debug.Log("Death animation ended");
        //animator.enabled = false;
        gameOverMenu.GameOver();
    }
}