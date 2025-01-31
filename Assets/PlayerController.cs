using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float rollSpeed = 5f;

    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded = false;
    private bool isRolling = false;
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
    }

    void Update()
    {
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
        if (Input.GetKeyDown(KeyCode.K) && isGrounded && !isAttacking) // Disable jump during attack
        {
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
        }
    }

    private void FixedUpdate()
    {
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
        animator.SetBool("isFalling", !isGrounded && rb.velocity.y < 0);

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
        isGrounded = true;
        animator.SetBool("isJumping", !isGrounded);
        animator.SetBool("isFalling", false);
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
        animator.SetBool("IsRolling", true);
        isRolling = true;

        PlayerAfterImagePool.Instance.GetFromPool();
        lastImageXPos = transform.position.x;
    }

    public void OnRollingEnd()
    {
        // Roll finished, reset to normal movement
        isRolling = false;
        animator.SetBool("IsRolling", false);
    }
}