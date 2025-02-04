using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    // a slider to represent the health bar
    public Slider healthBar;

    // the max health of the player
    public float maxHealth = 100f;

    // the current health of the player
    public float currentHealth;

    public PixelPool pixelPool;

    public bool isPlayer = false;

    public ScoreManager? scoreManager;

    private bool isDead = false;

    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.value = currentHealth / maxHealth;
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr.color = Color.white;
        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        // if falling off the map, die
        if (transform.position.y < -10 && !isDead)
        {
            Debug.Log("Fell off the map");
            Die();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(10f);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            Heal(10f);
        }
        healthBar.value = currentHealth / maxHealth;
    }


    public void TakeDamage(float damage)
    {
        // if health is already 0, do nothing
        if (currentHealth <= 0)
        {
            return;
        }
        // subtract the damage from the current health
        currentHealth -= damage;
        // update the health bar
        healthBar.value = currentHealth / maxHealth;

        // flash the player sprite
        StartCoroutine(FlashSpriteHit());

        PixelEffect();

        // check if the player is dead
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }

        // apply knockback
        rb.velocity = new Vector2(-transform.localScale.x * damage, 5f);
    }

    public void Die()
    {
        animator.SetBool("IsDead", true);
        if (scoreManager != null)
        {
            scoreManager.IncreaseKillCount();
        }
        isDead = true;
    }

    public void OnDeathAnimationEnd()
    {
        currentHealth = maxHealth;
        SnailEnemyPool.Instance.AddToPool(gameObject);
    }

    public void Heal(float healAmount)
    {
        // add the heal amount to the current health
        currentHealth += healAmount;
        // make sure the current health does not exceed the max health
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        // update the health bar
        healthBar.value = currentHealth / maxHealth;
        // flash the player sprite
        StartCoroutine(FlashSpriteHeal());
    }

    void PixelEffect()
    {
        if (scoreManager == null)
        {
            return;
        }
        for (int i = 0; i < Random.Range(5, 10); i++)  // Random amount of pixels
        {
            GameObject pixel = pixelPool.GetFromPool();

            // Spawn slightly above the enemy's position with some random offset
            float xOffset = Random.Range(-0.2f, 0.2f); // Small random horizontal offset
            float yOffset = Random.Range(0.3f, 0.6f); // Higher spawn position
            pixel.transform.position = transform.position + new Vector3(xOffset, yOffset, 0);

            // Apply more random velocity for a splash effect
            Rigidbody2D rb = pixel.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(Random.Range(-2.5f, 2.5f), Random.Range(3f, 6f));


            // Return pixel after 1 second
            StartCoroutine(ReturnPixelAfterDelay(pixel, 1f));
        }
    }

    private IEnumerator ReturnPixelAfterDelay(GameObject pixel, float delay)
    {
        yield return new WaitForSeconds(delay);
        pixelPool.AddToPool(pixel);
    }

    IEnumerator FlashSpriteHit()
    {
        // set the sprite color to red
        sr.color = Color.red;
        // wait for 0.3 seconds
        yield return new WaitForSeconds(0.3f);
        // set the sprite color back to normal
        sr.color = Color.white;
    }

    IEnumerator FlashSpriteHeal()
    {
        // set the sprite color to green
        sr.color = Color.green;
        // wait for 0.3 seconds
        yield return new WaitForSeconds(0.3f);
        // set the sprite color back to normal
        sr.color = Color.white;
    }
}
