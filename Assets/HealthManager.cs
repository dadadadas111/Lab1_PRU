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

    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(10f);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            Heal(10f);
        }

    }


    public void TakeDamage(float damage)
    {
        // subtract the damage from the current health
        currentHealth -= damage;
        // update the health bar
        healthBar.value = currentHealth / maxHealth;

        // flash the player sprite
        StartCoroutine(FlashSpriteHit());

        // check if the player is dead
        if (currentHealth <= 0)
        {
            Die();
        }

        // apply knockback
        rb.velocity = new Vector2(-transform.localScale.x * 8f, 5f);
    }

    public void Die()
    {
        animator.SetBool("IsDead", true);
        // destroy the player
        //Destroy(transform);
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
