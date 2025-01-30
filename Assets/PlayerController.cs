using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded = false;
    private float xVelocity;

    // Attack variables
    private bool isAttacking = false;
    private int attackCombo = 0;
    private float comboResetTime = 1f;
    private float lastAttackTime = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // --- Horizontal Movement ---
        // Only allow movement if NOT attacking
        if (!isAttacking)
        {
            xVelocity = Input.GetAxis("Horizontal") * moveSpeed;
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
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isAttacking) // Disable jump during attack
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
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

        // --- Combo Attack ---
        if (Input.GetKeyDown(KeyCode.J) && isAttacking && animator.GetBool("CanCombo"))
        {
            animator.SetTrigger("Attack");
            attackCombo++;
            lastAttackTime = Time.time;
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
            rb.velocity = new Vector2(0f, rb.velocity.y); // Stop horizontal movement
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
}